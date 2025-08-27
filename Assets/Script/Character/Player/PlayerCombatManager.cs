using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;

    public WeaponItem currentWeaponBeingUsed;
    public ProjectileSlot currentProjectileBeingUsed;

    [Header("Flags")]
    public bool canComboWithMainHandWeapon = false;
    //public bool canComboWithOffHandWeapon = false; 
    public bool isUsingItem= false;

    [Header("Projectile")]
    private Vector3 projectileAimDirection;

    [Header("Ranged Aim")]
    [SerializeField] Transform followTransformWhenAiming;

    protected override void Awake()
    {
        base.Awake(); 
        
        player = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene arg0, Scene arg1)
    {

        // DEAD SPOT
        if ( WorldSaveGameManager.instance.currentCharacterData.hasDeadSpot)
        {
            Vector3 deadSpotPosition = new Vector3(WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionX, 
                WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionY
               ,WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionZ);
            CreateDeadSpot(deadSpotPosition, WorldSaveGameManager.instance.currentCharacterData.deadPostRunesCount, false);
        }
    }

    public void CreateDeadSpot(Vector3 position , int runesCount, bool removePlayerRunes = true)
    {
        if (!player.IsHost)
            return;

        GameObject deadSpotFX = Instantiate(WorldCharacterEffectsManager.instance.deadSpotVFX);
        deadSpotFX.GetComponent<NetworkObject>().Spawn();

        deadSpotFX.transform.position = position;

        PickUpRunesInteractable pickUpRunes = deadSpotFX.GetComponent<PickUpRunesInteractable>();
        pickUpRunes.runesCount = runesCount;

        if(removePlayerRunes)
            player.playerStatManager.AddRunes(-player.playerStatManager.runes);

        WorldSaveGameManager.instance.currentCharacterData.hasDeadSpot = true;
        WorldSaveGameManager.instance.currentCharacterData.deadPostRunesCount = pickUpRunes.runesCount;
        WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionX = position.x;
        WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionY = position.y;
        WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionZ = position.z;
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction,WeaponItem weaponPerformingAction)
    {
        // PERFORM THE ACTION
        weaponAction.AttempToPerformAction(player, weaponPerformingAction);

    }

    public override void CloseAllDamageColliders()
    {
        base.CloseAllDamageColliders();

        player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
    }

    //CRITICAL ATTACKS
    public override void AttemptRiposte(RaycastHit hit)
    {
        CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

        if (targetCharacter == null)
            return;

        if (!targetCharacter.characterNetWorkManager.isRipostable.Value)
            return;

        if (targetCharacter.characterNetWorkManager.isBeingCriticallyDamaged.Value)
            return;

        MeleeWeaponItem riposteWeapon;
        MeleeWeaponDamageCollider riposteCollider;

        if (player.playerNetWorkManager.isTwoHandingLeftWeapon.Value)
        {
            riposteWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
            riposteCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
        }
        else
        {
            riposteWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
            riposteCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
        }

        character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Riposte_01" , true); 

        if(character.IsOwner)
            character.characterNetWorkManager.isInvulnerable.Value = true;

        TakeCriticalDamageEffects damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffects);

        damageEffect.physicalDamage = riposteCollider .physicalDamage;
        damageEffect.holyDamage = riposteCollider .holyDamage;
        damageEffect.fireDamage = riposteCollider .fireDamage;
        damageEffect.lightingDamage = riposteCollider .lightingDamage;
        damageEffect.magicDamage = riposteCollider .magicDamage;   
        damageEffect.poiseDamage = riposteCollider .poiseDamage;
        
        damageEffect.physicalDamage *= riposteWeapon.riposte_Attack_01_Modifiers;
        damageEffect.holyDamage *= riposteWeapon.riposte_Attack_01_Modifiers;
        damageEffect.fireDamage *= riposteWeapon.riposte_Attack_01_Modifiers;
        damageEffect.lightingDamage *= riposteWeapon.riposte_Attack_01_Modifiers;
        damageEffect.magicDamage *= riposteWeapon.riposte_Attack_01_Modifiers;
        damageEffect.poiseDamage *= riposteWeapon.riposte_Attack_01_Modifiers;

        targetCharacter.characterNetWorkManager.NotifyTheServerOfRiposteServerRpc(
            targetCharacter.NetworkObjectId,
            character.NetworkObjectId,
            "Riposted_01",
            riposteWeapon.itemID,
            damageEffect.physicalDamage,
            damageEffect.magicDamage,
            damageEffect.fireDamage,
            damageEffect.holyDamage,
            damageEffect.poiseDamage);
    }

    public override void AttemptBackstab(RaycastHit hit)
    {
        CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

        if (targetCharacter == null)
            return;

        if (!targetCharacter.characterCombatManager.canBeBackstabbed)
            return;

        if (targetCharacter.characterNetWorkManager.isBeingCriticallyDamaged.Value)
            return;

        MeleeWeaponItem backstabWeapon;
        MeleeWeaponDamageCollider backstabCollider;

        if (player.playerNetWorkManager.isTwoHandingLeftWeapon.Value)
        {
            backstabWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
            backstabCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
        }
        else
        {
            backstabWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
            backstabCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
        }


        character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Backstab_01", true);

        if (character.IsOwner)
            character.characterNetWorkManager.isInvulnerable.Value = true;

        TakeCriticalDamageEffects damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffects);

        damageEffect.physicalDamage = backstabCollider.physicalDamage;
        damageEffect.holyDamage = backstabCollider.holyDamage;
        damageEffect.fireDamage = backstabCollider.fireDamage;
        damageEffect.lightingDamage = backstabCollider.lightingDamage;
        damageEffect.magicDamage = backstabCollider.magicDamage;
        damageEffect.poiseDamage = backstabCollider.poiseDamage;

        damageEffect.physicalDamage *= backstabWeapon.backstab_Attack_01_Modifiers;
        damageEffect.holyDamage *= backstabWeapon.backstab_Attack_01_Modifiers;
        damageEffect.fireDamage *= backstabWeapon.backstab_Attack_01_Modifiers;
        damageEffect.lightingDamage *= backstabWeapon.backstab_Attack_01_Modifiers;
        damageEffect.magicDamage *= backstabWeapon.backstab_Attack_01_Modifiers;
        damageEffect.poiseDamage *= backstabWeapon.backstab_Attack_01_Modifiers;

        targetCharacter.characterNetWorkManager.NotifyTheServerOfBackstabServerRpc(
            targetCharacter.NetworkObjectId,
            character.NetworkObjectId,
            "Backstabbed_01",
            backstabWeapon.itemID,
            damageEffect.physicalDamage,
            damageEffect.magicDamage,
            damageEffect.fireDamage,
            damageEffect.holyDamage,
            damageEffect.poiseDamage);
    }

    public virtual void DrainStaminaBasedOnAttack()
    {
        if(!player.IsOwner)
            return;

        if(currentWeaponBeingUsed == null)
            return;

        float staminaDeducted = 0f;



        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.LightAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.LightJumpingAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.HeavyAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                break;
            case AttackType.HeavyAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                break;
            case AttackType.HeavyJumpingAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                break;
            case AttackType.ChargedAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;

                break;
            case AttackType.ChargedAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;

                break;
            case AttackType.RunningAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningAttackStaminaCostMultiplier;

                break;
            case AttackType.RollingAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingAttackStaminaCostMultiplier;

                break;
            case AttackType.BackstepAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepAttackStaminaCostMultiplier;

                break;
            default:
                break;
        }


        player.playerNetWorkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        if (player.IsOwner)
        {
            PlayerCamera.instance.SetLockCameraHeight();
        }
    }

    //COMBO
    public override void EnableCanDoCombo()
    {
        if (player.playerNetWorkManager.isUsingRightHand.Value)
        {
            player.playerCombatManager.canComboWithMainHandWeapon = true;
        }
        else
        {
            //ENABLE OFF HAND COMBO
        }
    }

    public override void DisableCanDoCombo()
    {
        player.playerCombatManager.canComboWithMainHandWeapon = false;
        // player.playerCombatManager.canComboWithOffHandWeapon = false;
    }

    //PROJECTILE
    public void ReleaseArrow()
    {
        if(player.IsOwner)
            player.playerNetWorkManager.hasArrowNotched.Value = false;

        if(player.playerEffectsManager.activeDrawProjectileFX != null) 
            Destroy(player.playerEffectsManager.activeDrawProjectileFX);

        player.characterSFXManager.PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(WorldSFXManger.instance.releaseArrowSFX));

        //Animator bowAnimator;

        //if (player.playerNetWorkManager.isTwoHandingLeftWeapon.Value)
        //{
        //    bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
        //}
        //else
        //{
        //    bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();

        //}

        //bowAnimator.SetBool("isDrawn", false);
        //bowAnimator.Play("Bow_Fire_01");


        if (!player.IsOwner)
            return;

        RangedProjectileItem projectileItem = null;

        switch (currentProjectileBeingUsed)
        {
            case ProjectileSlot.Main:
                projectileItem = player.playerInventoryManager.mainProjectile;
                break;
            case ProjectileSlot.Secondary:
                projectileItem = player.playerInventoryManager.secondaryProjectile;
                break;
            default:
                break;
        }

        if (projectileItem == null)
            return;

        if (projectileItem.currentAmmoAmount <= 0)
            return;

        Transform projectileInstantiationLocation;
        GameObject projectileGameObject;
        Rigidbody projectileRigibody;
        RangedProjectileDamageCollider projectileDamageCollider;

        projectileItem.currentAmmoAmount -= 1;

        switch (currentProjectileBeingUsed)
        {
            case ProjectileSlot.Main:

                PlayerUIManager.instance.playerUIHudManager.SetMainProjectileQuickSlotIcon(projectileItem);
                break;
            case ProjectileSlot.Secondary:
                PlayerUIManager.instance.playerUIHudManager.SetSecondaryProjectileQuickSlotIcon(projectileItem);
                break;
            default:
                break;
        }


        // UPDATE ARROW
        projectileInstantiationLocation = player.playerCombatManager.lockOnTranfrom;
        projectileGameObject = Instantiate(projectileItem.releaseProjectileModel , projectileInstantiationLocation);
        projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
        projectileRigibody = projectileGameObject.GetComponent<Rigidbody>();

        
        projectileDamageCollider.physicalDamage = 100;
        projectileDamageCollider.characterShootingProjectile = player;

        float yRotationDuringFire = player.transform.localEulerAngles.y;

        //LOCKED ONTO A TARGET
        if (player.playerNetWorkManager.isAiming.Value)
        {
            Ray newRay = new Ray(lockOnTranfrom.position, PlayerCamera.instance.aimDirection);
            projectileAimDirection = newRay.GetPoint(5000);
            projectileGameObject.transform.LookAt(projectileAimDirection);
        }
        else
        {
            //UNLOCKED AND NOT AIMING

            if (player.playerCombatManager.currentTarget != null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.lockOnTranfrom.position
                    - projectileGameObject.transform.position);
                projectileGameObject.transform.rotation = arrowRotation;
            }
            // AMING AND UNLOCKED
            else
            {
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

        //SYNC ARROW FIRE WITH THE SERVER
        player.playerNetWorkManager.NotifyTheServerOfReleaseProjectileServerRpc(player.OwnerClientId,
            projectileItem.itemID,
            projectileAimDirection.x,
            projectileAimDirection.y,
            projectileAimDirection.z,
            yRotationDuringFire);
    }


    public void InstantiateSpellWarmUpFX()
    {
        if (player.playerInventoryManager.currentSpell == null)
            return;

        player.playerInventoryManager.currentSpell.InstantiateWarmUpSpellFX(player);
    }

    public void SuccesfullyCastSpell()
    {
        if (player.playerInventoryManager.currentSpell == null)
            return;

        player.playerInventoryManager.currentSpell.SuccesfullyCastSpell(player);
    }

    //QUICK SLOT
    public void SuccessfulyQuickSlotItem()
    {
        if(player.playerInventoryManager.currentQuickSlotItem != null)
            player.playerInventoryManager.currentQuickSlotItem.SuccessfullyUseItem(player);
    }

    public WeaponItem SelectWeaponToPerformAshOfWar()
    {
        WeaponItem selectedWeapon = player.playerInventoryManager.currentLeftHandWeapon;

        player.playerNetWorkManager.SetCharacterActionHand(false);
        player.playerNetWorkManager.currentWeaponBeingUsed.Value = selectedWeapon.itemID;

        return selectedWeapon;
    }

}
