using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadHandDamageCollider : DamageCollider
{
    [SerializeField] AICharacterManager undeadCharacter;

    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        undeadCharacter = GetComponentInParent<AICharacterManager>();
    }


    protected override void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = undeadCharacter.transform.position - damageTarget.transform.position;
        dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
    }
    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if (characterDamage.Contains(damageTarget))
            return;

        characterDamage.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.poiseDamage = poiseDamage;

        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(undeadCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);

       

        //if (undeadCharacter.IsOwner)
        //{
        //    //SEND DAMAGE REQUEST TO THE SERVER
        //    damageTarget.characterNetWorkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId,
        //        undeadCharacter.NetworkObjectId,
        //        damageEffect.physicalDamage,
        //        damageEffect.magicDamage,
        //        damageEffect.fireDamage,
        //        damageEffect.holyDamage,
        //        damageEffect.poiseDamage,
        //        damageEffect.angleHitFrom,
        //        damageEffect.contactPoint.x,
        //        damageEffect.contactPoint.y,
        //        damageEffect.contactPoint.z);
        //}

        if (damageTarget.IsOwner)
        {
            //SEND DAMAGE REQUEST TO THE SERVER
            damageTarget.characterNetWorkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId,
                undeadCharacter.NetworkObjectId,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                damageEffect.poiseDamage,
                damageEffect.angleHitFrom,
                damageEffect.contactPoint.x,
                damageEffect.contactPoint.y,
                damageEffect.contactPoint.z);
        }
    }

    protected override void CheckForParry(CharacterManager damageTarget)
    {
        if (characterDamage.Contains(damageTarget)) return;

        if (!undeadCharacter.characterNetWorkManager.isParryable.Value)
            return;
        if (!damageTarget.IsOwner)
            return;

        if (damageTarget.characterNetWorkManager.isParrying.Value)
        {
            characterDamage.Add(damageTarget);
            damageTarget.characterNetWorkManager.NotifyTheServerOfParryServerRpc(undeadCharacter.NetworkObjectId);
            damageTarget.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parry_Land_01", true);
        }
    }
}
