using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterCombatManager : NetworkBehaviour
{
    protected CharacterManager character;

    [Header("Last Attack Animation Performed")]
    public string lastAttackAnimationPerformed;

    [Header("Previous Poise Damage Taken")]
    public float previousPoiseDamageTaken;

    [Header("Attack Target")]
    public CharacterManager currentTarget;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Lock On Tranfrom")]
    public Transform lockOnTranfrom;

    [Header("Attack Flags")]
    public bool canPerformRollingAttack = false;
    public bool canPerformBackstepAttack = false;
    public bool canBlock = true;
    public bool canBeBackstabbed = true;

    [Header("Critical Attack")]
    private Transform riposteReceiverTramform;
    private Transform backstabReceiverTramform;
    [SerializeField] float criticalAttackDistanceCheck = 1f;
    public int pendingCriticalDamage;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (character.IsOwner)
        {
            if(newTarget != null)
            {
                currentTarget = newTarget;
                character.characterNetWorkManager.currentTargetNetWorkObjectId.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
            }
            else
            {
                currentTarget = null;
            }
        }
    }

    public virtual void AttemptCriticalAttack()
    {
        if(character.isPerformingAction) 
            return;

        if (character.characterNetWorkManager.currentStamina.Value <= 0)
            return;

        RaycastHit[] hits = Physics.RaycastAll(character.characterCombatManager.lockOnTranfrom.position,
            character.transform.TransformDirection(Vector3.forward), criticalAttackDistanceCheck, WorldUtilityManager.instance.GetCharacterLayer());

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i]; 

            CharacterManager targetCharacter = hit.transform.GetComponent<CharacterManager>();

            if (targetCharacter != null)
            {
                if(targetCharacter == character)
                    continue;

                if(!WorldUtilityManager.instance.CanIDamageThisTarget(character.characterGroup , targetCharacter.characterGroup))
                    continue;

                Vector3 directionFromCharacterToTarget  = character.transform.position - targetCharacter.transform.position;

                float targetViewableAngle = Vector3.SignedAngle(directionFromCharacterToTarget, targetCharacter.transform.forward, Vector3.up);

                if (targetCharacter.characterNetWorkManager.isRipostable.Value)
                {
                    if (targetViewableAngle >= -60 && targetViewableAngle <= 60)
                    {
                        AttemptRiposte(hit);
                        return;
                    }
                }

                if (targetCharacter.characterCombatManager.canBeBackstabbed)
                {
                    if (targetViewableAngle <= 180 && targetViewableAngle >= 145)
                    {
                        AttemptBackstab(hit);
                        return;
                    }

                    if (targetViewableAngle >= -180 && targetViewableAngle <= -145)
                    {
                        AttemptBackstab(hit);
                        return;
                    }
                }
            }
        }
    }

    public virtual void AttemptRiposte(RaycastHit hit)
    {
        

    }

    public virtual void AttemptBackstab(RaycastHit hit)
    {


    } 

    public virtual void ApplyCriticalDamage()
    {
        character.characterEffectManager.PlayCriticalBloodSplatterVFX(character.characterCombatManager.lockOnTranfrom.position);
        character.characterSFXManager.PlayCriticalSFX();

        if (character.IsOwner)
            character.characterNetWorkManager.currentHealth.Value -= pendingCriticalDamage;
    }

    public IEnumerator ForceMoveEnemyCharacterToRipostePosition(CharacterManager enemycharacter, Vector3 ripostePosition)
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;

            if (riposteReceiverTramform == null)
            {
                GameObject riposteTranformObject = new GameObject("Riposte Tranform");
                riposteTranformObject.transform.parent = transform;
                riposteTranformObject.transform.position = Vector3.zero;
                riposteReceiverTramform = riposteTranformObject.transform;
            }

            riposteReceiverTramform.localPosition = ripostePosition;
            enemycharacter.transform.position = riposteReceiverTramform.position;
            transform.rotation = Quaternion.LookRotation(-enemycharacter.transform.forward);
            yield return null;
        }

    }

    public IEnumerator ForceMoveEnemyCharacterToBackstabPosition(CharacterManager enemycharacter, Vector3 backstabPosition)
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;

            if (backstabReceiverTramform == null)
            {
                GameObject backstabTranformObject = new GameObject("Backstab Tranform");
                backstabTranformObject.transform.parent = transform;
                backstabTranformObject.transform.position = Vector3.zero;
                backstabReceiverTramform = backstabTranformObject.transform;
            }

            backstabReceiverTramform.localPosition = backstabPosition;
            enemycharacter.transform.position = backstabReceiverTramform.position;
            transform.rotation = Quaternion.LookRotation(enemycharacter.transform.forward);
            yield return null;
        }

    }

    public void EnableIsInvulnerable()
    {
        if(character.IsOwner) 
            character.characterNetWorkManager.isInvulnerable.Value = true;
    }

    public void DisableIsInvulnerable()
    {
        if (character.IsOwner)
            character.characterNetWorkManager.isInvulnerable.Value = false;
    }

    public void EnableParrying()
    {
        if (character.IsOwner)
            character.characterNetWorkManager.isParrying.Value = true;
    }

    public void DisableParrying()
    {
        if (character.IsOwner)
            character.characterNetWorkManager.isParrying.Value = false;
    }

    public void EnableIsRipostable()
    {
        if(character.IsOwner)
            character.characterNetWorkManager.isRipostable.Value = true;
    }

    //ROLLING
    public void EnableCanDoRollingAttack()
    {
        canPerformRollingAttack = true;
    }

    public void DisableCanDoRollingAttack()
    {
        canPerformRollingAttack = false;
    }


    //BACK STEP
    public void EnableCanDoBackstepAttack()
    {
        canPerformBackstepAttack = true;
    }

    public void DisableCanDoBackstepAttack()
    {
        canPerformBackstepAttack = false;
    }

    public virtual void EnableCanDoCombo()
    {

    }

    public virtual void DisableCanDoCombo()
    {

    }


    public virtual void CloseAllDamageColliders()
    {

    }

    public void DestroyAllCurrentActionFX()
    {
        character.characterNetWorkManager.DestroyAllCurrentActionFXServerRpc();
    }

}
