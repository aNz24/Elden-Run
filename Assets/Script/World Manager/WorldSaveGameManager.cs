using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;
    public PlayerManager player;

    [Header("SAVE/LOAD")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] int worldSceneIndex = 1;

    [Header("Save Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;


    [Header("Current Chracter Data")]
    public CharacterSlot currentCharacterSlotBegingUsed;
    public CharacterSaveData currentCharacterData;
    private string saveFileName;


    [Header("Chracter Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;
    public CharacterSaveData characterSlot10;


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
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadAllCharacterProfiles();

    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public bool HasFreeCharacterSlot()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;


        // CHECK TO SEE IF WE CAN CREATE A NEW SAVE FILE (CHECK FOR OTHER EXISTTING FILE FIRST)


        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_01);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;


        // CHECK TO SEE IF WE CAN CREATE A NEW SAVE FILE (CHECK FOR OTHER EXISTTING FILE FIRST)
        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_02);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;


        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_03);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;


        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_04);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;


        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_05);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;


        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_06);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_07);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_08);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_09);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_10);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;


        return false;
    }

    public string DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "characterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "CharacterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "CharacterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "CharacterSlot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "CharacterSlot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "CharacterSlot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                fileName = "CharacterSlot_10";
                break;
            default:
                break;
        }
        return fileName;
    }

    public void AttemptToCreateNewGame()
    {

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;


        // CHECK TO SEE IF WE CAN CREATE A NEW SAVE FILE (CHECK FOR OTHER EXISTTING FILE FIRST)


        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_01);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        // CHECK TO SEE IF WE CAN CREATE A NEW SAVE FILE (CHECK FOR OTHER EXISTTING FILE FIRST)
        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_02);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_03);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_03;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_04);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_04;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_05);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_05;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_06);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_06;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }
        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_07);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_07;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }
        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_08);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_08;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }
        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_09);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_09;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }
        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_10);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // IF THIS PROFILE SLOT IS NOT TAKEN, MAKE A NEW ONE USING THIS SLOT
            currentCharacterSlotBegingUsed = CharacterSlot.CharacterSlot_10;
            currentCharacterData = new CharacterSaveData();
            NewGame();
            return;
        }
        // IF THERE ARE NO FREE SLOTS, NOTIFY THE PLAYER
        TitleScreenManager.instance.DisplayNoFreeCharacterSlotsPopUp();

    }

    private void NewGame()
    {
        //
        player.playerNetWorkManager.vigor.Value = 15;
        player.playerNetWorkManager.endurance.Value = 10;
        player.playerNetWorkManager.mind.Value = 10;
        SaveGame();
        WorldScreenManager.instance.LoadWorldScene(worldSceneIndex);
    }

    public void LoadGame()
    {
        // LOAD A PREVIOUS FILE, WITH A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
        saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(currentCharacterSlotBegingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        //GENERALLY WORKS ON MUTIPLE MACHINE TYPES
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        WorldScreenManager.instance.LoadWorldScene(worldSceneIndex);

    }

    public void SaveGame()
    {
        // SAVE THE CURRENT FILE UNDER A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
        saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(currentCharacterSlotBegingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        //GENERALLY WORKS ON MUTIPLE MACHINE TYPES
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        //PASS THE PLAYER INTO,FROM GAME, TO THEIR SAVE FILE
        player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

        //WRITE THAT INFO ONTO A JSON FILE, SAVED TO THIS MACHINE
        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }

    public void DeleteGame(CharacterSlot characterSlot)
    {
        // CHOOSE FILE BASED ON NAME

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(characterSlot);

        saveFileDataWriter.DeleteSaveFile();
    }

    // LOAD ALL CHARACTER PROFILES ON DEVICE WHEN  STARTING GAME
    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_01);   
        characterSlot01 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_05);
        characterSlot05 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_06);
        characterSlot06 = saveFileDataWriter.LoadSaveFile();


        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_07);
        characterSlot07 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_08);
        characterSlot08 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_09);
        characterSlot09 = saveFileDataWriter.LoadSaveFile();


        saveFileDataWriter.saveFileName = DecideChracterFileNameBaseOnCharacterBeingUsed(CharacterSlot.CharacterSlot_10);
        characterSlot10 = saveFileDataWriter.LoadSaveFile();

    }


    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }

    public SerializableWeapon GetSerializableWeaponFromWeaponItem(WeaponItem weapon)
    {
        SerializableWeapon serializableWeapon =  new SerializableWeapon();

        serializableWeapon.itemID = weapon.itemID;

        if (weapon.ashOfWarAction != null)
        {
            serializableWeapon.ashOfWarID = weapon.ashOfWarAction.itemID;
        }
        else
        {
            serializableWeapon.ashOfWarID = -1;
        }

        return serializableWeapon;
    }

    public SerizlizableProjectile GetSerializableRangedProjectileFromWeaponItem(RangedProjectileItem projectile)
    {
        SerizlizableProjectile serializableRangedProjectile = new SerizlizableProjectile();

        if (projectile != null)
        {
            serializableRangedProjectile.itemID = projectile.itemID;
            serializableRangedProjectile.itemAmount = projectile.currentAmmoAmount;
        }
        else
        {
            serializableRangedProjectile.itemID = -1;

        }


        return serializableRangedProjectile;
    }

    public SerializableFlask GetSerializableFlaskFromFlaskItem(FlaskItem flask)
    {
        SerializableFlask serializableFlask = new SerializableFlask();

        if (flask != null)
        {
            serializableFlask.itemID = flask.itemID;
        }
        else
        {
            serializableFlask.itemID = -1;

        }

        return serializableFlask;
    }

    public SerializableQuickSlotItem GetSerializableQuickSloteFromQuickSlotItem(QuickSlotItem quickSlot)
    {
        SerializableQuickSlotItem serializableQuickSlot = new SerializableQuickSlotItem();

        if (quickSlot != null)
        {
            serializableQuickSlot.itemID = quickSlot.itemID;
            serializableQuickSlot.itemAmount = quickSlot.itemAmount;
        }
        else
        {
            serializableQuickSlot.itemID = -1;

        }


        return serializableQuickSlot;
    }
}
