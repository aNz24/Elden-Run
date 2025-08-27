using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Rendering;

public class AICharacterCombatManager : CharacterCombatManager
{
    protected AICharacterManager aiCharacter;

    [Header("Action Recovery")]
    public float actionRecoveryTimer = 0f;

    [Header("Pivot")]
    public bool enablePivot = true; 

    [Header("Target Information")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetDirection;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    public float minimumFOV = -35;
    public float maximumFOV = 35;
    public float attackRotationSpeed = 25;

    [Header("Stance Setting")]
    public float maxStance = 150;
    public float currentStance;
    [SerializeField] float stanceRegeneratedPersecond = 15f;
    [SerializeField] bool ignoreStanceBreak = false;

    [Header("Stance Timer")]
    [SerializeField] float stanceRegenerationTimer = 0;
    private float stanceTickTimer = 0;
    [SerializeField] float defaultTimeUntilStanceRegenerationBegins =15;

    [Header("DEBUG")]
    [SerializeField] bool investigateSound =false;
    [SerializeField] Vector3 positionOfSound = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();
        lockOnTranfrom = GetComponentInChildren<LockOnTransform>().transform;
    }

    private void Update()
    {
        HandleStanceBreak();

        if (investigateSound)
        {
            investigateSound =false;
            AlertCharacterToSound(positionOfSound);
        }
    }

    public void AwardRunesOnDeath(PlayerManager player)
    {
        // CHECK IF PLAYER IS FIRENDLY
        if (player.characterGroup == CharacterGroup.Team02)
            return;

        //if (NetworkManager.Singleton.IsHost)
        //{

        //}

        //AWARD RUNES
        player.playerStatManager.AddRunes(aiCharacter.characterStatManager.runesDroppedOnDeath);


    }

    private void HandleStanceBreak()
    {
        if (!aiCharacter.IsOwner)
            return;

        if (aiCharacter.isDead.Value)
            return;

        if (stanceRegenerationTimer > 0)
        {
            stanceRegenerationTimer -= Time.deltaTime;
        }
        else
        {
            stanceRegenerationTimer = 0;

            if (currentStance < maxStance)
            {
                stanceTickTimer += Time.deltaTime;  

                if (stanceTickTimer >= 1)
                {
                    stanceTickTimer = 0;
                    currentStance += stanceRegeneratedPersecond;
                }
            }
            else
            {
                currentStance = maxStance;
            }
        }
           
        if (currentStance <= 0)
        {
            DamageIntensity previousDamageIntensity = WorldUtilityManager.instance.GetDamageIntensityBasedOnPoiseDamage(previousPoiseDamageTaken);

            if (previousDamageIntensity == DamageIntensity.Colossal)
            {
                currentStance = 1;
                return;
            }

            currentStance = maxStance;

            if (ignoreStanceBreak)
                return;

            aiCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Stance_Break_01" , true);
        }

    }

    public void DamageStance(int stanceDamage)
    {
        stanceRegenerationTimer = defaultTimeUntilStanceRegenerationBegins;

        currentStance -= stanceDamage;
    }

    public virtual void AlertCharacterToSound(Vector3 positionOfSound)
    {
        if (!aiCharacter.IsOwner)
            return;

       if(aiCharacter.isDead.Value)
            return;

        if (aiCharacter.idle == null)
            return;

        if (aiCharacter.investigateSound == null)
            return;

        if (!aiCharacter.idle.willInvestigateSound)
            return; 

        if (aiCharacter.idle.idleStateMode == IdleStateMode.Sleep && !aiCharacter.aiCharacteNetworkManager.isAwake.Value)
        {
            aiCharacter.aiCharacteNetworkManager.isAwake.Value = true;
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.aiCharacteNetworkManager.wakingAnimation.Value.ToString() , true);
        }

        aiCharacter.investigateSound.positionOfSound = positionOfSound; 
        aiCharacter.currentState = aiCharacter.currentState.SwitchState(aiCharacter, aiCharacter.investigateSound);

    }

    public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
    {
        if(currentTarget != null)
            return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayer());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if(targetCharacter == null)
                continue;

            if (targetCharacter == aiCharacter)
                continue;
            if (targetCharacter.isDead.Value)
                continue;

            //CAN I ATTACK THIS CHARACTER,  MAKE THEM MY CHARACTER
            if (WorldUtilityManager.instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfTarget = Vector3.Angle(targetDirection , aiCharacter.transform.forward);

                if(angleOfTarget > minimumFOV && angleOfTarget < maximumFOV)
                {
                    if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTranfrom.position, targetCharacter.characterCombatManager.lockOnTranfrom.position,
                        WorldUtilityManager.instance.GetEnviroLayer()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTranfrom.position, targetCharacter.characterCombatManager.lockOnTranfrom.position);
                    }
                    else
                    {
                        targetDirection = targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform,targetDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);

                        if(enablePivot)
                            PivotTowardsTarget(aiCharacter);

                    }
                }
            }

        }
    }

    public  virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        //
        if (aiCharacter.isPerformingAction)
            return;

        if (viewableAngle >= 20 && viewableAngle <= 90)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right90", true);
        }
        else if (viewableAngle <= -20 && viewableAngle >= -90)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left90", true);
        }
        else if (viewableAngle > 90 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right180", true);
        }
        else if (viewableAngle < -90 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left180", true);
        }


    }

    public virtual void PivotTowardsPosition(AICharacterManager aiCharacter, Vector3 position) 
    {
        //
        if (aiCharacter.isPerformingAction)
            return;

        Vector3 targetsDirection = position - aiCharacter.transform.position;
        float viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(aiCharacter.transform, targetsDirection);

        if (viewableAngle >= 20 && viewableAngle <= 90)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right90", true);
        }
        else if (viewableAngle <= -20 && viewableAngle >= -90)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left90", true);
        }
        else if (viewableAngle > 90 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right180", true);
        }
        else if (viewableAngle < -90 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left180", true);
        }


    }

    public void RotateTowardAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacteNetworkManager.isMoving.Value)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }

    public void RotateTowardTargetWhilstAttacking(AICharacterManager aiCharacter)
    {
        if (currentTarget == null)
            return;

        if(!aiCharacter.characterLocomotionManager.canRotate)
            return;

        if (!aiCharacter.isPerformingAction)
            return;

        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if(targetDirection == Vector3.zero)
            targetDirection = aiCharacter.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation , targetRotation , attackRotationSpeed * Time.deltaTime);
    }

    public void HandleActionRecovery(AICharacterManager aiCharacter)
    {
        if(actionRecoveryTimer  > 0)
        {
            if (!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }
}
