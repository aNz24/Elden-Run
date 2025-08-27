using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
public class HeavyAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string heavy_Attack_01 = "Main_Heavy_Attack_01";
    [SerializeField] string heavy_Attack_02 = "Main_Heavy_Attack_02";
    [SerializeField] string heavy_Jumpping_Attack_01 = "Main_Heavy_Jump_Attack_01";
    [SerializeField] string th_heavy_Jumpping_Attack_01 = "TH_Heavy_Jump_Attack_01";

    [SerializeField] string th_heavy_Attack_01 = "TH_Heavy_Attack_01";
    [SerializeField] string th_heavy_Attack_02 = "TH_Heavy_Attack_02";
    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {

        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
            return;

        if (playerPerformingAction.playerNetWorkManager.currentStamina.Value <= 0)
            return;

        if (playerPerformingAction.playerCombatManager.isUsingItem)
            return;

        if (!playerPerformingAction.characterLocomotionManager.isGrounded)
        {
            PerformJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        if (playerPerformingAction.playerNetWorkManager.isJumping.Value)
            return;

        PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerNetWorkManager.isTwoHandingWeapon.Value)
        {
            PerformTwoHandHeavyAttack(playerPerformingAction, weaponPerformingAction);

        }
        else
        {
            PerformMainHandHeavyAttack(playerPerformingAction , weaponPerformingAction);
        }

    }

    private void PerformMainHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // IF WE ARE ATTACKING CURRENTLY , AND WE CAN COMBO. PERFORM THE COMBO ATTACK
        if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, heavy_Attack_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);

            }

        }
        // OTHERWISE , JUST PERFORM A REGULAR ATTACK
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);
        }
    }

    private void PerformTwoHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // IF WE ARE ATTACKING CURRENTLY , AND WE CAN COMBO. PERFORM THE COMBO ATTACK
        if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == th_heavy_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, th_heavy_Attack_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, th_heavy_Attack_01, true);

            }

        }
        // OTHERWISE , JUST PERFORM A REGULAR ATTACK
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, th_heavy_Attack_01, true);
        }
    }

    private void PerformJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerNetWorkManager.isTwoHandingWeapon.Value)
        {
            PerformTwoHandJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }
        else
        {
            PerformMainHandJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }
    }


    private void PerformMainHandJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.isPerformingAction)
            return;

        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyJumpingAttack01, heavy_Jumpping_Attack_01, true);
    }


    private void PerformTwoHandJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.isPerformingAction)
            return;

        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyJumpingAttack01, th_heavy_Jumpping_Attack_01, true);
    }
}
