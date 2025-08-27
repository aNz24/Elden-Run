using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "A.I/State/Sleep")]

public class BossSleepState : AIState
{
    [SerializeField] bool sleepAnimationSet = false;
    [SerializeField] string sleepAnimation = "Sleep_01";
    [SerializeField] string wakingAnimation = "Wake_01";

    public bool hasBeenAwakened = false;
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        aiCharacter.navMeshAgent.enabled = false;

        if (!hasBeenAwakened)
        {
            return HasNotBeeenAwkened(aiCharacter);
        }
        else
        {
            return HasBeeenAwkened(aiCharacter);
        }

    }

    private AIState HasBeeenAwkened(AICharacterManager aiCharacter)
    {

        if (aiCharacter.characterCombatManager.currentTarget != null && !aiCharacter.aiCharacteNetworkManager.isAwake.Value)
        {
            aiCharacter.aiCharacteNetworkManager.isAwake.Value = true;
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }
        return this;
    }

    private AIState HasNotBeeenAwkened(AICharacterManager aiCharacter)
    {
        aiCharacter.navMeshAgent.enabled = false;


        if (!sleepAnimationSet && !aiCharacter.aiCharacteNetworkManager.isAwake.Value)
        {
            sleepAnimationSet = false;
            aiCharacter.aiCharacteNetworkManager.sleepAnimation.Value = sleepAnimation;
            aiCharacter.aiCharacteNetworkManager.wakingAnimation.Value = wakingAnimation;
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.aiCharacteNetworkManager.sleepAnimation.Value.ToString(), true);
        }


        if (aiCharacter.characterCombatManager.currentTarget != null && !aiCharacter.aiCharacteNetworkManager.isAwake.Value)
        {
            aiCharacter.aiCharacteNetworkManager.isAwake.Value = true;

            if (!aiCharacter.isPerformingAction && !aiCharacter.isDead.Value)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.aiCharacteNetworkManager.wakingAnimation.Value.ToString(), true);
            }

            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }

        return this;
    }
}
