using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase instance;

    public WeaponItem unarmedWeapon;

    public GameObject pickUpItemPrefab;

    [Header("Weapons")]
    [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

    [Header("Head Equipment")]
    [SerializeField] List<HeadEquipmentItem> headEquipment = new List<HeadEquipmentItem>();

    [Header("Body Equipment")]
    [SerializeField] List<BodyEquipmentItem> bodyEquipment = new List<BodyEquipmentItem>();

    [Header("Hand Equipment")]
    [SerializeField] List<HandEquipmentItem> handEquipment = new List<HandEquipmentItem>();

    [Header("Leg Equipment")]
    [SerializeField] List<LegEquipmentItem> legEquipment = new List<LegEquipmentItem>();

    [Header("Ashes Of War")]
    [SerializeField] List<AshOfWar> ashesOfWar = new List<AshOfWar>();

    [Header("Spells")]
    [SerializeField] List<SpellItem> spells = new List<SpellItem>();

    [Header("Projectile")]
    [SerializeField] List<RangedProjectileItem> projectiles = new List<RangedProjectileItem>();

    [Header("Quick Slot Item")]
    [SerializeField] List<QuickSlotItem> quickSlots = new List<QuickSlotItem>();

    [Header("Items")]
    private List<Item> items = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var weapon in weapons)
        {
            items.Add(weapon);
        }

        foreach (var item in headEquipment)
        {
            items.Add(item);
        }

        foreach (var item in handEquipment)
        {
            items.Add(item);
        }

        foreach (var item in legEquipment)
        {
            items.Add(item);
        }

        foreach (var item in bodyEquipment)
        {
            items.Add(item);
        }


        foreach (var item in ashesOfWar)
        {
            items.Add(item);
        }

        foreach (var item in spells)
        {
            items.Add(item);
        }


        foreach (var item in projectiles)
        {
            items.Add(item);
        }

        foreach (var item in quickSlots)
        {
            items.Add(item);
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }


    }

    //ITEM DATABASE

    public Item GetItemByID(int ID)
    {
        return items.FirstOrDefault(item => item.itemID == ID);
    }

    public  WeaponItem GetWeaponByID(int ID)
    {
        return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
    }

    public HeadEquipmentItem GetHeadEquipmentByID(int ID)
    {
        return headEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
    }

    public LegEquipmentItem GetLegEquipmentByID(int ID)
    {
        return legEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
    }

    public HandEquipmentItem GetHandEquipmentByID(int ID)
    {
        return handEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
    }

    public BodyEquipmentItem GetBodyEquipmentByID(int ID)
    {
        return bodyEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
    }

    public AshOfWar GetAshesOfWarByID(int ID)
    {
        return ashesOfWar.FirstOrDefault(item => item.itemID == ID);
    }

    public SpellItem GetSpellByID(int ID)
    {
        return spells.FirstOrDefault(item => item.itemID == ID);
    }

    public RangedProjectileItem GetProjectileByID(int ID)
    {
        return projectiles.FirstOrDefault(item => item.itemID == ID);
    }

    public QuickSlotItem GetQuickSlotItemByID(int ID)
    {
        return quickSlots.FirstOrDefault(item => item.itemID == ID);
    }

    //ITEM SERIALIZATION
    public WeaponItem GetWeaponFromSerializedData(SerializableWeapon serializableWeapon)
    {
        WeaponItem weapon= null;

        if(GetWeaponByID(serializableWeapon.itemID)) 
            weapon = Instantiate(GetWeaponByID(serializableWeapon.itemID));

        if(weapon == null)
            return Instantiate(unarmedWeapon);

        if (GetAshesOfWarByID(serializableWeapon.ashOfWarID))
        {
            AshOfWar ashOfWar = Instantiate(GetAshesOfWarByID(serializableWeapon.ashOfWarID));
            weapon.ashOfWarAction = ashOfWar;
        }

        return weapon;
    }

    public RangedProjectileItem GetRangedProjectileFromSerializedData(SerizlizableProjectile serizlizableProjectile)
    {
        RangedProjectileItem rangedProjectile = null;

        if (GetProjectileByID(serizlizableProjectile.itemID))
        {
            rangedProjectile = Instantiate(GetProjectileByID(serizlizableProjectile.itemID));
            rangedProjectile.currentAmmoAmount = serizlizableProjectile.itemAmount;
        }
         


        return rangedProjectile;
    }

    public FlaskItem GetFlaskFromSerializedData(SerializableFlask serizlizableFlask)
    {
        FlaskItem flask = null;

        if (GetQuickSlotItemByID(serizlizableFlask.itemID))
            flask = Instantiate(GetQuickSlotItemByID(serizlizableFlask.itemID)) as FlaskItem;


        return flask;
    }

    public QuickSlotItem GetQuickSlotItemFromSerializedData(SerializableQuickSlotItem serizlizableQuickSlot)
    {
        QuickSlotItem quickSLotItem = null;

        if (GetQuickSlotItemByID(serizlizableQuickSlot.itemID))
        {
            quickSLotItem = Instantiate(GetQuickSlotItemByID(serizlizableQuickSlot.itemID));

            quickSLotItem.itemAmount = serizlizableQuickSlot.itemAmount;
        }


        return quickSLotItem;
    }

}
