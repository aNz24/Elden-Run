using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Blocked Damage")]
public class TakeBlockDamageEffect : InstantCharacterEffects
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; // If the damage is caused by other character attack it will be stored here

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightingDamage = 0;
    public float holyDamage = 0;

    [Header("Final Damage")]
    private int finalDamageDealt = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = false;
    public AudioClip elementalDamageSoundFX;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    [Header("Stamina")]
    public float staminaDamage = 0;
    public float finalStaminaDamage = 0;


    public override void ProcessEffect(CharacterManager character)
    {
        if (character.characterNetWorkManager.isInvulnerable.Value)
            return;
        base.ProcessEffect(character);

        Debug.Log("HIT THE BLOCKED");

        if (character.isDead.Value)
            return;

        CalculateDamage(character);
        CalculateStaminaDamage(character);
        PlayDirectionalBasedBlockingAnimation(character);
        PlayDamageSFX(character);
        PlayDamageVFX(character);

        CheckForGuardBreak(character);
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (characterCausingDamage != null)
        {

        }

        Debug.Log("ORIGINAL PHYSICAL DAMAGE: " + physicalDamage);


        physicalDamage -= (physicalDamage * (character.characterStatManager.blockingPhysicalAbsorptions / 100));
        magicDamage -= (physicalDamage * (character.characterStatManager.blockingMagicAbsorptions / 100));
        fireDamage -= (physicalDamage * (character.characterStatManager.blockingFireAbsorptions / 100));
        holyDamage -= (physicalDamage * (character.characterStatManager.blockingHolyAbsorptions / 100));
        lightingDamage -= (physicalDamage * (character.characterStatManager.blockingLightingAbsorptions / 100));

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightingDamage + holyDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        Debug.Log("FINAL PHYSICAL DAMAGE: " + physicalDamage);

        character.characterNetWorkManager.currentHealth.Value -= finalDamageDealt;
    }   

    private void CalculateStaminaDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;
        finalStaminaDamage = staminaDamage;

        float staminaDamageAbsorption = finalStaminaDamage * (character.characterStatManager.blockingStability / 100);
        float staminaDamageAfterAbsorption = finalStaminaDamage - staminaDamageAbsorption;

        character.characterNetWorkManager.currentStamina.Value -= staminaDamageAfterAbsorption;
    }

    private void  CheckForGuardBreak(CharacterManager character)
    {
        if (!character.IsOwner)
            return;
        if(character.characterNetWorkManager.currentStamina.Value <= 0)
        {
            character.characterAnimatorManager.PlayTargetActionAnimation("Guard_Brake_01" , true);
            character.characterNetWorkManager.isBlocking.Value = false;
        }
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        //IF WE HAVE FIRE DAMAGE, PLAY FIRE PARTICLES
        character.characterEffectManager.PlayeBloodSplatterVFX(contactPoint);

    }

    private void PlayDamageSFX(CharacterManager character)
    {
        character.characterSFXManager.PlayBlockSFX();
    }

    private void PlayDirectionalBasedBlockingAnimation(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (character.isDead.Value)
            return;

        DamageIntensity damageIntensity =WorldUtilityManager.instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

        switch (damageIntensity)
        {
            case DamageIntensity.Ping:
                damageAnimation = "Block_Ping_01";
                break;
            case DamageIntensity.Light:
                damageAnimation = "Block_Light_01";

                break;
            case DamageIntensity.Medium:
                damageAnimation = "Block_Medium_01";

                break;
            case DamageIntensity.Heavy:
                damageAnimation = "Block_Haevy_01";

                break;
            case DamageIntensity.Colossal:
                damageAnimation = "Block_Colossal_01";

                break;
            default:
                break;
        }



        character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
        character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);




    }
}
