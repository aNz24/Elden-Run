using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee Action")]
public class OffHandMeleeAction : WeaponItemAction
{

    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

        //CHECK FOR
        if (!playerPerformingAction.playerCombatManager.canBlock)
            return;

        if (playerPerformingAction.playerCombatManager.isUsingItem)
            return;

        if (playerPerformingAction.playerNetWorkManager.isAttacking.Value)
        {
            if(playerPerformingAction.IsOwner)
                playerPerformingAction.playerNetWorkManager.isBlocking.Value = false;

            return;
        }

        if (playerPerformingAction.playerNetWorkManager.isBlocking.Value)
            return;
        if (playerPerformingAction.IsOwner)
            playerPerformingAction.playerNetWorkManager.isBlocking.Value = true;

    }
}
