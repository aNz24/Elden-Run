﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;

    [Header("Music")]
    [SerializeField] AudioClip bossIntroClip;
    [SerializeField] AudioClip bossBattleLoopClip;

    [Header("Status")]
    public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone , NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenAwakened = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone , NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone , NetworkVariableWritePermission.Owner);
    [SerializeField] List<FogWallInteractable> fogWalls;
    [SerializeField] string sleepAnimation;
    [SerializeField] string awakenAnimation;

    [Header("Phase Shift")]
    public float minimumHealthPercentageToShift = 50;
    [SerializeField] string phaseShiftAnimation = "Phase_Change_01";
    [SerializeField] CombatStanceState phase02CombatStanceState;

    [Header("State")]
    public BossSleepState sleepState;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
        OnBossFightIsActiveChanged(false ,bossFightIsActive.Value);

        if (IsOwner)
        {
            sleepState = Instantiate(sleepState);
            currentState = sleepState;
        }

        if (IsServer)
        {

            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
            }
            else
            {

                hasBeenDefeated.Value = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];
                hasBeenAwakened.Value = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];
                sleepState.hasBeenAwakened = hasBeenAwakened.Value;

            }


            StartCoroutine(GetFogWallsFromWorldObjectManager());

            if (hasBeenAwakened.Value)
            {
                for ( int i = 0;  i < fogWalls.Count;  i++)
                {
                    fogWalls[i].isActive.Value = true;
                }
            }

            if (hasBeenDefeated.Value)
            {
                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = false;
                }

                aiCharacteNetworkManager.isActive.Value = false;
            }
        }

        if (!hasBeenAwakened.Value)
        {
            animator.Play(sleepAnimation);
        }

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;

    }

    private IEnumerator GetFogWallsFromWorldObjectManager()
    {
        while(WorldObjectManager.instance.fogWalls.Count == 0) 
            yield return new WaitForEndOfFrame();


        fogWalls = new List<FogWallInteractable>();

        foreach (var fogWall in WorldObjectManager.instance.fogWalls)
        {
            if (fogWall.fogWallID == bossID)
                fogWalls.Add(fogWall);
        }
    } 

    public override IEnumerator ProcessDeathEvent(bool manaullySelectDeathAnimation = false)
    {
        PlayerUIManager.instance.playerUIPopUpManager.SendBossDefeatedPopUp("GREAT FOE FELLED");

        if (IsOwner)
        {
            characterNetWorkManager.currentHealth.Value = 0;
            isDead.Value = true;
            bossFightIsActive.Value = false;


            foreach (var fogWall in fogWalls)
            {
                fogWall.isActive.Value = false;
            }

            if (!manaullySelectDeathAnimation)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            }

            hasBeenDefeated.Value = true;

            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }

            WorldSaveGameManager.instance.SaveGame();
        }
 
        yield return new WaitForSeconds(5f);
    }

    public void WakeBoss()
    {
        if(IsOwner)
        {
            if (!hasBeenAwakened.Value)
            {
                characterAnimatorManager.PlayTargetActionAnimation(awakenAnimation, true);
            }

            bossFightIsActive.Value = true;
            hasBeenAwakened.Value = true;
            aiCharacteNetworkManager.isAwake.Value = true;  
            currentState = idle;

            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }

            for (int i = 0; i < fogWalls.Count; i++)
            {
                fogWalls[i].isActive.Value = true;
            }

          
        }

       

    }

    private void OnBossFightIsActiveChanged(bool oldStatus , bool newStatus)
    {
        // CREATE A HP BAR FOR EACH BOSS THAT IS IN THE FIGHT
        if(bossFightIsActive.Value)
        {
            WorldSFXManger.instance.PlayBossTrack(bossIntroClip, bossBattleLoopClip);

            GameObject bossHealthBar =
                        Instantiate(PlayerUIManager.instance.playerUIHudManager.bossHealthBarObject, PlayerUIManager.instance.playerUIHudManager.bossHealthBarParent);

            UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
            bossHPBar.EnableBossHPBar(this);
            PlayerUIManager.instance.playerUIHudManager.currentBossHealthBar = bossHPBar;
        }
        else
        {
            WorldSFXManger.instance.StopBossMusic();
        }

        
    }

    public void PhaseShift()
    {
        characterAnimatorManager.PlayTargetActionAnimation(phaseShiftAnimation, true);
        combatStance = Instantiate(phase02CombatStanceState);
        currentState = combatStance;
    }

}   
