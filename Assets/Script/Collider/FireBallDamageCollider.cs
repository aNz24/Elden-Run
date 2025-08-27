using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallDamageCollider : SpellProjectileDamageCollider
{
    public FireBallManager fireBallManager;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {

        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (damageTarget == spellCaster)
                return;

            if (!WorldUtilityManager.instance.CanIDamageThisTarget(spellCaster.characterGroup, damageTarget.characterGroup))
                return;

            CheckForParry(damageTarget);


            CheckForBlock(damageTarget);

            if (!damageTarget.characterNetWorkManager.isInvulnerable.Value)
                DamageTarget(damageTarget);

            fireBallManager.WaitThenInstantiateSpellDestructionFX(.4f);
        }
    }

    protected override void CheckForParry(CharacterManager damageTarget)
    {
 
    }

    protected override void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = spellCaster.transform.position - damageTarget.transform.position;
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
        damageEffect.poiseDamage = poiseDamage;

        damageEffect.holyDamage = holyDamage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(spellCaster.transform.forward, damageTarget.transform.forward, Vector3.up);


        if (spellCaster.IsOwner)
        {
            //SEND DAMAGE REQUEST TO THE SERVER
            damageTarget.characterNetWorkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId,
                spellCaster.NetworkObjectId,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                damageEffect.poiseDamage,
                damageEffect.angleHitFrom,
                damageEffect.contactPoint.x,
                damageEffect.contactPoint.y,
                damageEffect.contactPoint.z
                );
        }
    }
}
