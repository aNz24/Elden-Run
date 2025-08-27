using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ash Of War/Parry")]
public class ParryAshOfWar : AshOfWar
{
    public override void AttempToPerformAction(PlayerManager playerPerformingAction)
    {
        base.AttempToPerformAction(playerPerformingAction);

        if (!CanIUseThisAbility(playerPerformingAction))
            return;

        DeductStaminaCost(playerPerformingAction);
        DeductFocusPointCost(playerPerformingAction);
        PerfromParryTypeBasedOnWeapon(playerPerformingAction);

    }

    public override bool CanIUseThisAbility(PlayerManager playerPerformingAction)
    {
        if (playerPerformingAction.isPerformingAction)
        {
            Debug.Log("CANNOT PERFORM ASH OF WAR: YOU ARE ALREADY PERFROMING AN ACTION");
            return false;
        }

        if (playerPerformingAction.playerNetWorkManager.isJumping.Value)
        {
            Debug.Log("CANNOT PERFORM ASH OF WAR: YOU ARE JUMPING");
            return false; 
        }

        if (!playerPerformingAction.playerLocomotionManager.isGrounded)
        {
            Debug.Log("CANNOT PERFORM ASH OF WAR: YOU ARE NOT GROUNDED");
            return false;
        }

        if (playerPerformingAction.playerNetWorkManager.currentStamina.Value <= 0)
        {
            Debug.Log("CANNOT PERFORM ASH OF WAR: OUT OF STAMINA");
            return false;   
        }
        return true;

    }

    private void PerfromParryTypeBasedOnWeapon(PlayerManager playerPerformingAction)
    {
        WeaponItem weaponBeingUsed = playerPerformingAction.playerCombatManager.currentWeaponBeingUsed;

        switch (weaponBeingUsed.weaponClass)
        {
            case WeaponClass.StraightSword:
                break;
            case WeaponClass.Spear:
                break;
            case WeaponClass.MediumShield:
                playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Medium_Parry_01", true);
                break;
            case WeaponClass.Fist:
                break;
            case WeaponClass.LightShield:
                playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Fast_Parry_01 ", true);
                break;
            default:
                break;
        }
    }
}
