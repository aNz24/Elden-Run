using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ranged Projectile")]
public class RangedProjectileItem : Item
{
    public ProjcetileClass projcetileClass;

    [Header("Velocity")]
    public float forwardVelocity = 2200;
    public float upwardVelocity = 0;
    public float ammoMass = .01f;

    [Header("Capacity")]
    public int maxAmmoAmout = 30;
    public int currentAmmoAmount = 30;

    [Header("Damage")]
    public int physicalDamage = 0;
    public int fireDamage = 0;
    public int magicDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;

    [Header("Model")]
    public GameObject drawProjectileModel;
    public GameObject releaseProjectileModel;
}
