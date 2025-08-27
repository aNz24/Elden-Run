using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUndeadCombatManager : AICharacterCombatManager
{

    [Header("Damage Colliders")]
    [SerializeField] UndeadHandDamageCollider rightHandDamageCollider;
    [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;


    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] int basePoiseDamage = 25;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;

    public void SetAttack01Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        rightHandDamageCollider.poiseDamage = basePoiseDamage * attack01DamageModifier;

        leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * attack01DamageModifier;
    }
    public void SetAttack02Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        rightHandDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;

        leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;
    }

    public void OpenRightHandDamageColliders()
    {
        aiCharacter.characterSFXManager.PlayAttackGrunt();
        rightHandDamageCollider.EnableDamageCollider();

    }

    public void DisableRightHandDamageColliders()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftHandDamageColliders()
    {
        aiCharacter.characterSFXManager.PlayAttackGrunt();
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void DisableLeftHandDamageColliders()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }

    public override void CloseAllDamageColliders()
    {
        base.CloseAllDamageColliders();

        rightHandDamageCollider.DisableDamageCollider();
        leftHandDamageCollider.DisableDamageCollider();
    }
}
