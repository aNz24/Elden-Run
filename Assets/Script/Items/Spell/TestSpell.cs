using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Spells/Test Spell")]
public class TestSpell : SpellItem
{
    public override void AttempToCastSpell(PlayerManager player)
    {
        base.AttempToCastSpell(player); 

        if (!CanICastThisSpell(player))
            return;
    
        
        if (player.playerNetWorkManager.isUsingRightHand.Value)
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(mainHandSpellAnimation, true);   
        }
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(offHandSpellAnimation, true);

        }
    }

    public override void SuccesfullyCastSpell(PlayerManager player)
    {
        base.SuccesfullyCastSpell(player);

        Debug.Log("CASTED SPELL");
    }

    public override void InstantiateWarmUpSpellFX(PlayerManager playerManager)
    {
        base.InstantiateWarmUpSpellFX(playerManager);

        Debug.Log(" FX");

    }

    public override bool CanICastThisSpell(PlayerManager player)
    {
        if(player.isPerformingAction) 
            return false;

        if (player.playerNetWorkManager.isJumping.Value)
            return false;

        if (player.playerNetWorkManager.currentHealth.Value <= 0)
            return false;

        if (player.playerNetWorkManager.currentFocusPoints.Value <= 0)
            return false;

        if (player.playerNetWorkManager.currentStamina.Value <= 0)
            return false;

        return true;
    }
}
