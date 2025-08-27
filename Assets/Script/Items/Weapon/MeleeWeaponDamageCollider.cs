using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage;

    [Header("Weapon Attack Modifiers")]
    public float light_Attack_01_Modifiers;
    public float light_Attack_02_Modifiers;
    public float light_Jump_Attack_01_Modifier;
    public float heavy_Attack_01_Modifiers;
    public float heavy_Attack_02_Modifiers;
    public float heavy_Jump_Attack_01_Modifier;
    public float charge_Attack_01_Modifiers;
    public float charge_Attack_02_Modifiers;
    public float running_Attack_01_Modifiers;
    public float rolling_Attack_01_Modifiers;
    public float backstep_Attack_01_Modifiers;


    protected override void Awake()
    {
        base.Awake();

        if (damageCollider == null)
        {
            damageCollider = GetComponent<Collider>();
        }

        damageCollider.enabled = false; // MELEE WEAPON COLLIDERS SHOULD BE DISABLE AT START, ONLY ENABLE WHEN ANIMATION ALLOW
    }

    protected override void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = characterCausingDamage.transform.position - damageTarget.transform.position;
        dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
    }

    protected override void OnTriggerEnter(Collider other)
    {

        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (damageTarget == characterCausingDamage)
                return;

            if (!WorldUtilityManager.instance.CanIDamageThisTarget(characterCausingDamage.characterGroup, damageTarget.characterGroup))
                return;

            CheckForParry(damageTarget);


            CheckForBlock(damageTarget);

            if(!damageTarget.characterNetWorkManager.isInvulnerable.Value)
                DamageTarget(damageTarget);
        }
    }

    protected override void CheckForParry(CharacterManager damageTarget)
    {
        if(characterDamage.Contains(damageTarget)) return;

        if (!characterCausingDamage.characterNetWorkManager.isParryable.Value)
            return;
        if (!damageTarget.IsOwner)
            return;

        if (damageTarget.characterNetWorkManager.isParrying.Value)
        {
            characterDamage.Add(damageTarget);
            damageTarget.characterNetWorkManager.NotifyTheServerOfParryServerRpc(characterCausingDamage.NetworkObjectId);
            damageTarget.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parry_Land_01", true);
        }

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
        damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

 

        switch (characterCausingDamage.characterCombatManager.currentAttackType)
        {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifiers(light_Attack_01_Modifiers, damageEffect);
                break;
            case AttackType.LightAttack02:
                ApplyAttackDamageModifiers(light_Attack_02_Modifiers, damageEffect);
                break;
            case AttackType.LightJumpingAttack01:
                ApplyAttackDamageModifiers(light_Jump_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.HeavyAttack01:
                ApplyAttackDamageModifiers(heavy_Attack_01_Modifiers, damageEffect);
                break;
            case AttackType.HeavyAttack02:
                ApplyAttackDamageModifiers(heavy_Attack_02_Modifiers, damageEffect);
                break;
            case AttackType.HeavyJumpingAttack01:
                ApplyAttackDamageModifiers(heavy_Jump_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.ChargedAttack01:
                ApplyAttackDamageModifiers(charge_Attack_01_Modifiers, damageEffect);
                break;
            case AttackType.ChargedAttack02:
                ApplyAttackDamageModifiers(charge_Attack_01_Modifiers, damageEffect);
                break;
            case AttackType.RunningAttack01:
                ApplyAttackDamageModifiers(rolling_Attack_01_Modifiers, damageEffect);
                break;
            case AttackType.RollingAttack01:
                ApplyAttackDamageModifiers(rolling_Attack_01_Modifiers, damageEffect);
                break;
            case AttackType.BackstepAttack01:
                ApplyAttackDamageModifiers(backstep_Attack_01_Modifiers, damageEffect);
                break;
            default:
                break;
        }






        if (characterCausingDamage.IsOwner)
        {
            //SEND DAMAGE REQUEST TO THE SERVER
            damageTarget.characterNetWorkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId,
                characterCausingDamage.NetworkObjectId,
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

    private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
    {
        damage.physicalDamage *= modifier;
        damage.magicDamage *= modifier;
        damage.fireDamage *= modifier;
        damage.holyDamage *= modifier;
        damage.poiseDamage *= modifier;

    }
}
