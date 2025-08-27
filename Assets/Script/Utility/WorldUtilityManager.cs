using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager instance;

    [Header("Layer")]
    [SerializeField] LayerMask characterLayer;
    [SerializeField] LayerMask enviroLayer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LayerMask GetCharacterLayer()
    {
        return characterLayer;
    }

    public LayerMask GetEnviroLayer()
    {
        return enviroLayer;
    }

    public bool CanIDamageThisTarget(CharacterGroup attackingCharacter , CharacterGroup targetCharacter)
    {
        if(attackingCharacter == CharacterGroup.Team01)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.Team01:
                    return false;
                case CharacterGroup.Team02:
                    return true;
                default:
                    break;
            }
        }
        else if (attackingCharacter == CharacterGroup.Team02)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.Team01:
                    return true;
                case CharacterGroup.Team02:
                    return false;
                default:
                    break;
            }
        }

        return false;
    }

       
    public float GetAngleOfTarget(Transform characterTranfrom, Vector3 targetsDirection)
    {
        targetsDirection.y = 0f;
        float viewableAngle = Vector3.Angle(characterTranfrom.forward, targetsDirection);
        Vector3 cross = Vector3.Cross(characterTranfrom.forward, targetsDirection);

        if (cross.y < 0f) 
            viewableAngle = -viewableAngle;
        
        return viewableAngle; 
       
    }
    
    public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
    {
        DamageIntensity damageIntensity = DamageIntensity.Ping;

        if(poiseDamage >=10)
            damageIntensity = DamageIntensity.Light;

        if (poiseDamage >= 30)
            damageIntensity = DamageIntensity.Medium;

        if (poiseDamage >= 70)
            damageIntensity = DamageIntensity.Heavy;

        if (poiseDamage >= 120)
            damageIntensity = DamageIntensity.Colossal;

        return damageIntensity;
    }

    public Vector3 GetRipostingPositionBasedOnWeaponClass(WeaponClass weaponClass)
    {
         Vector3 positon = new Vector3 (.11f, 0, .7f);
        switch (weaponClass)
        {
            case WeaponClass.StraightSword:
                break;
             case WeaponClass.Spear:
                break;
            case WeaponClass.MediumShield:
                break;
            case WeaponClass.Fist:
                break;
        }

        return positon;
    }

    public Vector3 GetBackstabPositionBasedOnWeaponClass(WeaponClass weaponClass)
    {
        Vector3 positon = new Vector3(.12f, 0, .74f);
        switch (weaponClass)
        {
            case WeaponClass.StraightSword:
                break;
            case WeaponClass.Spear:
                break;
            case WeaponClass.MediumShield:
                break;
            case WeaponClass.Fist:
                break;
        }

        return positon;
    }
}
