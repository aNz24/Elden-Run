using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem
{
    [Header("Animations")]
    public AnimatorOverrideController weaponAnimator;

    [Header("Model Instantiation")]
    public WeaponModelType weaponModelType;


    [Header("Weapon Model")]
    public GameObject weaponModel;


    [Header("Weapon Class")]
    public WeaponClass weaponClass;


    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage =0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightingDamage = 0;  

    [Header("Weapon Poise")]
    public float poiseDamage = 10;

    [Header("Attack Modifiers")]
    public float light_Attack_01_Modifier = 1.0f;
    public float light_Attack_02_Modifier = 1.2f;
    public float light_Jump_Attack_01_Modifier = 1.0f;
    public float heavy_Attack_01_Modifier = 1.4f;
    public float heavy_Attack_02_Modifier = 1.6f;
    public float heavy_Jump_Attack_01_Modifier = 1.8f;
    public float charge_Attack_01_Modifier = 2.0f;
    public float charge_Attack_02_Modifier = 2.0f;
    public float running_Attack_01_Modifiers = 1.1f;
    public float rolling_Attack_01_Modifiers = 1.1f;
    public float backstep_Attack_01_Modifiers = 1.1f;




    [Header("Stamina Cost Modifiers")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 1.0f;
    public float heavyAttackStaminaCostMultiplier = 1.3f;
    public float chargedAttackStaminaCostMultiplier = 1.5f;
    public float runningAttackStaminaCostMultiplier = 1.1f;
    public float rollingAttackStaminaCostMultiplier = 1.1f;
    public float backstepAttackStaminaCostMultiplier = 1.1f;

    [Header("Weapon Blocking Absorption")]
    public float physicalBaseDamageAbsorption = 50;
    public float fireBaseDamageAbsorption = 50;
    public float magiclBaseDamageAbsorption = 50;
    public float lightingBaseDamageAbsorption = 50;
    public float holyBaseDamageAbsorption = 50;
    public float stability = 50;


    [Header("Actions")]
    public WeaponItemAction oh_RB_Action;
    public WeaponItemAction oh_RT_Action;
    public WeaponItemAction oh_LB_Action;
    public AshOfWar ashOfWarAction;

 

    [Header("SFX")]
    public AudioClip[] whooshes;
    public AudioClip[] blockingSFX;
}
