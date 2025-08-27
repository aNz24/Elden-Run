using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSFXManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] attackGrunts;

    [Header("FootSteps")]
    [SerializeField] protected AudioClip[] footSteps;


    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true,float pitchRandom = .1f) 
    {
        audioSource.PlayOneShot(soundFX, volume);
        audioSource.pitch = 1;

        if(randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSFXManger.instance.rollSFX);
    }

    public void PlayDamageGrunt()
    {
        if(damageGrunts.Length > 0)
            PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(damageGrunts));
    }

    public virtual void PlayAttackGrunt()
    {   
        if (attackGrunts.Length > 0)
            PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(attackGrunts));

    }

    public virtual void PlayFootStepSFX()
    {
        if (footSteps.Length > 0)
            PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(footSteps));
    }

    public virtual void PlayStanceBreakSFX()
    {
        audioSource.PlayOneShot(WorldSFXManger.instance.stanceBreakSFX);
    }

    public virtual void PlayCriticalSFX()
    {
        audioSource.PlayOneShot(WorldSFXManger.instance.criticalSFX);
    }

    public virtual void PlayBlockSFX()
    {

    }
}
