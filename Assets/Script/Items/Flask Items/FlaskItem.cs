using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Consumeables/Flask")]
public class FlaskItem : QuickSlotItem
{
    [Header("Flask Type")]
    public bool healthFlask = true;

    [Header("Restoration Value")]
    [SerializeField] int flaskRestoration = 50;

    [Header("Empty Flask")]
    public GameObject emptyFlaskItem;
    public string emptyFlaskAnimation;


    public override bool CanIUseThisItem(PlayerManager player)
    {
        if (!player.playerCombatManager.isUsingItem && player.isPerformingAction)
            return false;

        if(player.playerNetWorkManager.isAttacking.Value)
            return false;
        
        return true;
    }

    public override void AttemptToUseItem(PlayerManager player)
    {
        if (!CanIUseThisItem(player))
            return;


        if (healthFlask && player.playerNetWorkManager.remainingHealthFlasks.Value <= 0)
        {
            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerCombatManager.isUsingItem = true;

            if (player.IsOwner)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(emptyFlaskAnimation, false, false, true, true, false);
                player.playerNetWorkManager.HideWeaponsServerRpc();
            }

            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            return;

        }

        if (!healthFlask && player.playerNetWorkManager.remainingFocusPointFlasks.Value <= 0)
        {
            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerCombatManager.isUsingItem = true;

            if (player.IsOwner)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(emptyFlaskAnimation, false, false, true, true, false);
                player.playerNetWorkManager.HideWeaponsServerRpc();
            }

            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            return;
        }

        // CHUGGING
        if (player.playerCombatManager.isUsingItem)
        {
            if(player.IsOwner)
                player.playerNetWorkManager.isChugging.Value = true;

            return;
        }

        player.playerCombatManager.isUsingItem = true;

        player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);

        if (player.IsOwner)
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, false, false, true, true, false);
            player.playerNetWorkManager.HideWeaponsServerRpc();
        }
    }

    public override void SuccessfullyUseItem(PlayerManager player)
    {
        base.SuccessfullyUseItem(player);

        if (player.IsOwner)
        {
            if (healthFlask)
            {
                player.playerNetWorkManager.currentHealth.Value += flaskRestoration;
                player.playerNetWorkManager.remainingHealthFlasks.Value -= 1;

            }
            else
            {
                player.playerNetWorkManager.currentFocusPoints.Value += flaskRestoration;
                player.playerNetWorkManager.remainingFocusPointFlasks.Value -= 1;
            }
            PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);


            if (healthFlask && player.playerNetWorkManager.remainingHealthFlasks.Value <= 0)
            {
                Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            }

            if (!healthFlask && player.playerNetWorkManager.remainingFocusPointFlasks.Value <= 0)
            {
                Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            }

        }


        PlayHealingFX(player);
    }

    private void PlayHealingFX(PlayerManager player)
    {
        Instantiate(WorldCharacterEffectsManager.instance.healingFlaskVFX, player.transform);
        player.characterSFXManager.PlaySoundFX(WorldSFXManger.instance.healingFlaskSFX);
    }

    public override int GetCurrentAmount(PlayerManager player)
    {
        int currentAmount = 0;

        if (healthFlask)
            currentAmount = player.playerNetWorkManager.remainingHealthFlasks.Value;


        if (!healthFlask)
            currentAmount = player.playerNetWorkManager.remainingFocusPointFlasks.Value;

        return currentAmount;

    }

}
