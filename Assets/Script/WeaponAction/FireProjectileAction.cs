using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Fire Projectile Action")]
public class FireProjectileAction : WeaponItemAction 
{
    [SerializeField] ProjectileSlot projectileSlot;
    public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
            return;


        if (playerPerformingAction.playerNetWorkManager.currentStamina.Value <= 0)
            return;

        if (playerPerformingAction.playerCombatManager.isUsingItem)
            return;

        RangedProjectileItem  projectileItem = null;

        switch (projectileSlot)
        {
            case ProjectileSlot.Main:
                projectileItem = playerPerformingAction.playerInventoryManager.mainProjectile;
                break;
            case ProjectileSlot.Secondary:
                projectileItem = playerPerformingAction.playerInventoryManager.secondaryProjectile;
                break;
            default:
                break; 
        }

        if (projectileItem == null)
            return;

        if (!playerPerformingAction.IsOwner)
            return;
       

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

        if (!playerPerformingAction.playerNetWorkManager.hasArrowNotched.Value)
        {
            playerPerformingAction.playerNetWorkManager.hasArrowNotched.Value = true;
            bool canIDrawAProjectile =  CanIFireThisProjectile(weaponPerformingAction , projectileItem);

            if (!canIDrawAProjectile)
                return;

            if (projectileItem.currentAmmoAmount <= 0)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Out_Of_Ammo_01", true);
                return; 
            }

            playerPerformingAction.playerCombatManager.currentProjectileBeingUsed = projectileSlot;
            playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Bow_Draw_01", true);
            playerPerformingAction.playerNetWorkManager.NotifyTheServerOfDrawProjectileServerRpc( projectileItem.itemID);

        }
    }

    private bool CanIFireThisProjectile(WeaponItem weaponPerformingAction, RangedProjectileItem projectileItem)
    {

        return true;
    }

}
