using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatManager : CharacterStatManager
{
    PlayerManager player;

    [Header("Runes")]
    public int runes = 0;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();

        CaculateHealthBasedOnVitalityLevel(player.playerNetWorkManager.vigor.Value);
        CaculateStaminaBasedEnduranceLevel(player.playerNetWorkManager.endurance.Value);
        CaculateFocusPointsBasedOnMindLevel(player.playerNetWorkManager.mind.Value);    
    }

    public void CalculateTotalArmorAbsorption()
    {
        armorPhysicalDamageAbsorption = 0;
        armorMagicDamageAbsorption = 0;
        armorFireDamageAbsorption = 0;
        armorHolyDamageAbsorption = 0;
        armorLightningDamageAbsorption = 0;

        armorRobustness = 0;
        armorVitality = 0;
        armorImmunity = 0;
        armorFocus = 0;

        basePoiseDefense = 0;
       
        if (player.playerInventoryManager.headEquipment != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.headEquipment.physicalDamageAbsorption;
            armorMagicDamageAbsorption += player.playerInventoryManager.headEquipment.magicDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.headEquipment.fireDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.headEquipment.holyDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.headEquipment.lightningDamageAbsorption;

            armorRobustness += player.playerInventoryManager.headEquipment.robustness;
            armorVitality += player.playerInventoryManager.headEquipment.vitality;
            armorImmunity += player.playerInventoryManager.headEquipment.immunity;
            armorFocus += player.playerInventoryManager.headEquipment.focus;

            basePoiseDefense += player.playerInventoryManager.headEquipment.poise;
        }

        if (player.playerInventoryManager.bodyEquipment != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.bodyEquipment.physicalDamageAbsorption;
            armorMagicDamageAbsorption += player.playerInventoryManager.bodyEquipment.magicDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.bodyEquipment.fireDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.bodyEquipment.holyDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.bodyEquipment.lightningDamageAbsorption;

            armorRobustness += player.playerInventoryManager.bodyEquipment.robustness;
            armorVitality += player.playerInventoryManager.bodyEquipment.vitality;
            armorImmunity += player.playerInventoryManager.bodyEquipment.immunity;
            armorFocus += player.playerInventoryManager.bodyEquipment.focus;

            basePoiseDefense += player.playerInventoryManager.bodyEquipment.poise;
        }

        if (player.playerInventoryManager.handEquipment != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.handEquipment.physicalDamageAbsorption;
            armorMagicDamageAbsorption += player.playerInventoryManager.handEquipment.magicDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.handEquipment.fireDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.handEquipment.holyDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.handEquipment.lightningDamageAbsorption;

            armorRobustness += player.playerInventoryManager.handEquipment.robustness;
            armorVitality += player.playerInventoryManager.handEquipment.vitality;
            armorImmunity += player.playerInventoryManager.handEquipment.immunity;
            armorFocus += player.playerInventoryManager.handEquipment.focus;

            basePoiseDefense += player.playerInventoryManager.handEquipment.poise;
        }

        if (player.playerInventoryManager.legEquipment != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.legEquipment.physicalDamageAbsorption;
            armorMagicDamageAbsorption += player.playerInventoryManager.legEquipment.magicDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.legEquipment.fireDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.legEquipment.holyDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.legEquipment.lightningDamageAbsorption;

            armorRobustness += player.playerInventoryManager.legEquipment.robustness;
            armorVitality += player.playerInventoryManager.legEquipment.vitality;
            armorImmunity += player.playerInventoryManager.legEquipment.immunity;
            armorFocus += player.playerInventoryManager.legEquipment.focus;

            basePoiseDefense += player.playerInventoryManager.legEquipment.poise;
        }


    }

    public void AddRunes(int runesToAdd)
    {
        runes += runesToAdd;
        PlayerUIManager.instance.playerUIHudManager.SetRunesCount(runesToAdd); 
    }

}
