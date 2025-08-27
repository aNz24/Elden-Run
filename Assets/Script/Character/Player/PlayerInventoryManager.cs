using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    [Header("Weapon")]
    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;
    public WeaponItem currentTwoHandWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] weaponInRightHandSlots = new WeaponItem[03];
    public int rightHandWeaponIndex = 0;
    public WeaponItem[] weaponInLeftHandSlots = new WeaponItem[03];
    public int leftHandWeaponIndex = 0;
    public SpellItem currentSpell;
    public QuickSlotItem[] quickSlotItemsInQuickSlot = new QuickSlotItem[03];
    public int quickSlotItemIndex = 0;
    public QuickSlotItem currentQuickSlotItem;

    [Header("Armor")]
    public HeadEquipmentItem headEquipment;
    public BodyEquipmentItem bodyEquipment;
    public HandEquipmentItem handEquipment;
    public LegEquipmentItem legEquipment;

    [Header("Projectiles")]
    public RangedProjectileItem mainProjectile;
    public RangedProjectileItem secondaryProjectile;

    [Header("Armor")]
    public List<Item> itemInInventory;

    public void AddItemToInventory(Item item)
    {
        itemInInventory.Add(item);
    }

    public void RemoveItemToInventory(Item item)
    {
        itemInInventory.Remove(item);

        for (int i =  itemInInventory.Count -1 ; i > -1; i--)
        {
            if (itemInInventory[i] == null)
            {
               itemInInventory.RemoveAt(i);
            }
        }
    }
}
