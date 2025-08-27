using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshOfWar : Item
{
    [Header("Ash Of War Information")]
    public WeaponClass[] useableWeaponClasses;
    [Header("Cost")]
    public int focusPointCost = 20;
    public int  staminaCost = 20;

    public virtual void AttempToPerformAction(PlayerManager playerPerformingAction)
    {
        
    }

    public virtual bool CanIUseThisAbility(PlayerManager playerPerformingAction)
    {
        return false;
    }

    protected virtual void DeductStaminaCost(PlayerManager playerPerformingAction)
    {
        playerPerformingAction.playerNetWorkManager.currentStamina.Value -= staminaCost;
    }

    protected virtual void DeductFocusPointCost(PlayerManager playerPerformingAction)
    {

    }
}

