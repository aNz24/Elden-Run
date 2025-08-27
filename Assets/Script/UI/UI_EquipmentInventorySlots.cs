using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UI_EquipmentInventorySlots : MonoBehaviour
{
    public Image itemIcon;
    public Image hightlightedIcon;
    [SerializeField] public Item currentItem;

    public void AddItem(Item item)
    {
        if (item == null)
        {
            itemIcon.enabled = false;
            return;
        }

        itemIcon.enabled = true;    

        currentItem = item; 
        itemIcon.sprite = item.itemIcon;
    }

    public void SelectSlot()
    {
        hightlightedIcon.enabled = true;
    }

    public void DeselectSlot()
    {
        hightlightedIcon.enabled = false;
    }

    public void EquipItem()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
       Item equippedItem;

        switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:

                equippedItem = player.playerInventoryManager.weaponInRightHandSlots[0];

                if(equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInRightHandSlots[0] = currentItem as WeaponItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                if(player.playerInventoryManager.rightHandWeaponIndex == 0)
                    player.playerNetWorkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();

                break;
            case EquipmentType.RightWeapon02:

                equippedItem = player.playerInventoryManager.weaponInRightHandSlots[1];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInRightHandSlots[1] = currentItem as WeaponItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                    player.playerNetWorkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();

                break;
            case EquipmentType.RightWeapon03:
                equippedItem = player.playerInventoryManager.weaponInRightHandSlots[2];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInRightHandSlots[2] = currentItem as WeaponItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                    player.playerNetWorkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();

                break;
            case EquipmentType.LeftWeapon01:
                equippedItem = player.playerInventoryManager.weaponInLeftHandSlots[0];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInLeftHandSlots[0] = currentItem as WeaponItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                    player.playerNetWorkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;
            case EquipmentType.LeftWeapon02:
                equippedItem = player.playerInventoryManager.weaponInLeftHandSlots[1];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInLeftHandSlots[1] = currentItem as WeaponItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                    player.playerNetWorkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;
            case EquipmentType.LeftWeapon03:
                equippedItem = player.playerInventoryManager.weaponInLeftHandSlots[2];

                if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.weaponInLeftHandSlots[2] = currentItem as WeaponItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                    player.playerNetWorkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;

            case EquipmentType.Head:
                equippedItem = player.playerInventoryManager.headEquipment;

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.headEquipment = currentItem as HeadEquipmentItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;

            case EquipmentType.Body:
                equippedItem = player.playerInventoryManager.bodyEquipment;

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.bodyEquipment = currentItem as BodyEquipmentItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;

            case EquipmentType.Hands:
                equippedItem = player.playerInventoryManager.handEquipment;

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.handEquipment = currentItem as HandEquipmentItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;

            case EquipmentType.Legs:
                equippedItem = player.playerInventoryManager.legEquipment;

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.legEquipment = currentItem as LegEquipmentItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;
            case EquipmentType.MainProjectile:
                equippedItem = player.playerInventoryManager.mainProjectile;

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.mainProjectile = currentItem as RangedProjectileItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                player.playerEquipmentManager.LoadMainProjectileEquipment(player.playerInventoryManager.mainProjectile);

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;

            case EquipmentType.SecondaryProjectile:
                equippedItem = player.playerInventoryManager.secondaryProjectile;

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.secondaryProjectile = currentItem as RangedProjectileItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                player.playerEquipmentManager.LoadSecondaryProjectileEquipment(player.playerInventoryManager.secondaryProjectile);

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;

            case EquipmentType.QuickSlot01:
                equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlot[0];

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.quickSlotItemsInQuickSlot[0] = currentItem as QuickSlotItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                if (player.playerInventoryManager.quickSlotItemIndex == 0)
                    player.playerNetWorkManager.currentQuickSlotItemID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;


            case EquipmentType.QuickSlot02:
                equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlot[1];

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.quickSlotItemsInQuickSlot[1] = currentItem as QuickSlotItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                if (player.playerInventoryManager.quickSlotItemIndex == 1)
                    player.playerNetWorkManager.currentQuickSlotItemID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;

            case EquipmentType.QuickSlot03:
                equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlot[2];

                if (equippedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equippedItem);
                }

                player.playerInventoryManager.quickSlotItemsInQuickSlot[2] = currentItem as QuickSlotItem;

                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                if (player.playerInventoryManager.quickSlotItemIndex == 2)
                    player.playerNetWorkManager.currentQuickSlotItemID.Value = currentItem.itemID;

                PlayerUIManager.instance.playerUIEquipmentManager.RefeshMenu();
                break;
            default:
                break;
        }

        PlayerUIManager.instance.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();
    }
}
