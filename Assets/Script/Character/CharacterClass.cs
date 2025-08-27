using UnityEngine;

[System.Serializable]
public class CharacterClass
{
    [Header("Class Information")]
    public string className;

    [Header("Class Stats")]
    public int vitality = 10;
    public int endurence = 10;
    public int mind = 10;
    public int strength = 10;
    public int dexterity = 10;
    public int intellgence = 10;
    public int faith = 10;

    [Header("Class Weapon")]
    public WeaponItem[] mainHandWeapons = new WeaponItem[3];
    public WeaponItem[] offHandWeapons = new WeaponItem[3];

    [Header("Class Armor")]
    public HeadEquipmentItem headEquipment;
    public BodyEquipmentItem bodyEquipment;
    public LegEquipmentItem legEquipment;
    public HandEquipmentItem handEquipment;

    [Header("Quick Slot Items")]
    public QuickSlotItem[] quickSlotItems = new QuickSlotItem[3];

    [Header("Spell")]
    public SpellItem spellItems;

    [Header("Spell")]
    public RangedProjectileItem rangedProjectile;

    public void SetClass(PlayerManager player)
    {
        TitleScreenManager.instance.SetCharacterClass(player , vitality , endurence, mind, 
            strength, dexterity, intellgence,faith, mainHandWeapons, offHandWeapons,headEquipment , bodyEquipment,legEquipment,handEquipment,quickSlotItems , spellItems , rangedProjectile);
    }
}
