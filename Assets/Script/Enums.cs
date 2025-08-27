using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
   
}

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    NO_SLOT
}

public enum CharacterGroup
{
    Team01,
    Team02,
    Team03
}

public enum WeaponModelSlot
{
    RightHand,
    LeftHandWeaponSlot,
    LeftHandShieldSlot,
    BackSlot
}

public enum WeaponModelType
{
    Weapon,
    Shield
}

public enum EquipmentModelType
{
    FullHelmet,
    Hat,
    Hood,
    HelmetAcessorie,
    FaceCover,
    Torso,
    Back,
    RightShoulder,
    RightUpperArm,
    RightHand,
    RightElbow,
    RightLowerArm,
    LeftShoulder,
    LeftUpperArm,
    LeftHand,
    LeftElbow,
    LeftLowerArm,
    Hips,
    HipsAttachment,
    RightLeg,
    RightKnee,
    LeftLeg,
    LeftKnee,
}

public enum EquipmentType
{
    RightWeapon01,
    RightWeapon02,
    RightWeapon03,
    LeftWeapon01,
    LeftWeapon02,
    LeftWeapon03,
    Head,
    Body,
    Hands,
    Legs,
    MainProjectile,
    SecondaryProjectile,
    QuickSlot01,
    QuickSlot02,
    QuickSlot03,
}

public enum HeadEquipmentType
{
    FullHelmet,
    Hat,
    Hood,
    FaceCover
}

public enum SpellClass
{
    Incantation,
    Sorcery
}

public enum CharacterAttributes
{
    Vigor,
    Mind,
    Endurance,
    Strength,
    Dexterity,
    Intelligence,
    Faith
} 

public enum AttackType
{
    LightAttack01,
    LightAttack02,
    HeavyAttack01,
    HeavyAttack02,
    ChargedAttack01,
    ChargedAttack02,
    RunningAttack01,
    RollingAttack01,
    BackstepAttack01,
    HeavyJumpingAttack01,
    LightJumpingAttack01
}

public enum DamageIntensity
{
    Ping,
    Light,
    Medium,
    Heavy,
    Colossal
}

public enum WeaponClass
{
    StraightSword,
    Spear,
    MediumShield,
    Fist,
    LightShield,
    Bow
}

public enum ProjcetileClass
{
    Arrow,
    Bolt
}

public enum ProjectileSlot
{
    Main,
    Secondary
}

public enum ItemPickUpType
{
    WorldSpawn,
    CharacterDrop
}

public enum IdleStateMode
{
    Idle,
    Patrol,
    Sleep
}