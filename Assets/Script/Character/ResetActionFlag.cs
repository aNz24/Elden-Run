using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager character;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(character == null)
        {
            character = animator.GetComponent<CharacterManager>();
        }

        character.isPerformingAction = false;
        character.characterAnimatorManager.applyRootMotion = false;
        character.characterLocomotionManager.canRotate = true;
        character.characterLocomotionManager.canMove = true;
        character.characterLocomotionManager.canRun = true;
        character.characterLocomotionManager.canRoll = true;
        character.characterLocomotionManager.isRolling = false;
        character.characterCombatManager.DisableCanDoCombo();
        character.characterCombatManager.DisableCanDoRollingAttack();
        character.characterCombatManager.DisableCanDoBackstepAttack();

        if(character.characterEffectManager.activeSpellWarnUpFX != null) 
            Destroy(character.characterEffectManager.activeSpellWarnUpFX);

        if (character.characterEffectManager.activeQuickSlotItemFX != null)
            Destroy(character.characterEffectManager.activeQuickSlotItemFX);

        if (character.IsOwner)
        {
            character.characterNetWorkManager.isJumping.Value = false;
            character.characterNetWorkManager.isInvulnerable.Value = false;
            character.characterNetWorkManager.isAttacking.Value = false;
            character.characterNetWorkManager.isRipostable.Value = false;
            character.characterNetWorkManager.isBeingCriticallyDamaged.Value = false;
            character.characterNetWorkManager.isParrying.Value = false;
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
