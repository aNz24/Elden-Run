using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] GameObject characterGamObject;
    [SerializeField] GameObject instantiatedGamObject;
    private AICharacterManager aiCharacter;

    [Header("Patrol")]
    [SerializeField] bool hasPatrolPath = false;
    [SerializeField] int patrolPathID = 0;

    [Header("Sleep")]
    [SerializeField] bool isSleeping =false;

    private void Awake()
    {
    }

    private void Start()
    {
        WorldAIManager.instance.SpawnCharacter(this);
        gameObject.SetActive(false);

    }

    public void AttempToSpawnCharacter()
    {
        if(characterGamObject != null)
        {
            instantiatedGamObject = Instantiate(characterGamObject);
            instantiatedGamObject.transform.position = transform.position;
            instantiatedGamObject.transform.rotation = transform.rotation;
            instantiatedGamObject.GetComponent<NetworkObject>().Spawn();
            aiCharacter =instantiatedGamObject.GetComponent<AICharacterManager>();

            if(aiCharacter == null ) 
                return;

            WorldAIManager.instance.AddCharacterToSpawnedCharacterList(aiCharacter);

            if(hasPatrolPath)
                aiCharacter.idle.aiPatrolPath = WorldAIManager.instance.GetAIPatrolPathByID(patrolPathID);

            if (isSleeping)
                aiCharacter.aiCharacteNetworkManager.isAwake.Value = false;
            
        }   
    }

    public void ResetCharacter()
    {
        if (instantiatedGamObject == null)
            return;

        if (aiCharacter == null)
            return;

        instantiatedGamObject.transform.position = transform.position;
        instantiatedGamObject.transform.rotation = transform.rotation;
        aiCharacter.aiCharacteNetworkManager.currentHealth.Value = aiCharacter.aiCharacteNetworkManager.maxHealth.Value;
        aiCharacter.aiCharacterCombatManager.currentTarget = null;
        aiCharacter.aiCharacteNetworkManager.isActive.Value = false;
        aiCharacter.aiCharacteNetworkManager.isActive.Value = true;

        if (aiCharacter.isDead.Value)
        {
            aiCharacter.isDead.Value = false;
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Empty", false, false, true, true, true, true);
        }


        aiCharacter.characterUIManager.ResetCharacterHPBar();

        if (aiCharacter is AIBossCharacterManager)
        {
            AIBossCharacterManager boss = aiCharacter as AIBossCharacterManager;
            boss.aiCharacteNetworkManager.isAwake.Value = false;
            boss.sleepState.hasBeenAwakened = boss.hasBeenAwakened.Value;
            boss.currentState =boss.currentState.SwitchState(boss , boss.sleepState);
        }
    }
}
