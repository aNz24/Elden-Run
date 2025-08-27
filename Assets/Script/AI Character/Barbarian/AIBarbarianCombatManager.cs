using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBarbarianCombatManager : AICharacterCombatManager
{
    AIBarbarianCharacterManager barbarianManager;

    [Header("Damage Colliders")]
    [SerializeField] BossGaintDamageCollider rightHandDamageCollider;
    [SerializeField] BossGaintDamageCollider leftHandDamageCollider;
    [SerializeField] Transform stompingFoot;


    [Header("Damage")]
    [SerializeField] int baseDamage = 25;   
    [SerializeField] int basePoiseDamage = 25;   
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;
    [SerializeField] float attack03DamageModifier = 1.6f;
    [SerializeField] float attack04DamageModifier = 1.8f;
    [SerializeField] float stompDamage = 25;
    [SerializeField] float stompAttackAOERadius = 1.5f;

    protected override void Awake()
    {
        base.Awake();

        barbarianManager = GetComponent<AIBarbarianCharacterManager>();
    }

    public void SetAttack01Damage()
    {
        aiCharacter.characterSFXManager.PlayAttackGrunt();

        rightHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        rightHandDamageCollider.poiseDamage = basePoiseDamage * attack01DamageModifier;

        leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * attack01DamageModifier;

    }
    public void SetAttack02Damage()
    {
        aiCharacter.characterSFXManager.PlayAttackGrunt();
        rightHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        rightHandDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;

        leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * attack02DamageModifier;
    }

    public void SetAttack03Damage()
    {

        aiCharacter.characterSFXManager.PlayAttackGrunt();
        rightHandDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
        rightHandDamageCollider.poiseDamage = basePoiseDamage * attack03DamageModifier;

        leftHandDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * attack03DamageModifier;
    }

    public void SetAttack04Damage()
    {

        aiCharacter.characterSFXManager.PlayAttackGrunt();
        rightHandDamageCollider.physicalDamage = baseDamage * attack04DamageModifier;
        rightHandDamageCollider.poiseDamage = basePoiseDamage * attack04DamageModifier;

        leftHandDamageCollider.physicalDamage = baseDamage * attack04DamageModifier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * attack04DamageModifier;
    }


    public void OpenRightHandDamageColliders()
    {
        rightHandDamageCollider.EnableDamageCollider();
        barbarianManager.characterSFXManager.PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(barbarianManager.barbariansoundFXManager.whooshes));

    }

    public void DisableRightHandDamageColliders()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftHandDamageColliders()
    {
        leftHandDamageCollider.EnableDamageCollider();
        barbarianManager.characterSFXManager.PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(barbarianManager.barbariansoundFXManager.whooshes));
    }

    public void DisableLeftHandDamageColliders()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }

    public void ActiveStomp()
    {
        Collider[] colliders = Physics.OverlapSphere(stompingFoot.position, stompAttackAOERadius , WorldUtilityManager.instance.GetCharacterLayer());
        List<CharacterManager> charactersDamaged = new List<CharacterManager>();
        foreach (var collider in colliders)
        {
            CharacterManager character = collider.GetComponentInParent<CharacterManager>();

            if(character != null)
            {
                if (charactersDamaged.Contains(character))
                    continue;

                charactersDamaged.Add(character);

                if (character.IsOwner)
                {

                    TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                    damageEffect.physicalDamage = stompDamage;
                    damageEffect.poiseDamage = stompDamage;

                    character.characterEffectManager.ProcessInstantEffects(damageEffect);
                }
            }
           
        }
    }

    public override void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return;

        if (viewableAngle >= 20 && viewableAngle <= 90)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right90", true);
        }
        else if (viewableAngle <= -20 && viewableAngle >= -90)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left90", true);
        }
        else if (viewableAngle > 90 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right180", true);
        }
        else if (viewableAngle < -90 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left180", true);
        }
    }

}
