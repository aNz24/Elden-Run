using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Scene Index")]
    public int sceneIndex = 1;

    [Header("Character Name")]
    public string characterName = "Character";

    [Header("Dead Spot")]
    public bool hasDeadSpot = false;
    public float deadSpotPositionX;
    public float deadSpotPositionY;
    public float deadSpotPositionZ;
    public int deadPostRunesCount;

    [Header("Body Type")]
    public bool isMale = true;
    public int hairStyleID;
    public float hairColorRed;
    public float hairColorGreen;
    public float hairColorBlue;

    [Header("Time Played")]
    public float secondPlayed;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Stats")]
    public int currentHealth;
    public float currentStamina;
    public int currentFocusPoints;
    public int runes;

    [Header("Stats")]
    public int vigor;
    public int endurance;
    public int mind;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int faith;

    [Header("Site Of Grace")]
    public int lastSiteOfGraceRestedAt = 0;
    public SerializableDictionary<int, bool> siteOfGrace;

    [Header("Bosses")]
    public SerializableDictionary<int ,bool> bossesAwakened;
    public SerializableDictionary<int ,bool> bossesDefeated;

    [Header("World Item")]
    public SerializableDictionary<int, bool> worldItemLooted;

    [Header("Equipment")]
    public int headEquipment;
    public int bodyEquipment;
    public int legEquipment;
    public int handEquipment;

    public int rightWeaponIndex;
    public SerializableWeapon rightWeapon01;
    public SerializableWeapon rightWeapon02;
    public SerializableWeapon rightWeapon03;

    public int lefttWeaponIndex;
    public SerializableWeapon lefttWeapon01;
    public SerializableWeapon lefttWeapon02;
    public SerializableWeapon lefttWeapon03;

    public int quickSlotIndex;
    public SerializableQuickSlotItem quickSlotItem01;
    public SerializableQuickSlotItem quickSlotItem02;
    public SerializableQuickSlotItem quickSlotItem03;

    public SerizlizableProjectile mainProjectile;
    public SerizlizableProjectile secondaryProjectile;

    public int currentHealthFlaskRemaining = 3;
    public int currentFocusPointsFlaskRemaining = 1;

    [Header("Inventory")]
    public List<SerializableWeapon> weaponsInInventory;
    public List<SerizlizableProjectile> projectilesInInventory;
    public List<SerializableQuickSlotItem> quickSlotItemsInInventory;
    public List<int> headEquipmentInInventory;
    public List<int> bodyEquipmentInInventory;
    public List<int> legEquipmentInInventory;
    public List<int> handEquipmentInInventory;

    public int currentSpell;

    public CharacterSaveData()
    {
        siteOfGrace = new SerializableDictionary<int ,bool>();
        bossesAwakened = new SerializableDictionary<int ,bool>();
        bossesDefeated = new SerializableDictionary<int ,bool>();   
        worldItemLooted = new SerializableDictionary<int ,bool>();

        weaponsInInventory = new List<SerializableWeapon>();
        projectilesInInventory = new List<SerizlizableProjectile>();
        quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();

        headEquipmentInInventory =  new List<int>();
        bodyEquipmentInInventory =  new List<int>();
        legEquipmentInInventory =  new List<int>();
        handEquipmentInInventory =  new List<int>();
    }
}
