using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectileDamageCollider : DamageCollider
{
    [Header("Marksmen")]
    public CharacterManager characterShootingProjectile;

    [Header("Collison")]
    private bool hasPenetratedSurface = false;
    public Rigidbody _rigidbody;
    private CapsuleCollider capsuleCollider;


    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        if (_rigidbody.linearVelocity != Vector3.zero)
        {
            _rigidbody.rotation = Quaternion.LookRotation(_rigidbody.linearVelocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreatePenetrationIntoObject(collision);

        WorldSFXManger.instance.AlertNearbyCharacterToSound(transform.position, 3);


        CharacterManager potentialTarget = collision.transform.gameObject.GetComponent<CharacterManager>();

        if (characterShootingProjectile == null)
            return;

        Collider contactCollider = collision.gameObject.GetComponent<Collider>();

        if(contactCollider != null) 
            contactPoint = contactCollider.ClosestPoint(transform.position);

        if (potentialTarget == null)
            return;

        if (WorldUtilityManager.instance.CanIDamageThisTarget(characterShootingProjectile.characterGroup, potentialTarget.characterGroup))
        {
            CheckForBlock(potentialTarget);
            DamageTarget(potentialTarget);
        }

    }

    protected override void CheckForBlock(CharacterManager damageTarget)
    {
        if (characterDamage.Contains(damageTarget))
            return;

        float angle = Vector3.Angle(damageTarget.transform.forward, transform.forward);

        if (damageTarget.characterNetWorkManager.isBlocking.Value && angle > 145)
        {
            characterDamage.Add(damageTarget);
            TakeBlockDamageEffect blockDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeBlockDamageEffect);

            if(characterShootingProjectile != null)
                blockDamageEffect.characterCausingDamage = characterShootingProjectile;


            blockDamageEffect.physicalDamage = physicalDamage;
            blockDamageEffect.magicDamage = magicDamage;
            blockDamageEffect.fireDamage = fireDamage;
            blockDamageEffect.holyDamage = holyDamage;
            blockDamageEffect.contactPoint = contactPoint;
            blockDamageEffect.poiseDamage = poiseDamage;
            blockDamageEffect.staminaDamage = poiseDamage;

            damageTarget.characterEffectManager.ProcessInstantEffects(blockDamageEffect);
        }
    }

    private void CreatePenetrationIntoObject(Collision hit)
    {
        if (!hasPenetratedSurface)
        {
            hasPenetratedSurface = true;

            gameObject.transform.position = hit.GetContact(0).point;

            var emptyObject = new GameObject();
            emptyObject.transform.parent = hit.collider.transform;
            gameObject.transform.SetParent(emptyObject.transform, true);

            transform.position += transform.forward * (Random.Range(.1f, .3f));
            _rigidbody.isKinematic = true;
            capsuleCollider.enabled = false;

            Destroy(GetComponent<RangedProjectileDamageCollider>());
            Destroy(gameObject,20);
        }
    }
}
