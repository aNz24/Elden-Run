using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/State/Combat Stance")]
public class CombatStanceState : AIState
{
    [Header("Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks;
    [SerializeField] protected List<AICharacterAttackAction> potentialAttacks;
    [SerializeField] private AICharacterAttackAction choosenAttack;
    [SerializeField] private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;



    [Header("Combo")]
    [SerializeField] protected bool canPerfromCombo =false;
    [SerializeField] protected int chanceToPefromCombo =25;
    protected bool hasRolledForComboChance =false;

    [Header("Engagement Distance")]
    [SerializeField] public float maximunEngagementDistance = 5;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;

        if(aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
            aiCharacter.aiCharacterCombatManager.SetTarget(null);


        if(!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;


        if (aiCharacter.aiCharacterCombatManager.enablePivot)
        {
            if (!aiCharacter.aiCharacteNetworkManager.isMoving.Value)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
        }

    

        //ROTATE TO FACE OUR TARGET
        aiCharacter.aiCharacterCombatManager.RotateTowardAgent(aiCharacter); 


        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if (!hasAttack)
        {
            GetNewAttack(aiCharacter);

        }
        else
        {
            aiCharacter.attack.  currentAttack = choosenAttack;
            return SwitchState(aiCharacter, aiCharacter.attack);
        }

        if(aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximunEngagementDistance)
            return SwitchState(aiCharacter,aiCharacter.pursueTarget);

        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this;
    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
        potentialAttacks = new List<AICharacterAttackAction>();

    

        foreach (var potentialAttack in aiCharacterAttacks)
        {
            if(potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;

            if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;

            if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            potentialAttacks.Add(potentialAttack);

        }

        if (potentialAttacks.Count < 0)
            return;

        var totalWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;
        }

        var randomWeightValue = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if (randomWeightValue <= processedWeight)
            {
                choosenAttack = attack;
                previousAttack = choosenAttack;
                hasAttack = true;
                return;
            }
        }
    }

    protected virtual bool RollForOutcomeChance(int outcomeChance)
    {
        bool outcomeWillBePeformed = false;

        int randomPercentage =Random.Range(0, 100);

        if(randomPercentage < outcomeChance)
        {
            outcomeWillBePeformed = true;
        }

        return outcomeWillBePeformed; 
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);

        hasRolledForComboChance = false;
        hasAttack = false;
    }
}
