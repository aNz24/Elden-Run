using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallManager : SpellManager
{
    [Header("Collider")]
    public FireBallDamageCollider damageCollider;

    [Header("Instantiated FX")]
    private GameObject instantiatedDestructionFX;

    private bool hasCollided = false;
    private Rigidbody fireBallRigidbody;
    private Coroutine destructionFXCoroutine;


    protected override void Awake()
    {
        base.Awake();

        fireBallRigidbody = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();

        if (spellTarget != null)
            transform.LookAt(spellTarget.transform);

        if (fireBallRigidbody != null)
        {
            Vector3 currentVelocity = fireBallRigidbody.linearVelocity;
            fireBallRigidbody.linearVelocity = transform.forward + currentVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
            return;

        if (!hasCollided)
        {
            hasCollided=true;
            InstantiteSpellDestructionFX();
        }
    }

    public void InitializeFireBall(CharacterManager spellCaster)
    {
        damageCollider.spellCaster = spellCaster;
        damageCollider.fireDamage = 150;
    }

    public void InstantiteSpellDestructionFX()
    {
        instantiatedDestructionFX = Instantiate(impactParticle, transform.position , Quaternion.identity);

        WorldSFXManger.instance.AlertNearbyCharacterToSound(transform.position,15);
        Destroy(gameObject);

    }

    public void WaitThenInstantiateSpellDestructionFX(float timeToWait)
    {
        if (destructionFXCoroutine != null)
            StopCoroutine(destructionFXCoroutine);

        destructionFXCoroutine = StartCoroutine(WaitThenInstantiateFX(timeToWait));
        StartCoroutine(WaitThenInstantiateFX(timeToWait));
    }

    private IEnumerator WaitThenInstantiateFX(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        InstantiteSpellDestructionFX();
    }

}
