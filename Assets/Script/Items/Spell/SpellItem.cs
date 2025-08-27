using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
    [Header("Spell Type")]
    public SpellClass spellClass;

    [Header("Spell Modifiers")]
    //public float fullChargeEffectMutiplier = 2;

    [Header("Spell Costs")]
    public int spellSlotsUsed = 1;
    public int staminaCost = 25;
    public int focusPointCost = 25;
    

    [Header("Spell FX")]
    [SerializeField] protected GameObject spellCastWarmUpFX;
    [SerializeField] protected GameObject spellCastReleaseFX;

    [Header("Animations")]
    [SerializeField] protected string mainHandSpellAnimation;
    [SerializeField] protected string offHandSpellAnimation;

    [Header("Sound FX")]
    public AudioClip warmUpSoundFX;
    public AudioClip releaseSoundFX;


    public virtual void AttempToCastSpell(PlayerManager player)
    {
            
    }


    public virtual void InstantiateWarmUpSpellFX(PlayerManager playerManager)
    {


    }


    public virtual void SuccesfullyCastSpell(PlayerManager player)
    {
        if (player.IsOwner)
        {
            player.playerNetWorkManager.currentFocusPoints.Value -= focusPointCost;
            player.playerNetWorkManager.currentStamina.Value -= staminaCost;
        }
    }


    public virtual bool CanICastThisSpell(PlayerManager player)
    {
        if (player.playerNetWorkManager.currentFocusPoints.Value <= focusPointCost)
            return false;

        if (player.playerNetWorkManager.currentStamina.Value <= staminaCost)
            return false;

        if (player.isPerformingAction)
            return false;

        if (player.playerNetWorkManager.isJumping.Value)
            return false;

        return true;
    }
}