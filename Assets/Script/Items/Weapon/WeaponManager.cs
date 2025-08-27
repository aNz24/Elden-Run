using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage( CharacterManager characterWieldingWeapon, WeaponItem weapon)
    {
        if(meleeDamageCollider == null)
            return;

        meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
        meleeDamageCollider.physicalDamage = weapon.physicalDamage;
        meleeDamageCollider.magicDamage = weapon.magicDamage;
        meleeDamageCollider.fireDamage = weapon.fireDamage;
        meleeDamageCollider.lightingDamage = weapon.lightingDamage;
        meleeDamageCollider.holyDamage = weapon.holyDamage;
        meleeDamageCollider.poiseDamage = weapon.poiseDamage;

        meleeDamageCollider.light_Attack_01_Modifiers = weapon.light_Attack_01_Modifier;
        meleeDamageCollider.light_Attack_02_Modifiers = weapon.light_Attack_02_Modifier;
        meleeDamageCollider.light_Jump_Attack_01_Modifier = weapon.light_Jump_Attack_01_Modifier;

        meleeDamageCollider.heavy_Attack_01_Modifiers = weapon.heavy_Attack_01_Modifier;
        meleeDamageCollider.heavy_Attack_02_Modifiers = weapon.heavy_Attack_02_Modifier;
        meleeDamageCollider.heavy_Jump_Attack_01_Modifier = weapon.heavy_Jump_Attack_01_Modifier;

        meleeDamageCollider.charge_Attack_01_Modifiers = weapon.charge_Attack_01_Modifier;
        meleeDamageCollider.charge_Attack_02_Modifiers = weapon.charge_Attack_02_Modifier;

        meleeDamageCollider.running_Attack_01_Modifiers = weapon.running_Attack_01_Modifiers;
        meleeDamageCollider.rolling_Attack_01_Modifiers = weapon.rolling_Attack_01_Modifiers;
        meleeDamageCollider.backstep_Attack_01_Modifiers = weapon.backstep_Attack_01_Modifiers;




    }
}
