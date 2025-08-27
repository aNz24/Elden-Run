using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "A.I/State/Attack")]
public class AttackState : AIState
{
    [HideInInspector] public AICharacterAttackAction currentAttack;
    [HideInInspector] public bool willPerformCombo = false;

    [Header("State Flags")]
    protected bool hasperformedAttack = false;
    protected bool hasperformedCombo = false;

    [Header("Pivot After Attack")]
    [SerializeField] protected bool pivotAfterAttack = false;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if(aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
            return SwitchState(aiCharacter, aiCharacter.idle);


        aiCharacter.aiCharacterCombatManager.RotateTowardTargetWhilstAttacking(aiCharacter);

        aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);


        if(willPerformCombo && !hasperformedCombo)
        {
            if(currentAttack.comboAction != null)
            {
                //hasperformedCombo = true;
                //currentAttack.comboAction.AttempptToPeformAction(aiCharacter);
            }
        }

        if (aiCharacter.isPerformingAction)
            return this;

        if (!hasperformedAttack)
        {
            if(aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0) 
                return this;

            PerformAttack(aiCharacter);

            return this;
        }

        if (pivotAfterAttack)
            aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
        return SwitchState(aiCharacter, aiCharacter.combatStance);
    }

    protected void PerformAttack(AICharacterManager aiCharacter)
    {
        hasperformedAttack = true;
        currentAttack.AttempptToPeformAction(aiCharacter);
        aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);

        hasperformedAttack = false;
        hasperformedCombo = false;   
    }
}
