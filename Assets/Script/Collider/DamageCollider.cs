using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Coliider")]
    public Collider damageCollider;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightingDamage = 0;
    public float holyDamage = 0;

    [Header("Poise")]
    public float poiseDamage = 0;

    [Header("Contact Point")]
    protected Vector3 contactPoint;

    [Header("Character Damaged")]
    protected List<CharacterManager> characterDamage = new List<CharacterManager>();

    [Header("Block")]
    protected Vector3 directionFromAttackToDamageTarget;
    protected float dotValueFromAttackToDamageTarget;

    protected virtual void Awake()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

      
        if(damageTarget != null)
        {

            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            CheckForBlock(damageTarget);


            CheckForParry(damageTarget);

            if (!damageTarget.characterNetWorkManager.isInvulnerable.Value)
                DamageTarget(damageTarget);
        }
    }

    protected virtual void  CheckForBlock(CharacterManager damageTarget)
    {
        if (characterDamage.Contains(damageTarget))
            return;


        GetBlockingDotValues(damageTarget);

        if(damageTarget.characterNetWorkManager.isBlocking.Value && dotValueFromAttackToDamageTarget > 0.3f)
        {
            characterDamage.Add(damageTarget);
            TakeBlockDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeBlockDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.staminaDamage = poiseDamage;

            damageTarget.characterEffectManager.ProcessInstantEffects(damageEffect);

        }
    }

    protected virtual void CheckForParry(CharacterManager damageTarget)
    {

    }

    protected virtual void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = transform.position - damageTarget.transform.position;
        dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
        //if (characterDamage.Contains(damageTarget))
        //    return;

        characterDamage.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.poiseDamage = poiseDamage;
        damageEffect.contactPoint = contactPoint;

        damageTarget.characterEffectManager.ProcessInstantEffects(damageEffect);
    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true; 
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        characterDamage.Clear();

    } 
}
    