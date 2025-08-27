using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffects
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
    protected int finalDamageDealt = 0;

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


    public override void ProcessEffect(CharacterManager character)
    {
        if(character.characterNetWorkManager.isInvulnerable.Value)
            return;
        base.ProcessEffect(character);

        if (character.isDead.Value)
            return;

        CalculateDamage(character);
        PlayDirectionalBasedDamageAnimation(character);
        PlayDamageSFX(character);
        PlayDamageVFX(character);
        CalculateStanceDamage(character);
    }

    protected  virtual void  CalculateDamage(CharacterManager character)
    {
        if(!character.IsOwner)
            return;

        if(characterCausingDamage != null)
        {

        }

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightingDamage + holyDamage);

        if(finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        character.characterNetWorkManager.currentHealth.Value -= finalDamageDealt;

        character.characterStatManager.totalPoiseDamage -= poiseDamage;
        character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

        float remainingPoise = character.characterStatManager.basePoiseDefense +
            character.characterStatManager.offensivePoiseDamageBonus +
            character.characterStatManager.totalPoiseDamage;

        if(remainingPoise <= 0)
            poiseIsBroken = true;

        character.characterStatManager.poiseResetTimer = character.characterStatManager.defaultPoiseResetTime;
    }

    protected void CalculateStanceDamage(CharacterManager character)
    {
        AICharacterManager aiCharacter = character as   AICharacterManager;

        int stanceDamage = Mathf.RoundToInt(poiseDamage) ;

        if (aiCharacter != null)
        {
            aiCharacter.aiCharacterCombatManager.DamageStance(stanceDamage);
        }
    }

    protected void PlayDamageVFX(CharacterManager character)
    {
        //IF WE HAVE FIRE DAMAGE, PLAY FIRE PARTICLES
        character.characterEffectManager.PlayeBloodSplatterVFX(contactPoint);

    }

    protected void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSFXManger.instance.ChooseRandomSFXFromArray(WorldSFXManger.instance.physicalDamageSFX);

        character.characterSFXManager.PlaySoundFX(physicalDamageSFX);
        character.characterSFXManager.PlayDamageGrunt();
    }

    protected void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (character.isDead.Value) 
            return;

        if (poiseIsBroken)
        {
            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                // PLAY FRONT ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                // PLAY BACK ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);

            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                // PLAY BACK ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);

            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                // PLAY LEFT ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);

            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                // PLAY RIGHT ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
            }
        }
        else
        {

            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                // PLAY FRONT ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                // PLAY BACK ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);

            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                // PLAY BACK ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Ping_Damage);

            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                // PLAY LEFT ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Ping_Damage);

            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                // PLAY RIGHT ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Ping_Damage);
            }

        }


        character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;

        if(poiseIsBroken)
        {
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            character.characterCombatManager.DestroyAllCurrentActionFX();
        }
        else
        {
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, false , false , true ,true);

        }
    }
}
