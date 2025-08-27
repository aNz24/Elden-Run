using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WorldSFXManger : MonoBehaviour
{
    public static WorldSFXManger instance;

    [Header("Boss Track")]
    [SerializeField] AudioSource bossIntroPlayer;
    [SerializeField] AudioSource bossLoopPlayer;

    [Header("Damage Sounds")]
    public AudioClip[] physicalDamageSFX;

    [Header("Action Sounds")]
    public AudioClip pickUpItemSFX; 
    public AudioClip rollSFX;   
    public AudioClip stanceBreakSFX;   
    public AudioClip criticalSFX;
    public AudioClip[] releaseArrowSFX;
    public AudioClip[] notchArrowSFX;
    public AudioClip healingFlaskSFX;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBossTrack(AudioClip introTrack , AudioClip loopTrack)
    {
        bossIntroPlayer.volume = .1f;
        bossIntroPlayer.clip = introTrack;
        bossIntroPlayer.loop = false;
        bossIntroPlayer.Play();

        bossLoopPlayer.volume = .1f;
        bossLoopPlayer.clip = loopTrack;
        bossLoopPlayer.loop = true;
        bossLoopPlayer.PlayDelayed(bossIntroPlayer.clip.length);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
    {
        int index  = Random.Range(0, array.Length);

        return array[index];
    }

    public void StopBossMusic()
    {
        StartCoroutine(FadeOutBossMusicThenStop());
    }

    private IEnumerator FadeOutBossMusicThenStop()
    {
        while(bossLoopPlayer.volume > 0)
        {
            bossLoopPlayer.volume -=Time.deltaTime;
            bossIntroPlayer.volume -= Time.deltaTime;
            yield return null;
        }

        bossIntroPlayer.Stop();
        bossLoopPlayer.Stop();

    }

    public void AlertNearbyCharacterToSound(Vector3 positionOfSound, float rangeOfSound)
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        Collider[] characterColliders = Physics.OverlapSphere(positionOfSound, rangeOfSound);

        List<AICharacterManager> charaterToAlert = new List<AICharacterManager>();

        for (int i = 0; i < characterColliders.Length; i++)
        {
            AICharacterManager aiCharacter = characterColliders[i].GetComponent<AICharacterManager>();

            if(aiCharacter ==  null)
                continue;

            charaterToAlert.Add(aiCharacter);
        }

        for (int i = 0; i < charaterToAlert.Count; i++)
        {
            charaterToAlert[i].aiCharacterCombatManager.AlertCharacterToSound(positionOfSound);
        }
    }

    /*
    public AudioClip ChooseRandomFootStepsSoundBasedOnGround(GameObject steppedOnObject , CharacterManager character)
    {
        if (steppedOnObject.tag == "Dirt")
        {
            return ChooseRandomSFXFromArray(character.characterSFXManager.footStepsDirt);
        }
        else if (steppedOnObject.tag == "Stone")
        {
            return ChooseRandomSFXFromArray(character.characterSFXManager.footStepsStone);

        }

        return null;
    }
    */

}
