using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]

public class LightAttackWeaponItemAction : WeaponItemAction
{   //ONE HAND
    [Header("Light Attack")]
    [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
    [SerializeField] string light_Attack_02 = "Main_Light_Attack_02";
    [SerializeField] string light_Jumping_Attack_01 = "Main_Light_Jump_Attack_01";

    [Header("Running Attack")]
    [SerializeField] string run_Attack_01 = "Main_Run_Attack_01";

    [SerializeField] string roll_Attack_01 = "Main_Roll_Attack_01";
    [SerializeField] string backstep_Attack_01 = "Main_Backstep_Attack_01";

    //TWO HAND
    [Header("Light Attack")]
    [SerializeField] string th_light_Attack_01 = "TH_Light_Attack_01";
    [SerializeField] string th_light_Attack_02 = "TH_Light_Attack_02";
    [SerializeField] string th_light_Jumping_Attack_01 = "TH_Light_Jump_Attack_01";


    [Header("Running Attack")]
    [SerializeField] string th_run_Attack_01 = "TH_Run_Attack_01";

    [SerializeField] string th_roll_Attack_01 = "TH_Roll_Attack_01";
    [SerializeField] string th_backstep_Attack_01 = "TH_Backstep_Attack_01";

    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {

        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
            return;

        if (playerPerformingAction.playerCombatManager.isUsingItem)
            return;

        if (playerPerformingAction.playerNetWorkManager.currentStamina.Value <= 0) 
            return;

        if (!playerPerformingAction.characterLocomotionManager.isGrounded)
        {
            PerformJumpingLightAttack(playerPerformingAction ,weaponPerformingAction);
            return;
        }

        if (playerPerformingAction.playerNetWorkManager.isJumping.Value)
            return;

        //SPRITIN ATTACK

        if (playerPerformingAction.characterNetWorkManager.isSprinting.Value)
        {
            PerformRunningAttack(playerPerformingAction,weaponPerformingAction);
            return;
        }


        //ROLLING ATTACK

        if (playerPerformingAction.characterCombatManager.canPerformRollingAttack)
        {
            PerformRollingAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        //BACKSTEP ATTACK

        if (playerPerformingAction.characterCombatManager.canPerformBackstepAttack)
        {
            PerformBackstepAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        playerPerformingAction.characterCombatManager.AttemptCriticalAttack();

        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerNetWorkManager.isTwoHandingWeapon.Value)
        {
            PerformTwoHandLightAttack(playerPerformingAction, weaponPerformingAction);
        }
        else
        {
            PerformMainHandLightAttack(playerPerformingAction, weaponPerformingAction);
        }
      
    }

    private void PerformMainHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // IF WE ARE ATTACKING CURRENTLY , AND WE CAN COMBO. PERFORM THE COMBO ATTACK
        if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02, light_Attack_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
            }
        }
        // OTHERWISE , JUST PERFORM A REGULAR ATTACK
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
        }
    }

    private void PerformTwoHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // IF WE ARE ATTACKING CURRENTLY , AND WE CAN COMBO. PERFORM THE COMBO ATTACK
        if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == th_light_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02, th_light_Attack_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, th_light_Attack_01, true);
            }

        }
        // OTHERWISE , JUST PERFORM A REGULAR ATTACK
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, th_light_Attack_01, true);
        }
    }

    private void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if(playerPerformingAction.playerNetWorkManager.isTwoHandingWeapon.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01, th_run_Attack_01, true);

        }
        else
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01, run_Attack_01, true);
        }
    }

    private void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        playerPerformingAction.playerCombatManager.canPerformRollingAttack = false;

        if (playerPerformingAction.playerNetWorkManager.isTwoHandingWeapon.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingAttack01, th_roll_Attack_01, true);

        }
        else
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingAttack01, roll_Attack_01, true);
        }
    }

    private void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        playerPerformingAction.playerCombatManager.canPerformBackstepAttack = false;

        if (playerPerformingAction.playerNetWorkManager.isTwoHandingWeapon.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepAttack01, th_backstep_Attack_01, true);

        }
        else
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepAttack01, backstep_Attack_01, true);
        }

    }

    private void PerformJumpingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerNetWorkManager.isTwoHandingWeapon.Value)
        {
            PerformTwoHandJumpingLightAttack(playerPerformingAction, weaponPerformingAction);
        }
        else
        {
            PerformMainHandJumpingLightAttack(playerPerformingAction, weaponPerformingAction);
        }

    }

    private void PerformMainHandJumpingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.isPerformingAction)
            return;

        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightJumpingAttack01, light_Jumping_Attack_01, true);
    }

    private void PerformTwoHandJumpingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.isPerformingAction)
            return;

        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightJumpingAttack01,th_light_Jumping_Attack_01, true);
    }

}
