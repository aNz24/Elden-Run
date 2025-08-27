using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetWorkManager playerNetWorkManager;
    [HideInInspector] public PlayerStatManager playerStatManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;
    [HideInInspector] public PlayerEffectsManager playerEffectsManager;
    [HideInInspector] public PlayerBodyManager playerBodyManager;
    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetWorkManager = GetComponent<PlayerNetWorkManager>();
        playerStatManager = GetComponent<PlayerStatManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerBodyManager = GetComponent<PlayerBodyManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        playerLocomotionManager.HandleAllMovement();
        playerStatManager.RegenerateStamina();

    }

    protected override void LateUpdate()
    {
        if (!IsOwner)
            return;

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraAction();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallBBack;

        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
            PlayerUIManager.instance.localPlayer = this;
            WorldSaveGameManager.instance.player = this;

            // UPDATE THE TOTAL AMOUNT OF HEALTH OR STAMINA WHEN THE STAT LINKED TO EITHER CHANGES
            playerNetWorkManager.vigor.OnValueChanged += playerNetWorkManager.SetNewMaxHealthValue;
            playerNetWorkManager.endurance.OnValueChanged += playerNetWorkManager.SetNewMaxStaminaValue;
            playerNetWorkManager.mind.OnValueChanged += playerNetWorkManager.SetNewMaxFocusPointValue;

            // UPDATE UI STAT BARS WHEN A STAT CHANGE (HEAL OR STAMINA)
            playerNetWorkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetWorkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetWorkManager.currentFocusPoints.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointValue;
            playerNetWorkManager.currentStamina.OnValueChanged += playerStatManager.ResetStaminaRegenTimer;

            playerNetWorkManager.isAiming.OnValueChanged += playerNetWorkManager.OnIsAimingChanged;
        }


        if (!IsOwner)
            characterNetWorkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;

        //BODY TYPE
        playerNetWorkManager.isMale.OnValueChanged += playerNetWorkManager.OnIsMaleChanged;
        playerNetWorkManager.hairColorRed.OnValueChanged += playerNetWorkManager.OnHairColorRedChanged;
        playerNetWorkManager.hairColorGreen.OnValueChanged += playerNetWorkManager.OnHairColorGreenChanged;
        playerNetWorkManager.hairColorBlue.OnValueChanged += playerNetWorkManager.OnHairColorBlueChanged;
        

        //STAS
        playerNetWorkManager.currentHealth.OnValueChanged += playerNetWorkManager.OnHPChanged;
        playerNetWorkManager.currentFocusPoints.OnValueChanged += playerNetWorkManager.OnFocusPointChanged;
        playerNetWorkManager.maxFocusPoints.OnValueChanged += playerNetWorkManager.OnMaxFocusPointChanged;

        //LOCK ON
        playerNetWorkManager.isLockedOn.OnValueChanged += playerNetWorkManager.OnIsLockOnChanged;
        playerNetWorkManager.currentTargetNetWorkObjectId.OnValueChanged += playerNetWorkManager.OnLockOnTargetIDChange;

        // BODY
        playerNetWorkManager.hairStyleID.OnValueChanged += playerNetWorkManager.OnHairStyleIDChanged;


        //EQUIMENT
        playerNetWorkManager.currentRightHandWeaponID.OnValueChanged += playerNetWorkManager.OnCurrentRightHandWeaponIDChange;
        playerNetWorkManager.currentLeftHandWeaponID.OnValueChanged += playerNetWorkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetWorkManager.currentWeaponBeingUsed.OnValueChanged += playerNetWorkManager.OnCurrentWeaponBeingUsedIDChange;
        playerNetWorkManager.currentQuickSlotItemID.OnValueChanged += playerNetWorkManager.OnCurrentQuickSlotItemIDChange;
        playerNetWorkManager.isChugging.OnValueChanged += playerNetWorkManager.OnIsChuggingChanged;
        playerNetWorkManager.currentSpellID.OnValueChanged += playerNetWorkManager.OnCurrentSpellIDChange;
        playerNetWorkManager.isBlocking.OnValueChanged += playerNetWorkManager.OnIsBlockingChanged;
        playerNetWorkManager.headEquipmentID.OnValueChanged += playerNetWorkManager.OnHeadEquipmentChanged;
        playerNetWorkManager.handEquipmentID.OnValueChanged += playerNetWorkManager.OnHandEquipmentChanged;
        playerNetWorkManager.legEquipmentID.OnValueChanged += playerNetWorkManager.OnLegEquipmentChanged;
        playerNetWorkManager.bodyEquipmentID.OnValueChanged += playerNetWorkManager.OnBodyEquipmentChanged;
        playerNetWorkManager.mainProjectileID.OnValueChanged += playerNetWorkManager.OnMainProjectileIDChange;
        playerNetWorkManager.secondaryProjectileID.OnValueChanged += playerNetWorkManager.OnSecondaryProjectileIDChange;
        playerNetWorkManager.isHodingArrow.OnValueChanged += playerNetWorkManager.OnIsHodingArrowChange;



        //TWO HAND
        playerNetWorkManager.isTwoHandingWeapon.OnValueChanged += playerNetWorkManager.OnIsTwoHandingWeaponChanged;
        playerNetWorkManager.isTwoHandingRightWeapon.OnValueChanged += playerNetWorkManager.OnIsTwoHandRightWeaponChanged;
        playerNetWorkManager.isTwoHandingLeftWeapon.OnValueChanged += playerNetWorkManager.OnIsTwoHandLeftWeaponChanged;

        //FLAGS
        playerNetWorkManager.isChargingAttack.OnValueChanged += playerNetWorkManager.OnIsChargingAttackChanged;



        if (IsOwner && !IsServer)
        {
            LoadGameFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallBBack;

        if (IsOwner)
        {


            // UPDATE THE TOTAL AMOUNT OF HEALTH OR STAMINA WHEN THE STAT LINKED TO EITHER CHANGES
            playerNetWorkManager.vigor.OnValueChanged -= playerNetWorkManager.SetNewMaxHealthValue;
            playerNetWorkManager.endurance.OnValueChanged -= playerNetWorkManager.SetNewMaxStaminaValue;
            playerNetWorkManager.mind.OnValueChanged -= playerNetWorkManager.SetNewMaxFocusPointValue;


            // UPDATE UI STAT BARS WHEN A STAT CHANGE (HEAL OR STAMINA)
            playerNetWorkManager.currentHealth.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
            playerNetWorkManager.currentStamina.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            playerNetWorkManager.currentFocusPoints.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointValue;

            playerNetWorkManager.currentStamina.OnValueChanged -= playerStatManager.ResetStaminaRegenTimer;

            playerNetWorkManager.isAiming.OnValueChanged -= playerNetWorkManager.OnIsAimingChanged;


        }



        if (!IsOwner)
            characterNetWorkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;

        //BODY TYPE
        playerNetWorkManager.isMale.OnValueChanged -= playerNetWorkManager.OnIsMaleChanged;
        playerNetWorkManager.hairColorRed.OnValueChanged -= playerNetWorkManager.OnHairColorRedChanged;
        playerNetWorkManager.hairColorGreen.OnValueChanged -= playerNetWorkManager.OnHairColorGreenChanged;
        playerNetWorkManager.hairColorBlue.OnValueChanged -= playerNetWorkManager.OnHairColorBlueChanged;

        //STAS
        playerNetWorkManager.currentHealth.OnValueChanged -= playerNetWorkManager.OnHPChanged;
        playerNetWorkManager.currentFocusPoints.OnValueChanged -= playerNetWorkManager.OnFocusPointChanged;
        playerNetWorkManager.maxFocusPoints.OnValueChanged -= playerNetWorkManager.OnMaxFocusPointChanged;

        //LOCK ON
        playerNetWorkManager.isLockedOn.OnValueChanged -= playerNetWorkManager.OnIsLockOnChanged;
        playerNetWorkManager.currentTargetNetWorkObjectId.OnValueChanged -= playerNetWorkManager.OnLockOnTargetIDChange;


        // BODY
        playerNetWorkManager.hairStyleID.OnValueChanged -= playerNetWorkManager.OnHairStyleIDChanged;

        //EQUIMENT
        playerNetWorkManager.currentRightHandWeaponID.OnValueChanged -= playerNetWorkManager.OnCurrentRightHandWeaponIDChange;
        playerNetWorkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetWorkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetWorkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetWorkManager.OnCurrentWeaponBeingUsedIDChange;
        playerNetWorkManager.currentQuickSlotItemID.OnValueChanged -= playerNetWorkManager.OnCurrentQuickSlotItemIDChange;
        playerNetWorkManager.isChugging.OnValueChanged -= playerNetWorkManager.OnIsChuggingChanged;
        playerNetWorkManager.currentSpellID.OnValueChanged -= playerNetWorkManager.OnCurrentSpellIDChange;
        playerNetWorkManager.headEquipmentID.OnValueChanged -= playerNetWorkManager.OnHeadEquipmentChanged;
        playerNetWorkManager.handEquipmentID.OnValueChanged -= playerNetWorkManager.OnHandEquipmentChanged;
        playerNetWorkManager.legEquipmentID.OnValueChanged -= playerNetWorkManager.OnLegEquipmentChanged;
        playerNetWorkManager.bodyEquipmentID.OnValueChanged -= playerNetWorkManager.OnBodyEquipmentChanged;
        playerNetWorkManager.mainProjectileID.OnValueChanged -= playerNetWorkManager.OnMainProjectileIDChange;
        playerNetWorkManager.secondaryProjectileID.OnValueChanged -= playerNetWorkManager.OnSecondaryProjectileIDChange;
        playerNetWorkManager.isHodingArrow.OnValueChanged -= playerNetWorkManager.OnIsHodingArrowChange;



        //TWO HAND
        playerNetWorkManager.isTwoHandingWeapon.OnValueChanged -= playerNetWorkManager.OnIsTwoHandingWeaponChanged;
        playerNetWorkManager.isTwoHandingRightWeapon.OnValueChanged -= playerNetWorkManager.OnIsTwoHandRightWeaponChanged;
        playerNetWorkManager.isTwoHandingLeftWeapon.OnValueChanged -= playerNetWorkManager.OnIsTwoHandLeftWeaponChanged;

        playerNetWorkManager.isChargingAttack.OnValueChanged -= playerNetWorkManager.OnIsChargingAttackChanged;

    }

    public void OnClientConnectedCallBBack(ulong clientID)
    {
        WorldGameSessionManager.instance.AddPlayerToActivePlayersList(this);

        // WE ARE THE SERVER, WE ARE THE HOST, SO WE DONT NEED TO LOAD PLAYERS TO SYNC THEM
        // YOU ONLY NEED TO LOAD OTHER PLAYER GEAR TO SYNC IT IF YOU JION A GAME THATS ALREADY BEEN ACTIVE WITHOUT YOU BEING PRESENT    
        if(!IsServer && IsOwner)
        {
            foreach (var player in WorldGameSessionManager.instance.players)
            {
                if(player != this)
                {
                    player.LoadOtherPlayerCharacterWhenJoiningServer();
                }
            }
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manaullySelectDeathAnimation = false)
    {

        if (IsOwner)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
        }

        WorldGameSessionManager.instance.WaitThenReviveHost();

        return base.ProcessDeathEvent(manaullySelectDeathAnimation);

    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();
         
        if(IsOwner)
        {
            isDead.Value = false;
            playerNetWorkManager.currentHealth.Value = playerNetWorkManager.maxHealth.Value;
            playerNetWorkManager.currentStamina.Value = playerNetWorkManager.maxStamina.Value;

            playerAnimatorManager.PlayTargetActionAnimation("Empty",false);
        }
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentCharacterData.characterName = playerNetWorkManager.characterName.Value.ToString();
        currentCharacterData.isMale = playerNetWorkManager.isMale.Value;
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = playerNetWorkManager.currentHealth.Value;
        currentCharacterData.currentStamina = playerNetWorkManager.currentStamina.Value;
        currentCharacterData.currentFocusPoints = playerNetWorkManager.currentFocusPoints.Value;

        currentCharacterData.vigor = playerNetWorkManager.vigor.Value;
        currentCharacterData.endurance = playerNetWorkManager.endurance.Value;
        currentCharacterData.mind = playerNetWorkManager.mind.Value;
        currentCharacterData.strength = playerNetWorkManager.strength.Value;
        currentCharacterData.dexterity = playerNetWorkManager.dexterity.Value;
        currentCharacterData.intelligence = playerNetWorkManager.intelligence.Value;
        currentCharacterData.faith = playerNetWorkManager.faith.Value;

        //RUNES
        currentCharacterData.runes = playerStatManager.runes;

        // BODY
        currentCharacterData.hairStyleID =playerNetWorkManager.hairStyleID.Value;
        currentCharacterData.hairColorRed = playerNetWorkManager.hairColorRed.Value;
        currentCharacterData.hairColorGreen = playerNetWorkManager.hairColorGreen.Value;
        currentCharacterData.hairColorBlue = playerNetWorkManager.hairColorBlue.Value;

        currentCharacterData.currentHealthFlaskRemaining = playerNetWorkManager.remainingHealthFlasks.Value;
        currentCharacterData.currentFocusPointsFlaskRemaining = playerNetWorkManager.remainingFocusPointFlasks.Value;

        // EQUIPMENT
        currentCharacterData.headEquipment = playerNetWorkManager.headEquipmentID.Value;
        currentCharacterData.bodyEquipment = playerNetWorkManager.bodyEquipmentID.Value;
        currentCharacterData.legEquipment = playerNetWorkManager.legEquipmentID.Value;
        currentCharacterData.handEquipment = playerNetWorkManager.handEquipmentID.Value;

        currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;
        currentCharacterData.rightWeapon01 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponInRightHandSlots[0]);
        currentCharacterData.rightWeapon02 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponInRightHandSlots[1]);
        currentCharacterData.rightWeapon03 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponInRightHandSlots[2]);


        currentCharacterData.lefttWeaponIndex = playerInventoryManager.leftHandWeaponIndex;
        currentCharacterData.lefttWeapon01 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponInLeftHandSlots[0]);
        currentCharacterData.lefttWeapon02 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponInLeftHandSlots[1]);
        currentCharacterData.lefttWeapon03 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponInLeftHandSlots[2]);

        currentCharacterData.quickSlotIndex  = playerInventoryManager.quickSlotItemIndex;
        currentCharacterData.quickSlotItem01 = WorldSaveGameManager.instance.GetSerializableQuickSloteFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlot[0]);
        currentCharacterData.quickSlotItem02 = WorldSaveGameManager.instance.GetSerializableQuickSloteFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlot[1]);
        currentCharacterData.quickSlotItem03 = WorldSaveGameManager.instance.GetSerializableQuickSloteFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlot[2]);

        currentCharacterData.mainProjectile = WorldSaveGameManager.instance.GetSerializableRangedProjectileFromWeaponItem(playerInventoryManager.mainProjectile);
        currentCharacterData.secondaryProjectile = WorldSaveGameManager.instance.GetSerializableRangedProjectileFromWeaponItem(playerInventoryManager.secondaryProjectile);

        if(playerInventoryManager.currentSpell != null)
            currentCharacterData.currentSpell = playerInventoryManager.currentSpell.itemID;

        currentCharacterData.weaponsInInventory = new List<SerializableWeapon>();
        currentCharacterData.projectilesInInventory = new List<SerizlizableProjectile>();
        currentCharacterData.quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();
        currentCharacterData.headEquipmentInInventory = new List<int>();
        currentCharacterData.handEquipmentInInventory = new List<int>();
        currentCharacterData.bodyEquipmentInInventory = new List<int>();
        currentCharacterData.legEquipmentInInventory = new List<int>();


        for (int i = 0; i < playerInventoryManager.itemInInventory.Count; i++)
        {
            if (playerInventoryManager.itemInInventory[i] == null)
                continue;

            WeaponItem weaponInInventory = playerInventoryManager.itemInInventory[i] as WeaponItem;
            HeadEquipmentItem headEquipmentInInventory = playerInventoryManager.itemInInventory[i] as HeadEquipmentItem;
            HandEquipmentItem handEquipmentInInventory = playerInventoryManager.itemInInventory[i] as HandEquipmentItem;
            BodyEquipmentItem bodyEquipmentInInventory = playerInventoryManager.itemInInventory[i] as BodyEquipmentItem;
            LegEquipmentItem legEquipmentInInventory = playerInventoryManager.itemInInventory[i] as LegEquipmentItem;
            QuickSlotItem quickSlotItemInInventory = playerInventoryManager.itemInInventory[i] as  QuickSlotItem;
            RangedProjectileItem projectileInInventory = playerInventoryManager.itemInInventory[i] as RangedProjectileItem;

            if (weaponInInventory != null)
                currentCharacterData.weaponsInInventory.Add(WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(weaponInInventory));

            if(headEquipmentInInventory != null)
                currentCharacterData.headEquipmentInInventory.Add(headEquipmentInInventory.itemID);

            if (handEquipmentInInventory != null)
                currentCharacterData.handEquipmentInInventory.Add(handEquipmentInInventory.itemID);

            if (bodyEquipmentInInventory != null)
                currentCharacterData.bodyEquipmentInInventory.Add(bodyEquipmentInInventory.itemID);

            if (legEquipmentInInventory != null)
                currentCharacterData.legEquipmentInInventory.Add(legEquipmentInInventory.itemID);


            if (quickSlotItemInInventory != null)
                currentCharacterData.quickSlotItemsInInventory.Add(WorldSaveGameManager.instance.GetSerializableQuickSloteFromQuickSlotItem(quickSlotItemInInventory));


            if (projectileInInventory != null)
                currentCharacterData.projectilesInInventory.Add(WorldSaveGameManager.instance.GetSerializableRangedProjectileFromWeaponItem(projectileInInventory));
        }

    }

    public void LoadGameFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        playerNetWorkManager.characterName.Value = currentCharacterData.characterName;
        playerNetWorkManager.isMale.Value =currentCharacterData.isMale;
        playerBodyManager.ToggleBodyType(currentCharacterData.isMale);
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition + 7, currentCharacterData.zPosition);
        transform.position = myPosition;


        playerNetWorkManager.vigor.Value = currentCharacterData.vigor;
        playerNetWorkManager.endurance.Value = currentCharacterData.endurance;
        playerNetWorkManager.mind.Value = currentCharacterData.mind;
        playerNetWorkManager.strength.Value = currentCharacterData.strength;
        playerNetWorkManager.dexterity.Value = currentCharacterData.dexterity;
        playerNetWorkManager.intelligence.Value = currentCharacterData.intelligence;
        playerNetWorkManager.faith.Value = currentCharacterData.faith;


        //RUNES 
        //playerStatManager.runes = currentCharacterData.runes;
        //PlayerUIManager.instance.playerUIHudManager.SetRunesCount(currentCharacterData.runes);
        playerStatManager.AddRunes(currentCharacterData.runes);


        //THIS WILL BE MOVED WHEN SAVING AND LOADING IS ADDED
        playerNetWorkManager.maxHealth.Value = playerStatManager.CaculateHealthBasedOnVitalityLevel(playerNetWorkManager.vigor.Value);
        playerNetWorkManager.maxStamina.Value = playerStatManager.CaculateStaminaBasedEnduranceLevel(playerNetWorkManager.endurance.Value);
        playerNetWorkManager.maxFocusPoints.Value = playerStatManager.CaculateFocusPointsBasedOnMindLevel(playerNetWorkManager.mind.Value);
        playerNetWorkManager.currentHealth.Value = currentCharacterData.currentHealth;
        playerNetWorkManager.currentStamina.Value = currentCharacterData.currentStamina;
        playerNetWorkManager.currentFocusPoints.Value = currentCharacterData.currentFocusPoints;

        playerNetWorkManager.remainingHealthFlasks.Value = currentCharacterData.currentHealthFlaskRemaining;
        playerNetWorkManager.remainingFocusPointFlasks.Value = currentCharacterData.currentFocusPointsFlaskRemaining;



        // BODY
        playerNetWorkManager.hairStyleID.Value = currentCharacterData.hairStyleID;
        playerNetWorkManager.hairColorRed.Value = currentCharacterData.hairColorRed;
        playerNetWorkManager.hairColorGreen.Value = currentCharacterData.hairColorGreen;
        playerNetWorkManager.hairColorBlue.Value = currentCharacterData.hairColorBlue;

        // EQUIPMENT

        if (WorldItemDatabase.instance.GetHeadEquipmentByID(currentCharacterData.headEquipment))
        {
            HeadEquipmentItem headEquipment = Instantiate(WorldItemDatabase.instance.GetHeadEquipmentByID(currentCharacterData.headEquipment));
            playerInventoryManager.headEquipment = headEquipment;
        }
        else
        {
            playerInventoryManager.headEquipment = null;

        }


        if (WorldItemDatabase.instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipment))
        {
            BodyEquipmentItem bodyEquipment = Instantiate(WorldItemDatabase.instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipment));
            playerInventoryManager.bodyEquipment = bodyEquipment; 
        }
        else
        {
            playerInventoryManager.bodyEquipment = null;

        }

        if (WorldItemDatabase.instance.GetLegEquipmentByID(currentCharacterData.legEquipment))
        {
            LegEquipmentItem legEquipment = Instantiate(WorldItemDatabase.instance.GetLegEquipmentByID(currentCharacterData.legEquipment));
            playerInventoryManager.legEquipment = legEquipment;
        }
        else
        {
            playerInventoryManager.legEquipment = null;

        }


        if (WorldItemDatabase.instance.GetHandEquipmentByID(currentCharacterData.handEquipment))
        {
            HandEquipmentItem handEquipment = Instantiate(WorldItemDatabase.instance.GetHandEquipmentByID(currentCharacterData.handEquipment));
            playerInventoryManager.handEquipment = handEquipment;
        }
        else
        {
            playerInventoryManager.handEquipment = null;

        }

        // WEAPON
        playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex; 
        playerInventoryManager.weaponInRightHandSlots[0] = currentCharacterData.rightWeapon01.GetWeapon();
        playerInventoryManager.weaponInRightHandSlots[1] = currentCharacterData.rightWeapon02.GetWeapon();
        playerInventoryManager.weaponInRightHandSlots[2] = currentCharacterData.rightWeapon03.GetWeapon();

        playerInventoryManager.leftHandWeaponIndex = currentCharacterData.lefttWeaponIndex;
        playerInventoryManager.weaponInLeftHandSlots[0] = currentCharacterData.lefttWeapon01.GetWeapon();
        playerInventoryManager.weaponInLeftHandSlots[1] = currentCharacterData.lefttWeapon02.GetWeapon();
        playerInventoryManager.weaponInLeftHandSlots[2] = currentCharacterData.lefttWeapon03.GetWeapon();


        // QUICK SLOT ITEMS
        playerInventoryManager.quickSlotItemIndex = currentCharacterData.quickSlotIndex;    
        playerInventoryManager.quickSlotItemsInQuickSlot[0] = currentCharacterData.quickSlotItem01.GetQuickSlotItem();
        playerInventoryManager.quickSlotItemsInQuickSlot[1] = currentCharacterData.quickSlotItem02.GetQuickSlotItem();
        playerInventoryManager.quickSlotItemsInQuickSlot[2] = currentCharacterData.quickSlotItem03.GetQuickSlotItem();
        playerEquipmentManager.LoadQuickSlotEquipment(playerInventoryManager.quickSlotItemsInQuickSlot[playerInventoryManager.quickSlotItemIndex]);


        if (currentCharacterData.rightWeaponIndex >= 0)
        {
            playerInventoryManager.currentRightHandWeapon = playerInventoryManager.weaponInRightHandSlots[currentCharacterData.rightWeaponIndex];
            playerNetWorkManager.currentRightHandWeaponID.Value = playerInventoryManager.weaponInRightHandSlots[currentCharacterData.rightWeaponIndex].itemID;
        }
        else
        {
            playerNetWorkManager.currentRightHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
        }


        playerInventoryManager.leftHandWeaponIndex = currentCharacterData.lefttWeaponIndex;


        if (currentCharacterData.lefttWeaponIndex >= 0)
        {
            playerInventoryManager.currentLeftHandWeapon = playerInventoryManager.weaponInLeftHandSlots[currentCharacterData.lefttWeaponIndex];
            playerNetWorkManager.currentLeftHandWeaponID.Value = playerInventoryManager.weaponInLeftHandSlots[currentCharacterData.lefttWeaponIndex].itemID;
        }
        else
        {
            playerNetWorkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;

        }


        if (WorldItemDatabase.instance.GetSpellByID(currentCharacterData.currentSpell))
        {

            SpellItem currentSpell = Instantiate(WorldItemDatabase.instance.GetSpellByID(currentCharacterData.currentSpell));
            playerNetWorkManager.currentSpellID.Value =currentSpell.itemID;
        }
        else
        {
            playerNetWorkManager.currentSpellID.Value = -1;
        }

        for (int i = 0; i < currentCharacterData.weaponsInInventory.Count; i++)
        {
            WeaponItem weapon = currentCharacterData.weaponsInInventory[i].GetWeapon();
            playerInventoryManager.AddItemToInventory(weapon);
        }

        for (int i = 0; i < currentCharacterData.headEquipmentInInventory.Count; i++)
        {
            EquipmentItem headEquipment = WorldItemDatabase.instance.GetHeadEquipmentByID(currentCharacterData.headEquipmentInInventory[i]);
            playerInventoryManager.AddItemToInventory(headEquipment);
        }

        for (int i = 0; i < currentCharacterData.handEquipmentInInventory.Count; i++)
        {
            EquipmentItem handEquipment = WorldItemDatabase.instance.GetHandEquipmentByID(currentCharacterData.handEquipmentInInventory[i]);
            playerInventoryManager.AddItemToInventory(handEquipment);
        }

        for (int i = 0; i < currentCharacterData.bodyEquipmentInInventory.Count; i++)
        {
            EquipmentItem bodyEquipment = WorldItemDatabase.instance.GetBodyEquipmentByID(currentCharacterData.bodyEquipmentInInventory[i]);
            playerInventoryManager.AddItemToInventory(bodyEquipment);
        }

        for (int i = 0; i < currentCharacterData.legEquipmentInInventory.Count; i++)
        {
            EquipmentItem legEquipment = WorldItemDatabase.instance.GetLegEquipmentByID(currentCharacterData.legEquipmentInInventory[i]);
            playerInventoryManager.AddItemToInventory(legEquipment);
        }

   

        for (int i = 0; i < currentCharacterData.quickSlotItemsInInventory.Count; i++)
        {
            QuickSlotItem quickSlot = currentCharacterData.quickSlotItemsInInventory[i].GetQuickSlotItem();
            playerInventoryManager.AddItemToInventory(quickSlot);
        }

        for (int i = 0; i < currentCharacterData.projectilesInInventory.Count; i++)
        {
            RangedProjectileItem projectile = currentCharacterData.projectilesInInventory[i].GetProjectile();
            playerInventoryManager.AddItemToInventory(projectile);
        }
        playerEquipmentManager.EquipArmor();
        playerEquipmentManager.LoadMainProjectileEquipment(currentCharacterData.mainProjectile.GetProjectile());
        playerEquipmentManager.LoadSecondaryProjectileEquipment(currentCharacterData.secondaryProjectile.GetProjectile());
      

    }

    public void LoadOtherPlayerCharacterWhenJoiningServer()
    {
        // SYNC BODY TYPE
        playerNetWorkManager.OnIsMaleChanged(false, playerNetWorkManager.isMale.Value);
        playerNetWorkManager.OnHairStyleIDChanged(0, playerNetWorkManager.hairStyleID.Value);
        playerNetWorkManager.OnHairColorRedChanged(0, playerNetWorkManager.hairColorRed.Value);
        playerNetWorkManager.OnHairColorGreenChanged(0, playerNetWorkManager.hairColorGreen.Value);
        playerNetWorkManager.OnHairColorBlueChanged(0, playerNetWorkManager.hairColorBlue.Value);

        // SYNC WEAPON
        playerNetWorkManager.OnCurrentRightHandWeaponIDChange(0 , playerNetWorkManager.currentRightHandWeaponID.Value);
        playerNetWorkManager.OnCurrentLeftHandWeaponIDChange(0,playerNetWorkManager.currentLeftHandWeaponID.Value);
        playerNetWorkManager.OnCurrentSpellIDChange(0 ,playerNetWorkManager.currentSpellID.Value);


        //
        playerNetWorkManager.OnMainProjectileIDChange(0 ,playerNetWorkManager.mainProjectileID.Value);
        playerNetWorkManager.OnSecondaryProjectileIDChange(0 ,playerNetWorkManager.secondaryProjectileID.Value);

        // SYNC ARMOR
        playerNetWorkManager.OnHeadEquipmentChanged(0, playerNetWorkManager.headEquipmentID.Value);
        playerNetWorkManager.OnBodyEquipmentChanged(0, playerNetWorkManager.bodyEquipmentID.Value);
        playerNetWorkManager.OnHandEquipmentChanged(0, playerNetWorkManager.handEquipmentID.Value);
        playerNetWorkManager.OnLegEquipmentChanged(0, playerNetWorkManager.legEquipmentID.Value);

        //SYNC TWO HAND STATUS
        playerNetWorkManager.OnIsTwoHandRightWeaponChanged(false, playerNetWorkManager.isTwoHandingRightWeapon.Value);
        playerNetWorkManager.OnIsTwoHandLeftWeaponChanged(false, playerNetWorkManager.isTwoHandingLeftWeapon.Value);
        playerNetWorkManager.OnIsHodingArrowChange(false, playerNetWorkManager.isHodingArrow.Value);

        //SYNC BLOCK STATUS
        playerNetWorkManager.OnIsBlockingChanged(false, playerNetWorkManager.isBlocking.Value);

        // ARMOR

        //LOCK ON
        if(playerNetWorkManager.isLockedOn.Value)
        {
            playerNetWorkManager.OnLockOnTargetIDChange(0,playerNetWorkManager.currentTargetNetWorkObjectId.Value); 
        }
    } 
}
