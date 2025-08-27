using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.TextCore.Text;
using UnityEngine.Experimental.GlobalIllumination;
using Unity.VisualScripting;

public class PlayerNetWorkManager : CharacterNetWorkManager
{
    PlayerManager player;

    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Flask")]
    public NetworkVariable<int> remainingHealthFlasks = new NetworkVariable<int>(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> remainingFocusPointFlasks = new NetworkVariable<int>(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isChugging = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Equiment")]
    public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentSpellID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentQuickSlotItemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingRightHand  = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingLeftHand  = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Body")]
    public NetworkVariable<int> hairStyleID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> hairColorRed = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> hairColorGreen = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> hairColorBlue= new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Two Handing")]
    public NetworkVariable<int> currentWeaponBeingTwoHanded = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isTwoHandingWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isTwoHandingRightWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isTwoHandingLeftWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Armor")]
    public NetworkVariable<bool> isMale = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> headEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> bodyEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> legEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> handEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Projectiles")]
    public NetworkVariable<int> mainProjectileID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> secondaryProjectileID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasArrowNotched = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isHodingArrow = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isAiming = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public override void OnIsDeadChanged(bool oldStatus, bool newStatus)
    {
        base.OnIsDeadChanged(oldStatus, newStatus);

        if (player.isDead.Value)
            player.playerCombatManager.CreateDeadSpot(player.transform.position, player.playerStatManager.runes);

        if (player.isDead.Value && NetworkManager.Singleton.IsServer)
        {
            if (PlayerUIManager.instance.playerUIHudManager.currentBossHealthBar)
                PlayerUIManager.instance.playerUIHudManager.currentBossHealthBar.RemoveHPBar(1f);

            WorldAIManager.instance.DisableAllBossFights();
        }

    }

    public void SetCharacterActionHand(bool rightHandedAction)
    {
        if(rightHandedAction)
        {
            isUsingLeftHand.Value = false;
            isUsingRightHand.Value = true;
        }
        else
        {
            isUsingRightHand.Value = false;
            isUsingLeftHand.Value = true;
        }
    }

    public void SetNewMaxHealthValue(int oldVitality, int newVitality)
    {
        maxHealth.Value = player.playerStatManager.CaculateHealthBasedOnVitalityLevel(newVitality);
        PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
        currentHealth.Value = maxHealth.Value;
    }

    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
    {
        maxStamina.Value = player.playerStatManager.CaculateStaminaBasedEnduranceLevel(newEndurance);
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
        currentStamina.Value = maxStamina.Value;
    }

    public void SetNewMaxFocusPointValue(int oldMind, int newMind)
    {
        maxFocusPoints.Value = player.playerStatManager.CaculateFocusPointsBasedOnMindLevel(newMind);
        PlayerUIManager.instance.playerUIHudManager.SetMaxFocusPointValue(maxFocusPoints.Value);
        currentFocusPoints.Value = maxFocusPoints.Value;
    }

    public void OnHairStyleIDChanged(int oldID, int newID)
    {
        player.playerBodyManager.ToggleHairStyle( hairStyleID.Value);
    }

    public void OnHairColorRedChanged(float oldValue, float newValue)
    {
        player.playerBodyManager.SetHairColor();
    }

    public void OnHairColorGreenChanged(float oldValue, float newValue)
    {
        player.playerBodyManager.SetHairColor();
    }

    public void OnHairColorBlueChanged(float oldValue, float newValue)
    {
        player.playerBodyManager.SetHairColor();
    }

    public void OnCurrentRightHandWeaponIDChange(int oldID , int newID) 
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentRightHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadRightWeapon();

        if (player.IsOwner)
        {
            PlayerUIManager.instance.playerUIHudManager.SetRightWeaponQuickSlotIcon(newID);

            if (player.playerInventoryManager.currentRightHandWeapon.weaponClass == WeaponClass.Bow)
            {
                PlayerUIManager.instance.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(true);
            }
            else
            {
                PlayerUIManager.instance.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(false);
            }
        }
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadLeftWeapon();

        if (player.IsOwner)
        {
            PlayerUIManager.instance.playerUIHudManager.SetLeftWeaponQuickSlotIcon(newID);


            if (player.playerInventoryManager.currentLeftHandWeapon.weaponClass == WeaponClass.Bow)
            {
                PlayerUIManager.instance.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(true);
            }
            else
            {
                PlayerUIManager.instance.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(false);
            }
        }

    }

    public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

        if (player.IsOwner)
            return;

        if (player.playerCombatManager.currentWeaponBeingUsed != null)
            player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
       
    }

    public void OnCurrentSpellIDChange(int oldID, int newID)
    {
        SpellItem newSpell = null;

        if(WorldItemDatabase.instance.GetSpellByID(newID))
            newSpell = Instantiate(WorldItemDatabase.instance.GetSpellByID(newID));


        if (newSpell != null)
        {
            player.playerInventoryManager.currentSpell = newSpell;

            if (player.IsOwner)
                PlayerUIManager.instance.playerUIHudManager.SetSpellItemQuickSlotIcon(newID);
        }

 

    }

    public void OnCurrentQuickSlotItemIDChange(int oldID, int newID)
    {
        QuickSlotItem newQuikSlotItem = null;

        if (WorldItemDatabase.instance.GetQuickSlotItemByID(newID))
            newQuikSlotItem = Instantiate(WorldItemDatabase.instance.GetQuickSlotItemByID(newID));


        if (newQuikSlotItem != null)
        {
            player.playerInventoryManager.currentQuickSlotItem = newQuikSlotItem;
        }
        else
        {
            player.playerInventoryManager.currentQuickSlotItem = null;
        }

        if (player.IsOwner)
            PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);
    }

    public void OnMainProjectileIDChange(int oldID, int newID)
    {
        RangedProjectileItem newProjectile = null;

        if (WorldItemDatabase.instance.GetSpellByID(newID))
            newProjectile = Instantiate(WorldItemDatabase.instance.GetProjectileByID(newID));


        if (newProjectile != null)
            player.playerInventoryManager.mainProjectile = newProjectile;

        if (player.IsOwner)
            PlayerUIManager.instance.playerUIHudManager.SetMainProjectileQuickSlotIcon(player.playerInventoryManager.mainProjectile);
    }

    public void OnMaxFocusPointChanged(int oldFP, int newFP)
    {
        if(player.IsOwner)
            PlayerUIManager.instance.playerUIHudManager.SetMaxFocusPointValue(newFP);    
    }

    public void OnFocusPointChanged(int oldFP, int newFP)
    {
        if (player.IsOwner)
            PlayerUIManager.instance.playerUIHudManager.SetNewFocusPointValue(oldFP, newFP);
    }

    public void OnSecondaryProjectileIDChange(int oldID, int newID)
    {
        RangedProjectileItem newProjectile = null;

        if (WorldItemDatabase.instance.GetSpellByID(newID))
            newProjectile = Instantiate(WorldItemDatabase.instance.GetProjectileByID(newID));


        if (newProjectile != null)
            player.playerInventoryManager.secondaryProjectile = newProjectile;

        if (player.IsOwner)
            PlayerUIManager.instance.playerUIHudManager.SetSecondaryProjectileQuickSlotIcon(player.playerInventoryManager.secondaryProjectile);
    }

    public void OnIsHodingArrowChange(bool oldStatus, bool newStatus)
    {
        player.animator.SetBool("isHodingArrow", isHodingArrow.Value);
    }

    public void OnIsAimingChanged(bool oldStatus, bool newStatus)
    {
        if (!isAiming.Value)
        {
            PlayerCamera.instance.cameraObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            PlayerCamera.instance.cameraObject.fieldOfView = 60;
            PlayerCamera.instance.cameraObject.nearClipPlane = .3f;
            PlayerCamera.instance.cameraPivotTranform.localPosition = new Vector3(0, PlayerCamera.instance.cameraPivotYPositionOffset, 0);
            PlayerUIManager.instance.playerUIHudManager.crossHair.SetActive(false);

        }else
        {
            PlayerCamera.instance.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            PlayerCamera.instance.cameraPivotTranform.localEulerAngles = new Vector3(0, 0, 0);
            PlayerCamera.instance.cameraObject.fieldOfView = 40;
            PlayerCamera.instance.cameraObject.nearClipPlane = 1.3f;
            PlayerCamera.instance.cameraPivotTranform.localPosition = Vector3.zero;

            PlayerUIManager.instance.playerUIHudManager.crossHair.SetActive(true);

        }
    }

    public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)
    {
        base.OnIsBlockingChanged(oldStatus, newStatus);

        if (IsOwner)
        {
            player.playerStatManager.blockingPhysicalAbsorptions = player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
            player.playerStatManager.blockingFireAbsorptions = player.playerCombatManager.currentWeaponBeingUsed.fireBaseDamageAbsorption;
            player.playerStatManager.blockingHolyAbsorptions = player.playerCombatManager.currentWeaponBeingUsed.holyBaseDamageAbsorption;
            player.playerStatManager.blockingMagicAbsorptions = player.playerCombatManager.currentWeaponBeingUsed.magiclBaseDamageAbsorption;
            player.playerStatManager.blockingLightingAbsorptions = player.playerCombatManager.currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
            player.playerStatManager.blockingStability = player.playerCombatManager.currentWeaponBeingUsed.stability;
        }
    }

    public  void OnIsTwoHandingWeaponChanged(bool oldStatus , bool newStatus)
    {
        if (!isTwoHandingWeapon.Value)
        {
            if (IsOwner)
            {
                isTwoHandingLeftWeapon.Value = false;
                isTwoHandingRightWeapon.Value = false;
            }

            player.playerEquipmentManager.UnTwoHandWeapon();
            player.playerEffectsManager.RemoveStaticEffect(WorldCharacterEffectsManager.instance.twoHandingEffect.staticEffectID);

        }
        else
        {
            StaticCharacterEffect twoHandEffect = Instantiate(WorldCharacterEffectsManager.instance.twoHandingEffect);
            player.playerEffectsManager.AddStaticEffect(twoHandEffect);
        }
        player.animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon.Value);

    }

    public void OnIsTwoHandRightWeaponChanged(bool oldStatus, bool newStatus)
    {
        if (!isTwoHandingRightWeapon.Value)
            return;

        if (IsOwner)
        {
            currentWeaponBeingTwoHanded.Value = currentRightHandWeaponID.Value;
            isTwoHandingWeapon.Value = true;
        }

     
        player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentRightHandWeapon;
        player.playerEquipmentManager.TwoHandRightWeapon();
    }

    public void OnIsTwoHandLeftWeaponChanged(bool oldStatus, bool newStatus)
    {
        if (!isTwoHandingLeftWeapon.Value)
            return;

        if (IsOwner)
        {

            currentWeaponBeingTwoHanded.Value = currentLeftHandWeaponID.Value;
            isTwoHandingWeapon.Value = true;
        }

        player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentLeftHandWeapon;
        player.playerEquipmentManager.TwoHandLeftWeapon();
    }

    public void OnIsChuggingChanged(bool oldStatus, bool newStatus)
    {
        player.animator.SetBool("isChugging", isChugging.Value);
    }

    public void OnHeadEquipmentChanged(int oldValue, int newValue)
    {
        //
        if (IsOwner)
            return;

        HeadEquipmentItem equipment = WorldItemDatabase.instance.GetHeadEquipmentByID(headEquipmentID.Value);

        if(equipment != null)
        {
            player.playerEquipmentManager.LoadHeadEquipment(Instantiate(equipment));
        }
        else
        {
            player.playerEquipmentManager.LoadHeadEquipment(null);

        }
    }

    public void OnBodyEquipmentChanged(int oldValue, int newValue)
    {
        //
        if (IsOwner)
            return;

        BodyEquipmentItem equipment = WorldItemDatabase.instance.GetBodyEquipmentByID(bodyEquipmentID.Value);

        if (equipment != null)
        {
            player.playerEquipmentManager.LoadBodyEquipment(Instantiate(equipment));
        }
        else
        {
            player.playerEquipmentManager.LoadBodyEquipment(null);

        }
    }

    public void OnLegEquipmentChanged(int oldValue, int newValue)
    {
        //
        if (IsOwner)
            return;

        LegEquipmentItem equipment = WorldItemDatabase.instance.GetLegEquipmentByID(legEquipmentID.Value);

        if (equipment != null)
        {
            player.playerEquipmentManager.LoadLegEquipment(Instantiate(equipment));
        }
        else
        {
            player.playerEquipmentManager.LoadLegEquipment(null);

        }
    }

    public void OnHandEquipmentChanged(int oldValue, int newValue)
    {
        //
        if (IsOwner)
            return;

        HandEquipmentItem equipment = WorldItemDatabase.instance.GetHandEquipmentByID(handEquipmentID.Value);

        if (equipment != null)
        {
            player.playerEquipmentManager.LoadHandEquipment(Instantiate(equipment));
        }
        else
        {
            player.playerEquipmentManager.LoadHandEquipment(null);

        }
    }

    public void OnIsMaleChanged(bool oldStatus, bool newStatus)
    {
        player.playerBodyManager.ToggleBodyType(isMale.Value);

    }

    //Item Action
    [ServerRpc]
    public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID , int actionID , int weaponID)
    {
        if (IsServer)
        {
            NotifyTheServerOfWeaponActionClientRpc(clientID, actionID,weaponID);
        }
    }

    [ClientRpc]
    public void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
    {
        if(clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformWeaponBasedAction(actionID , weaponID);
        }
    }

    private void PerformWeaponBasedAction(int actionID, int weaponID)
    {
        WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

        if(weaponAction != null)
        {
            weaponAction.AttempToPerformAction(player, WorldItemDatabase.instance.GetWeaponByID(weaponID));
        }
        else
        {
            Debug.LogError("ACTION IS NULL");
        }
    }

    [ClientRpc]
    public override void DestroyAllCurrentActionFXClientRpc()
    {

        if (player.characterEffectManager.activeSpellWarnUpFX != null)
            Destroy(player.characterEffectManager.activeSpellWarnUpFX);

        if (player.characterEffectManager.activeDrawProjectileFX != null)
            Destroy(player.characterEffectManager.activeDrawProjectileFX);


        if (player.characterEffectManager.activeQuickSlotItemFX != null)
            Destroy(player.characterEffectManager.activeQuickSlotItemFX);

        if (hasArrowNotched.Value)
        {
            if (player.IsOwner)
                hasArrowNotched.Value = false;
        }

    }

    //Draw Projectile
    [ServerRpc]
    public void NotifyTheServerOfDrawProjectileServerRpc( int projectileID)
    {
        if (IsServer)
        {
            NotifyTheServerOfDrawProjectileClientRpc(projectileID);
        }
    }

    [ClientRpc]
    private void NotifyTheServerOfDrawProjectileClientRpc(int projectileID)
    {
        //Animator bowAnimator;
        
        //if (isTwoHandingLeftWeapon.Value)
        //{
        //    bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
        //}
        //else
        //{
        //    bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();

        //}

        //bowAnimator.SetBool("isDrawn", true);
        //bowAnimator.Play("Bow_Draw_01");

        GameObject arrow = Instantiate(WorldItemDatabase.instance.GetProjectileByID(projectileID).drawProjectileModel, player.playerEquipmentManager.leftHandWeaponSlot.transform);
        player.playerEffectsManager.activeDrawProjectileFX= arrow;

        player.characterSFXManager.PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(WorldSFXManger.instance.notchArrowSFX));

    }

    // RELEASE PROJECTILE
    [ServerRpc]
    public void NotifyTheServerOfReleaseProjectileServerRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
    {
        if (IsServer)
        {
            NotifyTheServerOfReleaseProjectileClientRpc(playerClientID , projectileID , xPosition,yPosition,zPosition ,yCharacterRotation);
        }
    }

    [ClientRpc]
    public void NotifyTheServerOfReleaseProjectileClientRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
    {
        if (playerClientID != NetworkManager.Singleton.LocalClientId)
            PerformReleaseProjectileFromRpc(projectileID , xPosition , yPosition,zPosition,yCharacterRotation);
    }

    private void PerformReleaseProjectileFromRpc(int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
    {
        RangedProjectileItem projectileItem = null;

        if(WorldItemDatabase.instance.GetProjectileByID(projectileID) != null)
            projectileItem = WorldItemDatabase.instance.GetProjectileByID(projectileID);


        if (projectileItem == null)
            return;


        Transform projectileInstantiationLocation;
        GameObject projectileGameObject;
        Rigidbody projectileRigibody;
        RangedProjectileDamageCollider projectileDamageCollider;


        // UPDATE ARROW
        projectileInstantiationLocation = player.playerCombatManager.lockOnTranfrom;
        projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation);
        projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
        projectileRigibody = projectileGameObject.GetComponent<Rigidbody>();

        //
        projectileDamageCollider.physicalDamage = 100;
        projectileDamageCollider.characterShootingProjectile = player;

        //1. LOCKED ONTO A TARGET
        if (player.playerNetWorkManager.isAiming.Value)
        {
            projectileGameObject.transform.LookAt(new Vector3(xPosition, yPosition, zPosition));
        }
        else
        {
            //2. UNLOCKED AND NOT AIMING

            if (player.playerCombatManager.currentTarget != null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.lockOnTranfrom.position
                    - projectileGameObject.transform.position);
                projectileGameObject.transform.rotation = arrowRotation;
            }
            //3. AMING AND UNLOCKED
            else
            {
                player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, yCharacterRotation , player.transform.rotation.z);
                Quaternion arrowRotation = Quaternion.LookRotation(player.transform.forward);
                projectileGameObject.transform.rotation = arrowRotation;

            }
        }

        //GET ALL CHARACTER COLLIDER AND IGNORE SELF
        Collider[] characterCollider = player.GetComponentsInChildren<Collider>();
        List<Collider> colliderArrowWillIgnore = new List<Collider>();

        foreach (var item in characterCollider)
            colliderArrowWillIgnore.Add(item);

        foreach (Collider hitBox in colliderArrowWillIgnore)
            Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);

        projectileRigibody.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
        projectileGameObject.transform.parent = null;

    }

    [ServerRpc]
    public void HideWeaponsServerRpc()
    {
        if (IsServer)
            HideWeaponsClientRpc();

    }

    [ClientRpc]
    private void HideWeaponsClientRpc()
    {
        if(player.playerEquipmentManager.rightHandWeaponModel != null)
            player.playerEquipmentManager.rightHandWeaponModel.SetActive(false);


        if (player.playerEquipmentManager.leftHandWeaponModel != null)
            player.playerEquipmentManager.leftHandWeaponModel.SetActive(false);
    }

    [ServerRpc]
    public void NotifyServerOfQuickSlotItemActionServerRpc(ulong clientID,  int quickSlotItemID)
    {
        NotifyServerOfQuickSlotItemActionClientRpc(clientID, quickSlotItemID);
    }

    [ClientRpc]
    private void NotifyServerOfQuickSlotItemActionClientRpc(ulong clientID, int quickSlotItemID)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            QuickSlotItem item = WorldItemDatabase.instance.GetQuickSlotItemByID(quickSlotItemID);
            item.AttemptToUseItem(player);
        }
    }

}
