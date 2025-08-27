using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Xml.Serialization;

public class CharacterNetWorkManager : NetworkBehaviour
{
    CharacterManager character;


    [Header("Active")]
    public NetworkVariable<bool> isActive = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Vector3 networkPositionVelocity;
    public float networkPositionSmoothTime = .1f;
    public float networkRotationSmoothTime = .1f;



    [Header("Animator")]
    public NetworkVariable<bool> isMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Target")]
    public NetworkVariable<ulong> currentTargetNetWorkObjectId = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Flags")]
    public NetworkVariable<bool> isBlocking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isParrying = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isParryable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isAttacking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isInvulnerable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isLockedOn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isChargingAttack = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isRipostable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isBeingCriticallyDamaged = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Resource")]
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxHealth = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> currentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentFocusPoints = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxFocusPoints = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Stats")]
    public NetworkVariable<int> vigor = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> mind = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> strength = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> dexterity = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> intelligence = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> faith = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Stats")]
    public NetworkVariable<int> strengthModifier = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void OnHPChanged(int oldValue, int newValue)
    {
        if (currentHealth.Value <= 0)
        {
            StartCoroutine(character.ProcessDeathEvent());
        }
        if (character.IsOwner)
        {
            if (currentHealth.Value > maxHealth.Value)
            {
                currentHealth.Value = maxHealth.Value;
            }
        }
    }

    public virtual void OnIsDeadChanged(bool oldStatus, bool newStatus)
    {
        character.animator.SetBool("isDead", character.isDead.Value);
    }

    public void OnLockOnTargetIDChange(ulong oldID, ulong newID)
    {
        if (!IsOwner)
        {
            character.characterCombatManager.currentTarget = NetworkManager.Singleton.SpawnManager.SpawnedObjects[newID].gameObject.GetComponent<CharacterManager>();
        }
    }

    public void OnIsLockOnChanged(bool old, bool isLocked)
    {
        if (!isLocked)
        {
            character.characterCombatManager.currentTarget = null;
        }
    }

    public void OnIsChargingAttackChanged(bool oldStatus, bool newStatus)
    {
        character.animator.SetBool("isChargingAttack", isChargingAttack.Value);
    }

    public void OnIsMovingChanged(bool oldStatus, bool newStatus)
    {
        character.animator.SetBool("isMoving", isMoving.Value);
    }

    public virtual void OnIsActiveChanged(bool oldStatus, bool newStatus)
    {
        gameObject.SetActive(isActive.Value);
    }

    public virtual void OnIsBlockingChanged(bool oldStatus, bool newStatus)
    {
        character.animator.SetBool("isBlocking", isBlocking.Value);

    }

    [ServerRpc]
    public void DestroyAllCurrentActionFXServerRpc()
    {
        if (IsServer)
        {
            DestroyAllCurrentActionFXClientRpc();
        }
    }

    [ClientRpc]
    public virtual void DestroyAllCurrentActionFXClientRpc()
    {
        if (character.characterEffectManager.activeSpellWarnUpFX != null)
            Destroy(character.characterEffectManager.activeSpellWarnUpFX);

        if(character.characterEffectManager.activeDrawProjectileFX != null)
            Destroy(character.characterEffectManager.activeDrawProjectileFX);


        if (character.characterEffectManager.activeQuickSlotItemFX != null)
            Destroy(character.characterEffectManager.activeQuickSlotItemFX);

    }

    // SERVER RPC

    [ServerRpc(RequireOwnership = false)]
    public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID,bool applyRootMotion)
    {
        if(IsServer)
        {
            PlayActionAnimationForAllClientsClientRpc(clientID,animationID,applyRootMotion); 
        }
    }

    [ClientRpc]
    public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
    {
        if(clientID !=NetworkManager.Singleton.LocalClientId)
        {
            PerformActionAnimationFromServer(animationID, applyRootMotion);
        }
    }

    private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
    {
        character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(animationID, 0.2f);
    }

    //

    [ServerRpc(RequireOwnership = false)]
    public void NotifyTheServerOfInstantActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
    {
        if (IsServer)
        {
            PlayInstantActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayInstantActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformInstantActionAnimationFromServer(animationID, applyRootMotion);
        }
    }

    private void PerformInstantActionAnimationFromServer(string animationID, bool applyRootMotion)
    {
        character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        character.animator.Play(animationID);
    }

    // ATTACK ANIMATION

    [ServerRpc]
    public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
    {
        if (IsServer)
        {
            PlayAttackActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayAttackActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformAttackActionAnimationFromServer(animationID, applyRootMotion);
        }
    }

    private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion)
    {
        character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(animationID, 0.2f);
    }

    // DAMAGE
    [ServerRpc(RequireOwnership = false)]
    public void NotifyTheServerOfCharacterDamageServerRpc(
        ulong damagedCharacter,
        ulong characterCausingDamage,
        float physicalDamage,
        float magicDamage,
        float fireDamage,
        float holyDamage,
        float poiseDamage,
        float angleHitFrom,
        float contactPointX,
        float contactPointY,
        float contactPointZ)
    {
        if(IsServer)
        {
            NotifyTheServerOfCharacterDamageClientRpc(damagedCharacter, characterCausingDamage, physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);
        }
    }

    [ClientRpc]
    public void NotifyTheServerOfCharacterDamageClientRpc(
          ulong damagedCharacterID,
          ulong characterCausingDamageID,
          float physicalDamage,
          float magicDamage,
          float fireDamage,
          float holyDamage,
          float poiseDamage,
          float angleHitFrom,
          float contactPointX,
          float contactPointY,
          float contactPointZ)
    {
        ProcessCharacterDamageFromServer(damagedCharacterID, characterCausingDamageID, physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);
    }

    public void ProcessCharacterDamageFromServer(
         ulong damagedCharacterID,
         ulong characterCausingDamageID,
         float physicalDamage,
         float magicDamage,
         float fireDamage,
         float holyDamage,
         float poiseDamage,
         float angleHitFrom,
         float contactPointX,
         float contactPointY,
         float contactPointZ)
    {
        CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
        CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);

        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage; 
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.poiseDamage = poiseDamage;
        damageEffect.angleHitFrom = angleHitFrom;
        damageEffect.contactPoint = new Vector3(contactPointX , contactPointY , contactPointZ);
        damageEffect.characterCausingDamage = characterCausingDamage; 

        damagedCharacter.characterEffectManager.ProcessInstantEffects(damageEffect);

    }


    //CRITICAL ATTACK

    [ServerRpc(RequireOwnership = false)]
    public void NotifyTheServerOfRiposteServerRpc(
     ulong damagedCharacter,
     ulong characterCausingDamage,
     string criticalDamageAnimation,
     int weaponID,
     float physicalDamage,
     float magicDamage,
     float fireDamage,
     float holyDamage,
     float poiseDamage)
    {
        if (IsServer)
        {
            NotifyTheServerOfRiposteClientRpc(damagedCharacter, characterCausingDamage, criticalDamageAnimation , weaponID, physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage);
        }
    }

    [ClientRpc]
    public void NotifyTheServerOfRiposteClientRpc(
         ulong damagedCharacter,
         ulong characterCausingDamage,
         string criticalDamageAnimation,
         int weaponID,
         float physicalDamage,
         float magicDamage,
         float fireDamage,
         float holyDamage,
         float poiseDamage)
    {
        ProcessRiposteFromServer(damagedCharacter, characterCausingDamage, criticalDamageAnimation,weaponID,physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage);
    }

    public void ProcessRiposteFromServer(
         ulong damagedCharacterID,
         ulong characterCausingDamageID,
         string criticalDamageAnimation,
         int weaponID,
         float physicalDamage,
         float magicDamage,
         float fireDamage,
         float holyDamage,
         float poiseDamage)
    {
        CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
        CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        TakeCriticalDamageEffects damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffects);

        if (damagedCharacter.IsOwner)
            damagedCharacter.characterNetWorkManager.isBeingCriticallyDamaged.Value = true;

        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.poiseDamage = poiseDamage;
        damageEffect.characterCausingDamage = characterCausingDamage;

        damagedCharacter.characterEffectManager.ProcessInstantEffects(damageEffect);

        if(damagedCharacter.IsOwner)
           damagedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly(criticalDamageAnimation, true);





        StartCoroutine(damagedCharacter.characterCombatManager.ForceMoveEnemyCharacterToRipostePosition
            (characterCausingDamage, WorldUtilityManager.instance.GetRipostingPositionBasedOnWeaponClass(weapon.weaponClass)));

    }


    // BACKSTAB

    [ServerRpc(RequireOwnership = false)]
    public void NotifyTheServerOfBackstabServerRpc(
          ulong damagedCharacter,
          ulong characterCausingDamage,
          string criticalDamageAnimation,
          int weaponID,
          float physicalDamage,
          float magicDamage,
          float fireDamage,
          float holyDamage,
          float poiseDamage)
    {
        if (IsServer)
        {
            NotifyTheServerOfBackstabClientRpc(damagedCharacter, characterCausingDamage, criticalDamageAnimation, weaponID, physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage);
        }
    }

    [ClientRpc]
    public void NotifyTheServerOfBackstabClientRpc(
         ulong damagedCharacter,
         ulong characterCausingDamage,
         string criticalDamageAnimation,
         int weaponID,
         float physicalDamage,
         float magicDamage,
         float fireDamage,
         float holyDamage,
         float poiseDamage)
    {
        ProcessBackstabFromServer(damagedCharacter, characterCausingDamage, criticalDamageAnimation, weaponID, physicalDamage, magicDamage, fireDamage, holyDamage, poiseDamage);
    }

    public void ProcessBackstabFromServer(
         ulong damagedCharacterID,
         ulong characterCausingDamageID,
         string criticalDamageAnimation,
         int weaponID,
         float physicalDamage,
         float magicDamage,
         float fireDamage,
         float holyDamage,
         float poiseDamage)
    {
        CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
        CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        TakeCriticalDamageEffects damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffects);

        if (damagedCharacter.IsOwner)
            damagedCharacter.characterNetWorkManager.isBeingCriticallyDamaged.Value = true;

        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.poiseDamage = poiseDamage;
        damageEffect.characterCausingDamage = characterCausingDamage;

        damagedCharacter.characterEffectManager.ProcessInstantEffects(damageEffect);
        damagedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly(criticalDamageAnimation, true);




        StartCoroutine(characterCausingDamage.characterCombatManager.ForceMoveEnemyCharacterToBackstabPosition
            (damagedCharacter, WorldUtilityManager.instance.GetBackstabPositionBasedOnWeaponClass(weapon.weaponClass)));

    }

    //PARRY
    [ServerRpc(RequireOwnership = false)]
    public void NotifyTheServerOfParryServerRpc(ulong parriedClientID)
    {
        if (IsServer)
        {
            NotifyTheServerOfParryClientRpc(parriedClientID);
        }

    }

    [ClientRpc()]
    protected void NotifyTheServerOfParryClientRpc(ulong parriedClientID)
    {
        ProcessParryFromServer(parriedClientID);
    }

    protected void ProcessParryFromServer(ulong parriedClientID)
    {
        CharacterManager parriedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[parriedClientID].gameObject.GetComponent<CharacterManager>();

        if (parriedCharacter == null)
            return;

        if (parriedCharacter.IsOwner)
        {
            parriedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parried_01", true);

        }
    }
}
