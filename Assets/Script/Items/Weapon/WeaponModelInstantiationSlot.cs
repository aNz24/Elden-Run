
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelInstantiationSlot : MonoBehaviour
{
    public WeaponModelSlot weaponSlot;
    public GameObject currentWeaponModel;


    public void UnloadWeapon()
    {
        if(currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void PlaceWeaponModelIntoSlot(GameObject weaponModel)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;

    }

    public void PlaceWeaponModelInUnequippedSot(GameObject  weaponModel , WeaponClass weaponClass , PlayerManager player)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        switch (weaponClass)
        {
            case WeaponClass.StraightSword:
                weaponModel.transform.localPosition = new Vector3(.064f, 0f, -0.06f);
                weaponModel.transform.localRotation = Quaternion.Euler(194, 90, -0.22f);
                break;
            case WeaponClass.Spear:
                weaponModel.transform.localPosition = new Vector3(.064f, 0f, -0.06f);
                weaponModel.transform.localRotation = Quaternion.Euler(194, 90, -0.22f);
                break;
            case WeaponClass.MediumShield:
                weaponModel.transform.localPosition = new Vector3(0.25f, -0.047f, -0.173f);
                weaponModel.transform.localRotation = Quaternion.Euler(-13.541f, 19.182f, -182.02f);
                break;
            default:
                break;
        }
    }
}
