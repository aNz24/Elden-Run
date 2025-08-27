using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItem : EquipmentItem
{
    [Header("Equipment Absorption Bouns")]
    public float physicalDamageAbsorption;
    public float magicDamageAbsorption;
    public float fireDamageAbsorption;
    public float lightningDamageAbsorption;
    public float holyDamageAbsorption;

    [Header("Equipment Resistance Bouns")]
    public float immunity;        // RESISTANCE TO ROT AND POISON
    public float robustness;      // RESISTANCE TO BLEED AND FROST
    public float focus;           // RESISTANCE TO MADNESS AND SLEEP
    public float vitality;        // RESISTANCE TO DEATH AND CURSE

    [Header("Poise")]
    public float poise;

    public EquipmentModel[] equipmentModels;
}
