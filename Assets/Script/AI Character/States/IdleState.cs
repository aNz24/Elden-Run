using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/State/Idle")]
public class IdleState : AIState
{
    [Header("Idle Options")]
    public IdleStateMode idleStateMode;

    [Header("Patrol Options")]
    public AIPatrolPath aiPatrolPath;
    [SerializeField] bool hasFoundClosestPointNearCharacterSpawn = false;
    [SerializeField] bool patrolComplete = false;
    [SerializeField] bool repeatPatrol = false;
    [SerializeField] int patrolDestinationIndex;
    [SerializeField] bool hasPatrolDestination = false;
    [SerializeField] Vector3 currentPatrolDestination;
    [SerializeField] float distanceFromCurrentDestination;
    [SerializeField] float timeBetweenPatrols = 15;
    [SerializeField] float restTimer = 0;

    [Header("Sleep Option")]
    public bool willInvestigateSound = true;
    [SerializeField] bool sleepAnimationSet = false; 
    [SerializeField] string sleepAnimation = "Sleep_01";
    [SerializeField] string wakingAnimation = "Wake_01";
 
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if(aiCharacter.aiCharacteNetworkManager.isAwake.Value) 
            aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);

        switch (idleStateMode)
        {
            case IdleStateMode.Idle: return Idle(aiCharacter);
            case IdleStateMode.Patrol: return Patrol(aiCharacter);
            case IdleStateMode.Sleep: return SleepUntilDisturbed(aiCharacter);
            default:
                break;
        }

        return this;
    }

    protected virtual AIState Idle(AICharacterManager aiCharacter)
    {
        if (aiCharacter.characterCombatManager.currentTarget != null)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }
        else
        {
            return this;
        }
    }

    protected virtual AIState Patrol(AICharacterManager aiCharacter)
    {
        if (!aiCharacter.aiCharacterLocomotionManager.isGrounded)
            return this;

        if (aiCharacter.isPerformingAction)
        {
            aiCharacter.navMeshAgent.enabled = false;
            aiCharacter.characterNetWorkManager.isMoving.Value = false;
            return this;
        }

        if(!aiCharacter.navMeshAgent.enabled) 
            aiCharacter.navMeshAgent.enabled = true;

        if(aiCharacter.aiCharacterCombatManager.currentTarget != null)
            return SwitchState(aiCharacter ,aiCharacter.pursueTarget); 

        if (patrolComplete && repeatPatrol)
        {
            if (timeBetweenPatrols > restTimer)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.characterNetWorkManager.isMoving.Value=false;
                restTimer += Time.deltaTime;
            }
            else
            {
                patrolDestinationIndex = -1;
                hasPatrolDestination = false;
                currentPatrolDestination = aiCharacter.transform.position;
                patrolComplete = false;
                restTimer = 0;
            }
        }
        else if(patrolComplete && !repeatPatrol) 
        {
            aiCharacter.navMeshAgent.enabled = false;
            aiCharacter.characterNetWorkManager.isMoving.Value = false;
        }

        if (hasPatrolDestination)
        {
            distanceFromCurrentDestination = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination);

            if (distanceFromCurrentDestination > 2)
            {
                aiCharacter.navMeshAgent.enabled=true;
                aiCharacter.aiCharacterLocomotionManager.RotateTowardAgent(aiCharacter);
            }
            else
            {
                currentPatrolDestination = aiCharacter.transform.position;
                hasPatrolDestination= false;
            }
        }
        else
        {
            patrolDestinationIndex += 1;

            if (patrolDestinationIndex > aiPatrolPath.patrolPoints.Count - 1)
            {
                patrolComplete=true;
                return this;
            }
            
            if (!hasFoundClosestPointNearCharacterSpawn)
            {
                hasFoundClosestPointNearCharacterSpawn = true;
                float closetDistance = Mathf.Infinity;  

                for (int i = 0; i < aiPatrolPath.patrolPoints.Count; i++)
                {
                    float distanceFromThisPoint = Vector3.Distance(aiCharacter.transform.position, aiPatrolPath.patrolPoints[i]);

                    if (distanceFromThisPoint < closetDistance)
                    {
                        closetDistance = distanceFromThisPoint;
                        patrolDestinationIndex = i;
                        currentPatrolDestination = aiPatrolPath.patrolPoints [i];
                    }
                }
            }
            else
            {
                currentPatrolDestination = aiPatrolPath.patrolPoints[patrolDestinationIndex];
            }

            hasPatrolDestination = true;
        }

        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(currentPatrolDestination , path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this; 
    }

    protected virtual AIState SleepUntilDisturbed(AICharacterManager aiCharacter)
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

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);

        sleepAnimationSet = false;
    }

}
