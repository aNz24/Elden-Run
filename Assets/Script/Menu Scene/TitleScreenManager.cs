using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;

    
    // MAIN MENU
    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;
    [SerializeField] GameObject titleScreenCharacterCreationMenu;


    [Header("Main Menu Button")]
    [SerializeField] Button loadNenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button deleteCharacterSlotButton;

    [Header("Main Menu Pop up")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharacterSlotsOkayButton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;

    // CHARACTER CREATION MENU
    [Header("Character Creation Main Panel Button")]
    [SerializeField] Button characterNameButton;
    [SerializeField] Button characterClassButton;
    [SerializeField] Button characterHairButton;
    [SerializeField] Button characterHairColorButton;
    [SerializeField] Button characterSexButton;
    [SerializeField] TextMeshProUGUI characterSexText;
    [SerializeField] Button startGameButton;

    [Header("Character Creation Class Panel Button")]
    [SerializeField] Button[] characterClassButtons;
    [SerializeField] Button[] characterHairButtons;
    [SerializeField] Button[] characterHairColorButtons;


    [Header(" Character Creation Secondary Panel Button")]
    [SerializeField] GameObject characterClassMenu;
    [SerializeField] GameObject characterHairMenu;
    [SerializeField] GameObject characterHairColorMenu;
    [SerializeField] GameObject characterNameMenu;
    [SerializeField] TMP_InputField characterNameInputField;

    [Header("Color Sliders")]
    [SerializeField] Slider redSilder;
    [SerializeField] Slider greenSilder;
    [SerializeField] Slider blueSilder;

    [Header("Hidden Gear")]
    private HeadEquipmentItem hiddenHelmet;

    [Header("Save slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

    [Header("Classes")]
    public CharacterClass [] startingClasses;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNetWorkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void AttempToCreateNewCharacter()
    {
        if (WorldSaveGameManager.instance.HasFreeCharacterSlot())
        {
            OpenCharacterCreationMenu();
        }
        else
        {
            DisplayNoFreeCharacterSlotsPopUp();
        }
    }

    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttemptToCreateNewGame();
    }

    public void OpenLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
        loadNenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        titleScreenLoadMenu.SetActive(false);
        titleScreenMainMenu.SetActive(true);
        mainMenuLoadGameButton.Select();

    }

    public void DisplayNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkayButton.Select();
    }

    public void ToggleBodyType()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetWorkManager.isMale.Value = !player.playerNetWorkManager.isMale.Value;

        if (player.playerNetWorkManager.isMale.Value)
        {
            characterSexText.text = "MALE";
        }
        else
        {
            characterSexText.text = "FEMALE";

        }
    }

    public void OpenTitleScreenMenu()
    {
        titleScreenMainMenu.SetActive(true);
    }

    public void CloseTitleScreenMenu()
    {
        titleScreenMainMenu.SetActive(false);
    }

    public void OpenCharacterCreationMenu()
    {
        titleScreenCharacterCreationMenu.SetActive(true);
        CloseTitleScreenMenu();

        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
        player.playerBodyManager.ToggleBodyType(true);
    }

    public void CloseCharacterCreationMenu()
    {
        titleScreenCharacterCreationMenu.SetActive(false);
        OpenTitleScreenMenu();
        mainMenuNewGameButton.Select();
    }

    public void OpenChooseCharacterClassSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(false);

        characterClassMenu.SetActive(true);

        if (characterClassButtons.Length > 0)
        {
            characterClassButtons[0].Select();
            characterClassButtons[0].OnSelect(null);
        }
    }

    public void CloseChooseCharacterClassSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(true);

        characterClassMenu.SetActive(false);

        characterClassButton.Select();
        characterClassButton.OnSelect(null);

    }

    public void OpenChooseHairStyleSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(false);

        characterHairMenu.SetActive(true);

        if (characterHairButtons.Length > 0)
        {
            characterHairButtons[0].Select();
            characterHairButtons[0].OnSelect(null);
        }

        if (player.playerInventoryManager.headEquipment != null)
            hiddenHelmet = Instantiate(player.playerInventoryManager.headEquipment);

        player.playerInventoryManager.headEquipment = null;
        player.playerEquipmentManager.EquipArmor();
    }

    public void CloseChooseHairStyleSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(true);

        characterHairMenu.SetActive(false);

        characterHairButton.Select();
        characterHairButton.OnSelect(null); 

        if(hiddenHelmet != null)
            player.playerInventoryManager.headEquipment = hiddenHelmet;

        player.playerEquipmentManager.EquipArmor();

    }

    public void OpenChooseHairColorSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(false);

        characterHairColorMenu.SetActive(true);

        if (characterHairColorButtons.Length > 0)
        {
            characterHairColorButtons[0].Select();
            characterHairColorButtons[0].OnSelect(null);
        }

        if (player.playerInventoryManager.headEquipment != null)
            hiddenHelmet = Instantiate(player.playerInventoryManager.headEquipment);

        player.playerInventoryManager.headEquipment = null;
        player.playerEquipmentManager.EquipArmor();
    }

    public void CloseChooseHairColorSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(true);

        characterHairColorMenu.SetActive(false);

        characterHairColorButton.Select();
        characterHairColorButton.OnSelect(null);

        if (hiddenHelmet != null)
            player.playerInventoryManager.headEquipment = hiddenHelmet;

        player.playerEquipmentManager.EquipArmor();

    }

    public void OpenChooseNameSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(false);

        characterNameButton.gameObject.SetActive(false);
        characterNameMenu.SetActive(true);

        characterNameInputField.Select();

   
    }

    public void CloseChooseNameSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(true);

        characterNameMenu.SetActive(false);
        characterNameButton.gameObject.SetActive(true);

        characterNameButton.Select();


        player.playerNetWorkManager.characterName.Value = characterNameInputField.text;

    }

    private void ToggleCharacterCreationScreenMainMenuButtons(bool status)
    {
        characterNameButton.enabled = status;
        characterClassButton.enabled = status;
        characterHairButton.enabled = status;
        characterHairColorButton.enabled = status;
        startGameButton.enabled = status;
        characterSexButton.enabled = status;

    }

    public void CloseNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();

    }

    // CHARACTER SLOTS
    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelecteNoSlot()
    {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteCharacterSlot()
    {
        if(currentSelectedSlot != CharacterSlot.NO_SLOT)
        {
            deleteCharacterSlotPopUp.SetActive(true);
            deleteCharacterSlotButton.Select();
        }
    }

    public void DeleteCharacterSlot()
    {

        deleteCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);

        titleScreenLoadMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
        loadNenuReturnButton.Select();
    }

    public void CloseDeleteCharacterPopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        loadNenuReturnButton.Select();
    }

    //CHARACTER CLASS
    public void SelectClass(int classID)
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        if (startingClasses.Length <= 0)
            return;

        startingClasses[classID].SetClass(player);
        CloseChooseCharacterClassSubMenu();
    }

    public void PreviewClass(int classID)
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        if (startingClasses.Length <= 0)
            return;

        startingClasses[classID].SetClass(player);
    }

    public void SetCharacterClass(PlayerManager player , int vitality , int endurence , int strength, int mind, int dexterity, int intellgence, int faith ,
        WeaponItem[] mainHandWeapons , WeaponItem[] offHandWeapons ,
        HeadEquipmentItem headEquipment , BodyEquipmentItem bodyEquipment, LegEquipmentItem legEquipment , HandEquipmentItem handEquipment,
        QuickSlotItem[] quickSlotItems , SpellItem spellItems , RangedProjectileItem rangedProjectile)
    {
        hiddenHelmet = null;

        // set the stats
        player.playerNetWorkManager.vigor.Value = vitality;
        player.playerNetWorkManager.endurance.Value = endurence;
        player.playerNetWorkManager.strength.Value = strength;
        player.playerNetWorkManager.mind.Value = mind;
        player.playerNetWorkManager.dexterity.Value = dexterity;
        player.playerNetWorkManager.intelligence.Value = intellgence;
        player.playerNetWorkManager.faith.Value = faith;

        // set weapons
        player.playerInventoryManager.weaponInRightHandSlots[0] = Instantiate(mainHandWeapons[0]);
        player.playerInventoryManager.weaponInRightHandSlots[1] = Instantiate(mainHandWeapons[1]);
        player.playerInventoryManager.weaponInRightHandSlots[2] = Instantiate(mainHandWeapons[2]);
        player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponInRightHandSlots[0];
        player.playerNetWorkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponInRightHandSlots[0].itemID;

        player.playerInventoryManager.weaponInLeftHandSlots[0] = Instantiate(offHandWeapons[0]);
        player.playerInventoryManager.weaponInLeftHandSlots[1] = Instantiate(offHandWeapons[1]);
        player.playerInventoryManager.weaponInLeftHandSlots[2] = Instantiate(offHandWeapons[2]);
        player.playerInventoryManager.currentLeftHandWeapon = player.playerInventoryManager.weaponInLeftHandSlots[0];
        player.playerNetWorkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponInLeftHandSlots[0].itemID;

        // set armor

        //Head
        if (headEquipment != null)
        {
            HeadEquipmentItem equipment = Instantiate(headEquipment);
            player.playerInventoryManager.headEquipment = equipment;
        }
        else
        {
            player.playerInventoryManager.headEquipment = null;
        }

        // Hand
        if (handEquipment != null)
        {
            HandEquipmentItem equipment = Instantiate(handEquipment);
            player.playerInventoryManager.handEquipment = equipment;
        }
        else
        {
            player.playerInventoryManager.handEquipment = null;
        }

        // Body
        if (bodyEquipment != null)
        {
            BodyEquipmentItem equipment = Instantiate(bodyEquipment);
            player.playerInventoryManager.bodyEquipment = equipment;
        }
        else
        {
            player.playerInventoryManager.bodyEquipment = null;
        }

        // Leg
        if (legEquipment != null)
        {
            LegEquipmentItem equipment = Instantiate(legEquipment);
            player.playerInventoryManager.legEquipment = equipment;
        }
        else
        {
            player.playerInventoryManager.legEquipment = null;
        }

        player.playerEquipmentManager.EquipArmor();

        // set quick slot items

        player.playerInventoryManager.quickSlotItemIndex = 0;

        if (quickSlotItems[0] != null)
            player.playerInventoryManager.quickSlotItemsInQuickSlot[0] = Instantiate(quickSlotItems[0]);

        if (quickSlotItems[1] != null)
            player.playerInventoryManager.quickSlotItemsInQuickSlot[1] = Instantiate(quickSlotItems[1]);

        if (quickSlotItems[2] != null)
            player.playerInventoryManager.quickSlotItemsInQuickSlot[2] = Instantiate(quickSlotItems[2]);

        player.playerEquipmentManager.LoadQuickSlotEquipment(player.playerInventoryManager.quickSlotItemsInQuickSlot[player.playerInventoryManager.quickSlotItemIndex]);

        // set spell
        if (spellItems != null)
            player.playerInventoryManager.currentSpell = Instantiate(spellItems);

        // Projectile 
        if (rangedProjectile != null)
            player.playerInventoryManager.mainProjectile = Instantiate(rangedProjectile);
    }


    // CHARACTER HAIR

    public void SelectHair(int hairID)
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetWorkManager.hairStyleID.Value = hairID;

        CloseChooseHairStyleSubMenu();
    }

    public void PreviewHair(int hairID)
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetWorkManager.hairStyleID.Value = hairID;

    }

    public void SelectHairColor()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetWorkManager.hairColorRed.Value = redSilder.value;
        player.playerNetWorkManager.hairColorGreen.Value = greenSilder.value;
        player.playerNetWorkManager.hairColorBlue.Value = blueSilder.value;

        CloseChooseHairColorSubMenu();
    }

    public void PreviewHairColor()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetWorkManager.hairColorRed.Value = redSilder.value;
        player.playerNetWorkManager.hairColorGreen.Value = greenSilder.value;
        player.playerNetWorkManager.hairColorBlue.Value = blueSilder.value;

    }

    public void SetRedColorSilder(float redValue)
    {
        redSilder.value = redValue;
    }

    public void SetGreenColorSilder(float greenValue)
    {
        greenSilder.value = greenValue;
    }

    public void SetBlueColorSilder(float blueValue)
    {
        blueSilder.value = blueValue;
    }

  

}
