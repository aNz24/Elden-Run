using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class PlayerUIEquipmentManager : PlayerUIMenu
{
   

    [Header("Weapon Slots")]
    [SerializeField] Image rightHandSlot01;
    private Button rightHandSlot01Button;
    [SerializeField] Image rightHandSlot02;
    private Button rightHandSlot02Button;
    [SerializeField] Image rightHandSlot03;
    private Button rightHandSlot03Button;
    [SerializeField] Image leftHandSlot01;
    private Button leftHandSlot01Button;
    [SerializeField] Image leftHandSlot02;
    private Button leftHandSlot02Button;
    [SerializeField] Image leftHandSlot03;
    private Button leftHandSlot03Button;
    [SerializeField] Image headEquipmentSlot;
    private Button headEquipmentSlotButton;
    [SerializeField] Image bodyEquipmentSlot;
    private Button bodyEquipmentSlotButton;
    [SerializeField] Image handEquipmentSlot;
    private Button  handEquipmentSlotButton;
    [SerializeField] Image legEquipmentSlot;
    private Button legEquipmentSlotButton;

    [SerializeField] Image mainProjectileEquipmentSlot;
    private Button mainProjectileEquipmentSlotButton;
    [SerializeField] TextMeshProUGUI mainProjectileCount;
    [SerializeField] Image secondaryProjectileEquipmentSlot;
    private Button secondaryProjectileEquipmentSlotButton;
    [SerializeField] TextMeshProUGUI secondaryProjectileCount;

    [Header("Quick Slot")]
    [SerializeField] Image quickSlot01EquipmentSlot;
    private Button quickSlot01Button;
    [SerializeField] TextMeshProUGUI quickSlot01Count;
    [SerializeField] Image quickSlot02EquipmentSlot;
    private Button quickSlot02Button;
    [SerializeField] TextMeshProUGUI quickSlot02Count;
    [SerializeField] Image quickSlot03EquipmentSlot;
    private Button quickSlot03Button;
    [SerializeField] TextMeshProUGUI quickSlot03Count;

    [Header("Equipment Inventory")]
    public EquipmentType currentSelectedEquipmentSlot;
    [SerializeField] GameObject equipmentInventoryWindow;
    [SerializeField] GameObject equipmentInventorySlotPrefab;
    [SerializeField] Transform equipmentInventoryContentWindow;
    [SerializeField] Item currentSelectedItem;

    private void Awake()
    {
        rightHandSlot01Button  = rightHandSlot01.GetComponentInParent<Button>(true);
        rightHandSlot02Button  = rightHandSlot02.GetComponentInParent<Button>(true);
        rightHandSlot03Button  = rightHandSlot03.GetComponentInParent<Button>(true);

        leftHandSlot01Button = leftHandSlot01.GetComponentInParent<Button>(true);
        leftHandSlot02Button = leftHandSlot02.GetComponentInParent<Button>(true);
        leftHandSlot03Button = leftHandSlot03.GetComponentInParent<Button>(true);

        headEquipmentSlotButton = headEquipmentSlot.GetComponentInParent<Button>(true);
        bodyEquipmentSlotButton = bodyEquipmentSlot.GetComponentInParent<Button>(true); 
        handEquipmentSlotButton = handEquipmentSlot.GetComponentInParent<Button>(true);
        legEquipmentSlotButton  = legEquipmentSlot.GetComponentInParent<Button>(true);

        mainProjectileEquipmentSlotButton = mainProjectileEquipmentSlot.GetComponentInParent<Button>(true);
        secondaryProjectileEquipmentSlotButton = secondaryProjectileEquipmentSlot.GetComponentInParent<Button>(true);
        quickSlot01Button = quickSlot01EquipmentSlot.GetComponentInParent<Button>(true);
        quickSlot02Button = quickSlot02EquipmentSlot.GetComponentInParent<Button>(true);
        quickSlot03Button = quickSlot03EquipmentSlot.GetComponentInParent<Button>(true);

    }

    public override void OpenMenu()
    {
        base.OpenMenu();

        ToggleEquipmentButtons(true);
        equipmentInventoryWindow.SetActive(false);
        ClearEquipmentInventory();
        RefeshEquipmentSlotIcons();
    }

    public void RefeshMenu()
    {
        ClearEquipmentInventory();
        RefeshEquipmentSlotIcons();
    }

    private void ToggleEquipmentButtons (bool isEnable)
    {
        rightHandSlot01Button.enabled = isEnable;
        rightHandSlot02Button.enabled = isEnable;
        rightHandSlot03Button.enabled = isEnable;

        leftHandSlot01Button.enabled = isEnable;
        leftHandSlot02Button.enabled = isEnable;
        leftHandSlot03Button.enabled = isEnable;

        headEquipmentSlotButton.enabled = isEnable;
        bodyEquipmentSlotButton.enabled = isEnable;
        handEquipmentSlotButton.enabled = isEnable;
        legEquipmentSlotButton.enabled = isEnable;
        mainProjectileEquipmentSlotButton.enabled = isEnable;
        secondaryProjectileEquipmentSlotButton.enabled = isEnable;
        quickSlot01Button.enabled = isEnable;
        quickSlot02Button.enabled = isEnable;
        quickSlot03Button.enabled = isEnable;
    }

    public void SelectLastSelectedEquipmentSlot()
    {
        Button lastSelectedButton =null;
        ToggleEquipmentButtons(true);
        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                lastSelectedButton = rightHandSlot01Button;
                break;
            case EquipmentType.RightWeapon02:
                lastSelectedButton = rightHandSlot02Button;
                break;
            case EquipmentType.RightWeapon03:
                lastSelectedButton = rightHandSlot03Button;
                break;
            case EquipmentType.LeftWeapon01:
                lastSelectedButton = leftHandSlot01Button;
                break;
            case EquipmentType.LeftWeapon02:
                lastSelectedButton = leftHandSlot02Button;
                break;
            case EquipmentType.LeftWeapon03:
                lastSelectedButton = leftHandSlot03Button;
                break;
            case EquipmentType.Head:
                lastSelectedButton = headEquipmentSlotButton;
                break;
            case EquipmentType.Body:
                lastSelectedButton = bodyEquipmentSlotButton;
                break;
            case EquipmentType.Hands:
                lastSelectedButton = handEquipmentSlotButton;
                break;
            case EquipmentType.Legs:
                lastSelectedButton = legEquipmentSlotButton;
                break;
            case EquipmentType.MainProjectile:
                lastSelectedButton = mainProjectileEquipmentSlotButton;
                break;
            case EquipmentType.SecondaryProjectile:
                lastSelectedButton = secondaryProjectileEquipmentSlotButton;
                break;
            case EquipmentType.QuickSlot01:
                lastSelectedButton = quickSlot01Button;
                break;
            case EquipmentType.QuickSlot02:
                lastSelectedButton = quickSlot02Button;
                break;
            case EquipmentType.QuickSlot03:
                lastSelectedButton = quickSlot03Button;
                break;
            default:
                break;
        }

        if (lastSelectedButton != null)
        {
            lastSelectedButton.Select();
            lastSelectedButton.OnSelect(null);

        }

        equipmentInventoryWindow.SetActive(false);

    }

    private void RefeshEquipmentSlotIcons()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        //RIGHT 01
        WeaponItem rightHandWeapon01 =player.playerInventoryManager.weaponInRightHandSlots[0];

        if (rightHandWeapon01.itemIcon != null)
        {
            rightHandSlot01.enabled = true;
            rightHandSlot01.sprite = rightHandWeapon01.itemIcon;
        }
        else
        {
            rightHandSlot01.enabled = false;
        }

        //RIGHT 02
        WeaponItem rightHandWeapon02 = player.playerInventoryManager.weaponInRightHandSlots[1];

        if (rightHandWeapon02.itemIcon != null)
        {
            rightHandSlot02.enabled = true;
            rightHandSlot02.sprite = rightHandWeapon02.itemIcon;
        }
        else
        {
            rightHandSlot02.enabled = false;
        }


        //RIGHT 03
        WeaponItem rightHandWeapon03 = player.playerInventoryManager.weaponInRightHandSlots[2];

        if (rightHandWeapon03.itemIcon != null)
        {
            rightHandSlot03.enabled = true;
            rightHandSlot03.sprite = rightHandWeapon03.itemIcon;
        }
        else
        {
            rightHandSlot03.enabled = false;
        }



        //LEFT 01
        WeaponItem leftHandWeapon01 = player.playerInventoryManager.weaponInLeftHandSlots[0];

        if (leftHandWeapon01.itemIcon != null)
        {
            leftHandSlot01.enabled = true;
            leftHandSlot01.sprite = leftHandWeapon01.itemIcon;
        }
        else
        {
            leftHandSlot01.enabled = false;
        }

        //LEFT 02
        WeaponItem leftHandWeapon02 = player.playerInventoryManager.weaponInLeftHandSlots[1];

        if (leftHandWeapon02.itemIcon != null)
        {
            leftHandSlot02.enabled = true;
            leftHandSlot02.sprite = leftHandWeapon02.itemIcon;
        }
        else
        {
            leftHandSlot02.enabled = false;
        }


        //LEFT 03
        WeaponItem leftHandWeapon03 = player.playerInventoryManager.weaponInLeftHandSlots[2];

        if (leftHandWeapon03.itemIcon != null)
        {
            leftHandSlot03.enabled = true;
            leftHandSlot03.sprite = leftHandWeapon03.itemIcon;
        }
        else
        {
            leftHandSlot03.enabled = false;
        }

        // HEAD 
        HeadEquipmentItem headEquipment =player.playerInventoryManager.headEquipment;

        if (headEquipment != null)
        {
            headEquipmentSlot.enabled = true;
            headEquipmentSlot.sprite = headEquipment.itemIcon;
        }
        else
        {
            headEquipmentSlot.enabled = false;  
        }

        // BODY
        BodyEquipmentItem bodyEquipment = player.playerInventoryManager.bodyEquipment;

        if (bodyEquipment != null)
        {
            bodyEquipmentSlot.enabled = true;
            bodyEquipmentSlot.sprite = bodyEquipment.itemIcon;
        }
        else
        {
            bodyEquipmentSlot.enabled = false;
        }

        // HANDS
        HandEquipmentItem handEquipment = player.playerInventoryManager.handEquipment;

        if (handEquipment != null)
        {
            handEquipmentSlot.enabled = true;
            handEquipmentSlot.sprite = handEquipment.itemIcon;
        }
        else
        {
            handEquipmentSlot.enabled = false;
        }


        // LEGS
        LegEquipmentItem legEquipment = player.playerInventoryManager.legEquipment;

        if (legEquipment != null)
        {
            legEquipmentSlot.enabled = true;
            legEquipmentSlot.sprite = legEquipment.itemIcon;
        }
        else
        {
            legEquipmentSlot.enabled = false;
        }

        // MAIN PROJECTILE
        RangedProjectileItem mainProjectileEquipment = player.playerInventoryManager.mainProjectile;

        if (mainProjectileEquipment != null)
        {
            mainProjectileEquipmentSlot.enabled = true;
            mainProjectileEquipmentSlot.sprite = mainProjectileEquipment.itemIcon;
            mainProjectileCount.enabled = true;
            mainProjectileCount.text = mainProjectileEquipment.currentAmmoAmount.ToString();
        }
        else
        {
            mainProjectileEquipmentSlot.enabled = false;
            mainProjectileCount.enabled = false;

        }

        // SECONDARY PROJECTILE
        RangedProjectileItem secondaryProjectileEquipment = player.playerInventoryManager.secondaryProjectile;

        if (secondaryProjectileEquipment != null)
        {
            secondaryProjectileEquipmentSlot.enabled = true;
            secondaryProjectileEquipmentSlot.sprite = secondaryProjectileEquipment.itemIcon;
            secondaryProjectileCount.enabled = true;
            secondaryProjectileCount.text = secondaryProjectileEquipment.currentAmmoAmount.ToString();
        }
        else
        {
            secondaryProjectileEquipmentSlot.enabled = false;
            secondaryProjectileCount.enabled = false;

        }

        //QUICK SLOTS

        QuickSlotItem quickSlot01Equipment = player.playerInventoryManager.quickSlotItemsInQuickSlot[0];

        if (quickSlot01Equipment != null)
        {
            quickSlot01EquipmentSlot.enabled = true;
            quickSlot01EquipmentSlot.sprite = quickSlot01Equipment.itemIcon;

            if (quickSlot01Equipment.isConsumable)
            {
                quickSlot01Count.enabled = true;
                quickSlot01Count.text = quickSlot01Equipment.GetCurrentAmount(player).ToString();
            }
            else
            {
                quickSlot01Count.enabled = false;
            }
        }
        else
        {
            quickSlot01EquipmentSlot.enabled = false;
            quickSlot01Count.enabled = false;
        }


        // 02
        QuickSlotItem quickSlot02Equipment = player.playerInventoryManager.quickSlotItemsInQuickSlot[1];

        if (quickSlot02Equipment != null)
        {
            quickSlot02EquipmentSlot.enabled = true;
            quickSlot02EquipmentSlot.sprite = quickSlot02Equipment.itemIcon;

            if (quickSlot02Equipment.isConsumable)
            {
                quickSlot02Count.enabled = true;
                quickSlot02Count.text = quickSlot02Equipment.GetCurrentAmount(player).ToString();
            }
            else
            {
                quickSlot02Count.enabled = false;
            }
        }
        else
        {
            quickSlot02EquipmentSlot.enabled = false;
            quickSlot02Count.enabled = false;
        }

        // 03
        QuickSlotItem quickSlot03Equipment = player.playerInventoryManager.quickSlotItemsInQuickSlot[2];

        if (quickSlot03Equipment != null)
        {
            quickSlot03EquipmentSlot.enabled = true;
            quickSlot03EquipmentSlot.sprite = quickSlot03Equipment.itemIcon;

            if (quickSlot03Equipment.isConsumable)
            {
                quickSlot03Count.enabled = true;
                quickSlot03Count.text = quickSlot03Equipment.GetCurrentAmount(player).ToString();
            }
            else
            {
                quickSlot03Count.enabled = false;
            }
        }
        else
        {
            quickSlot03EquipmentSlot.enabled = false;
            quickSlot03Count.enabled = false;
        }

    }

    private void ClearEquipmentInventory()
    {
        foreach (Transform item in equipmentInventoryContentWindow)
        {
            Destroy(item.gameObject);
        }
    }

    public void LoadEquipmentInventory()
    {
        ToggleEquipmentButtons(false);
        equipmentInventoryWindow.SetActive(true);

        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                LoadWeaponInventory();
                break;
            case EquipmentType.RightWeapon02:
                LoadWeaponInventory();
                break;
            case EquipmentType.RightWeapon03:
                LoadWeaponInventory();
                break;
            case EquipmentType.LeftWeapon01:
                LoadWeaponInventory();
                break;
            case EquipmentType.LeftWeapon02:
                LoadWeaponInventory();
                break;
            case EquipmentType.LeftWeapon03:
                LoadWeaponInventory();
                break;
            case EquipmentType.Head:
                LoadHeadEquipmentInventory();
                break;
            case EquipmentType.Body:
                LoadBodyEquipmentInventory();
                break;
            case EquipmentType.Hands:
                LoadHandEquipmentInventory();
                break;
            case EquipmentType.Legs:
                LoadLegEquipmentInventory();
                break;
            case EquipmentType.MainProjectile:
                LoadProjectileInventory();
                break;
            case EquipmentType.SecondaryProjectile:
                LoadProjectileInventory();
                break;
            case EquipmentType.QuickSlot01:
                LoadQuickSlotInInventory();
                break;
            case EquipmentType.QuickSlot02:
                LoadQuickSlotInInventory();
                break;
            case EquipmentType.QuickSlot03:
                LoadQuickSlotInInventory();
                break;
            default:
                break;

        }
    }

    private void LoadWeaponInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<WeaponItem> weaponInInventory = new List<WeaponItem>();

        for (int i = 0; i < player.playerInventoryManager.itemInInventory.Count; i++)
        {
            WeaponItem weapon =  player.playerInventoryManager.itemInInventory[i] as WeaponItem;
            
            if(weapon != null ) 
                weaponInInventory.Add(weapon);
        }

        if (weaponInInventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefeshMenu();

            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < weaponInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlots equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlots>();
            equipmentInventorySlot.AddItem(weaponInInventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot= true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }
    }

    public void LoadHeadEquipmentInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<HeadEquipmentItem> headEquipmentInIventory = new List<HeadEquipmentItem>();

        for (int i = 0; i < player.playerInventoryManager.itemInInventory.Count; i++)
        {
            HeadEquipmentItem equipment = player.playerInventoryManager.itemInInventory[i] as HeadEquipmentItem;

            if (equipment != null)
                headEquipmentInIventory.Add(equipment);
        }

        if (headEquipmentInIventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefeshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < headEquipmentInIventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlots equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlots>();
            equipmentInventorySlot.AddItem(headEquipmentInIventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }
    }

    public void LoadBodyEquipmentInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<BodyEquipmentItem> bodyEquipmentInIventory = new List<BodyEquipmentItem>();

        for (int i = 0; i < player.playerInventoryManager.itemInInventory.Count; i++)
        {
            BodyEquipmentItem equipment = player.playerInventoryManager.itemInInventory[i] as BodyEquipmentItem;

            if (equipment != null)
                bodyEquipmentInIventory.Add(equipment);
        }

        if (bodyEquipmentInIventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefeshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < bodyEquipmentInIventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlots equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlots>();
            equipmentInventorySlot.AddItem(bodyEquipmentInIventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }
    }

    public void LoadHandEquipmentInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<HandEquipmentItem> handEquipmentInIventory = new List<HandEquipmentItem>();

        for (int i = 0; i < player.playerInventoryManager.itemInInventory.Count; i++)
        {
            HandEquipmentItem equipment = player.playerInventoryManager.itemInInventory[i] as HandEquipmentItem;

            if (equipment != null)
                handEquipmentInIventory.Add(equipment);
        }

        if (handEquipmentInIventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefeshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < handEquipmentInIventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlots equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlots>();
            equipmentInventorySlot.AddItem(handEquipmentInIventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }
    }

    public void LoadLegEquipmentInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<LegEquipmentItem> legEquipmentInIventory = new List<LegEquipmentItem>();

        for (int i = 0; i < player.playerInventoryManager.itemInInventory.Count; i++)
        {
            LegEquipmentItem equipment = player.playerInventoryManager.itemInInventory[i] as LegEquipmentItem;

            if (equipment != null)
                legEquipmentInIventory.Add(equipment);
        }

        if (legEquipmentInIventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefeshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < legEquipmentInIventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlots equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlots>();
            equipmentInventorySlot.AddItem(legEquipmentInIventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }
    }

    private void LoadProjectileInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<RangedProjectileItem> projectileInInventory = new List<RangedProjectileItem>();

        for (int i = 0; i < player.playerInventoryManager.itemInInventory.Count; i++)
        {
            RangedProjectileItem projectile = player.playerInventoryManager.itemInInventory[i] as RangedProjectileItem;

            if (projectile != null)
                projectileInInventory.Add(projectile);
        }

        if (projectileInInventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefeshMenu();

            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < projectileInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlots equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlots>();
            equipmentInventorySlot.AddItem(projectileInInventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }
    }

    private void LoadQuickSlotInInventory()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        List<QuickSlotItem> quickSlotItemInInventory = new List<QuickSlotItem>();

        for (int i = 0; i < player.playerInventoryManager.itemInInventory.Count; i++)
        {
            QuickSlotItem quickSlotItem = player.playerInventoryManager.itemInInventory[i] as QuickSlotItem;

            if (quickSlotItem != null)
                quickSlotItemInInventory.Add(quickSlotItem);
        }

        if (quickSlotItemInInventory.Count <= 0)
        {
            equipmentInventoryWindow.SetActive(false);
            ToggleEquipmentButtons(true);
            RefeshMenu();

            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < quickSlotItemInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlots equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlots>();
            equipmentInventorySlot.AddItem(quickSlotItemInInventory[i]);

            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }
    }

    public void SelectEquipment(int equipmentSlot)
    {
        currentSelectedEquipmentSlot = (EquipmentType) equipmentSlot;
    }

    public void UnEquipSelectedItem()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
        Item unequippedItem;

        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                unequippedItem = player.playerInventoryManager.weaponInRightHandSlots[0];
                
                if(unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInRightHandSlots[0] = Instantiate(WorldItemDatabase.instance.unarmedWeapon); 

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                }

                if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                    player.playerNetWorkManager.currentRightHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;

                break;

            case EquipmentType.RightWeapon02:
                unequippedItem = player.playerInventoryManager.weaponInRightHandSlots[1];

                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInRightHandSlots[1] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                }

                if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                    player.playerNetWorkManager.currentRightHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;
            case EquipmentType.RightWeapon03:
                unequippedItem = player.playerInventoryManager.weaponInRightHandSlots[2];

                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInRightHandSlots[2] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                }

                if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                    player.playerNetWorkManager.currentRightHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;
            case EquipmentType.LeftWeapon01:
                unequippedItem = player.playerInventoryManager.weaponInLeftHandSlots[0];

                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInLeftHandSlots[0] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                }

                if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                    player.playerNetWorkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;
            case EquipmentType.LeftWeapon02:
                unequippedItem = player.playerInventoryManager.weaponInLeftHandSlots[1];

                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInLeftHandSlots[1] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                }

                if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                    player.playerNetWorkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;
            case EquipmentType.LeftWeapon03:
                unequippedItem = player.playerInventoryManager.weaponInLeftHandSlots[2];

                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponInLeftHandSlots[2] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                }

                if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                    player.playerNetWorkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
                break;

            case EquipmentType.Head:
                unequippedItem = player.playerInventoryManager.headEquipment;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.headEquipment = null;
                player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);

                break;

            case EquipmentType.Body:
                unequippedItem = player.playerInventoryManager.bodyEquipment;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.bodyEquipment = null;
                player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);

                break;
            case EquipmentType.Hands:
                unequippedItem = player.playerInventoryManager.handEquipment;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.handEquipment = null;
                player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);

                break;

            case EquipmentType.Legs:
                unequippedItem = player.playerInventoryManager.legEquipment;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.legEquipment = null;
                player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);

                break;

            case EquipmentType.MainProjectile:
                unequippedItem = player.playerInventoryManager.mainProjectile;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.mainProjectile = null;
                player.playerEquipmentManager.LoadMainProjectileEquipment(player.playerInventoryManager.mainProjectile);

                break;

            case EquipmentType.SecondaryProjectile:
                unequippedItem = player.playerInventoryManager.secondaryProjectile;

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.secondaryProjectile = null;
                player.playerEquipmentManager.LoadSecondaryProjectileEquipment(player.playerInventoryManager.secondaryProjectile);

                break;


            case EquipmentType.QuickSlot01:
                unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlot[0];

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.quickSlotItemsInQuickSlot[0] = null;

                if (player.playerInventoryManager.quickSlotItemIndex == 0)
                    player.playerNetWorkManager.currentQuickSlotItemID.Value = -1;

                break;

            case EquipmentType.QuickSlot02:
                unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlot[1];

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.quickSlotItemsInQuickSlot[1] = null;

                if (player.playerInventoryManager.quickSlotItemIndex == 1)
                    player.playerNetWorkManager.currentQuickSlotItemID.Value = -1;

                break;


            case EquipmentType.QuickSlot03:
                unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlot[2];

                if (unequippedItem != null)
                    player.playerInventoryManager.AddItemToInventory(unequippedItem);

                player.playerInventoryManager.quickSlotItemsInQuickSlot[2] = null;

                if (player.playerInventoryManager.quickSlotItemIndex == 2)
                    player.playerNetWorkManager.currentQuickSlotItemID.Value = -1;

                break;
            default:
                break;
        }

        RefeshMenu();
    }

}
