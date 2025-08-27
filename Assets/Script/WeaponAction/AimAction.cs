using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Aim Action")]
public class AimAction : WeaponItemAction
{
    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.playerLocomotionManager.isGrounded)
            return;

        if (playerPerformingAction.playerNetWorkManager.isJumping.Value)
            return;

        if (playerPerformingAction.playerLocomotionManager.isRolling)
            return;

        if (playerPerformingAction.playerNetWorkManager.isLockedOn.Value)
            return;

        if (playerPerformingAction.playerCombatManager.isUsingItem)
            return;

        if (playerPerformingAction.IsOwner)
        {
            if (!playerPerformingAction.playerNetWorkManager.isTwoHandingWeapon.Value)
            {
                if (playerPerformingAction.playerNetWorkManager.isUsingRightHand.Value)
                {
                    playerPerformingAction.playerNetWorkManager.isTwoHandingRightWeapon.Value = true;
                }
                else if (playerPerformingAction.playerNetWorkManager.isUsingLeftHand.Value)
                {
                    playerPerformingAction.playerNetWorkManager.isTwoHandingLeftWeapon.Value = true;
                }

            }
            playerPerformingAction.playerNetWorkManager.isAiming.Value = true;
        }
    }
}
