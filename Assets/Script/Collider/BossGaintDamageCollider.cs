using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGaintDamageCollider : DamageCollider
{
    [SerializeField] AIBossCharacterManager bossCharacter;

    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        bossCharacter = GetComponentInParent<AIBossCharacterManager>();
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
        damageEffect.contactPoint = contactPoint;
        damageEffect.poiseDamage = poiseDamage;

        damageEffect.angleHitFrom = Vector3.SignedAngle(bossCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);



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
                bossCharacter.NetworkObjectId,
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
}
