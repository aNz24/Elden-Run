using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Character Actions/Weapon Actions/Incantation Action")]
public class CastIncantationAction : WeaponItemAction
{
    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
            return;

        if (playerPerformingAction.playerNetWorkManager.currentStamina.Value <= 0)
            return;

        if (!playerPerformingAction.characterLocomotionManager.isGrounded)
            return;

        if (playerPerformingAction.playerInventoryManager.currentSpell == null)
            return;

        if (playerPerformingAction.playerInventoryManager.currentSpell.spellClass != SpellClass.Incantation)
            return;

        if (playerPerformingAction.playerCombatManager.isUsingItem)
            return;

        if (playerPerformingAction.IsOwner)
            playerPerformingAction.playerNetWorkManager.isAttacking.Value = true;

        CastIncantation(playerPerformingAction , weaponPerformingAction);
    }

    private void CastIncantation(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        playerPerformingAction.playerInventoryManager.currentSpell.AttempToCastSpell(playerPerformingAction);
    }
}
