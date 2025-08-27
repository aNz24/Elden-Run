using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class EvenTriggerWakeNearbyCharacter : MonoBehaviour
{
    [SerializeField] float awakenRadius = 8;

    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        PlayerManager player= other.GetComponent<PlayerManager>();

        if(player == null )
            return;

        Debug.Log("jj");

        Collider[] creaturesInRadius =Physics.OverlapSphere(transform.position,awakenRadius ,  WorldUtilityManager.instance.GetCharacterLayer());
        List<AICharacterManager> creaturesToWake = new List<AICharacterManager>();

        for (int i = 0; i < creaturesInRadius.Length; i++)
        {
            AICharacterManager aiCharacter = creaturesInRadius[i].GetComponentInParent<AICharacterManager>();

            if(aiCharacter == null ) 
                continue;

            if(aiCharacter.isDead.Value) 
                continue;

            if(aiCharacter.aiCharacteNetworkManager.isAwake.Value) 
                continue;

            if(!creaturesToWake.Contains(aiCharacter) ) 
                creaturesToWake.Add(aiCharacter);
        }

        for (int i = 0; i < creaturesToWake.Count; i++)
        {
            creaturesToWake[i].aiCharacterCombatManager.SetTarget(player);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, awakenRadius);
    }
}
