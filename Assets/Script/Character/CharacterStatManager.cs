using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Runes")]
    public int runesDroppedOnDeath = 50;

    [Header("Stamina Regenation")]
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2;
    [SerializeField] float staminaRegenAmount = 2;

    [Header("Blocking Absorptions")]
    public float blockingPhysicalAbsorptions;
    public float blockingFireAbsorptions;
    public float blockingMagicAbsorptions;
    public float blockingLightingAbsorptions;
    public float blockingHolyAbsorptions;
    public float blockingStability;

    [Header("Armor Absorptions")]
    public float armorPhysicalDamageAbsorption;
    public float armorMagicDamageAbsorption;
    public float armorFireDamageAbsorption;
    public float armorLightningDamageAbsorption;
    public float armorHolyDamageAbsorption;
        
    [Header("Armor Resistances")]
    public float armorImmunity;        // RESISTANCE TO ROT AND POISON
    public float armorRobustness;      // RESISTANCE TO BLEED AND FROST
    public float armorFocus;           // RESISTANCE TO MADNESS AND SLEEP
    public float armorVitality;



    [Header("Poise")]
    public float totalPoiseDamage;
    public float offensivePoiseDamageBonus;
    public float basePoiseDefense;
    public float defaultPoiseResetTime = 8;
    public float poiseResetTimer = 8;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();    
    }

    public int CaculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = 0;

        health = vitality * 15;

        return Mathf.RoundToInt(health);
    }

    public int CaculateStaminaBasedEnduranceLevel(int endurance)
    {
        float stamina = 0;

        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }

    public int CaculateFocusPointsBasedOnMindLevel(int mind)
    {
        int focusPoints = 0;

        focusPoints = mind * 10;

        return Mathf.RoundToInt(focusPoints);
    }

    public int CaculateCharacterLevelBasedOnAttributes(bool calculateProjectedLevel = false)
    {
        if (calculateProjectedLevel)
        {
            int totalProjectedAttributes =
            Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.vigorSlider.value) +
            Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.enduranceSlider.value) +
            Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.mindSlider.value) +
            Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.strengthSlider.value) +
            Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.dexteritySlider.value) +
            Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.intelligenceSlider.value) +
            Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.faithSlider.value);
          

            int projectedCharacterLevel = totalProjectedAttributes - 70 + 1;

            if (projectedCharacterLevel < 1)
                projectedCharacterLevel = 1;

            return projectedCharacterLevel;
        }

        int totalAttributes = character.characterNetWorkManager.vigor.Value +
        character.characterNetWorkManager.mind.Value +
        character.characterNetWorkManager.endurance.Value +
        character.characterNetWorkManager.strength.Value +
        character.characterNetWorkManager.dexterity.Value +
        character.characterNetWorkManager.intelligence.Value +
        character.characterNetWorkManager.faith.Value;

        int characterLevel = totalAttributes - 70 + 1;

        if (characterLevel < 1)
            characterLevel = 1;

        return characterLevel;

    }

    public virtual void RegenerateStamina()
    {
        if (!character.IsOwner)
            return;

        if (character.characterNetWorkManager.isSprinting.Value)
            return;

        if (character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetWorkManager.currentStamina.Value < character.characterNetWorkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.characterNetWorkManager.currentStamina.Value += staminaRegenAmount;
                }
            }

        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount , float currentStaminaAmount)
    {
        if(currentStaminaAmount < previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
    }

    protected virtual void HandlePoiseResetTimer()
    {
        if(poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDamage = 0;
        }
    }
}
