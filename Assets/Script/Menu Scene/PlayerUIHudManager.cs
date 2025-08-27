using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] CanvasGroup[] canvasGroup;

    [Header("STAT BARS")]
    [SerializeField] UI_StatBars healthBar;
    [SerializeField] UI_StatBars staminaBar;
    [SerializeField] UI_StatBars focusPointBar;

    [Header("Runes")]
    [SerializeField] float runesUpdateCountDelayTimer = 2.5f;
    private int pendingRunesToAdd = 0;
    private Coroutine waitThenAddRunesCoroutine;
    [SerializeField] TextMeshProUGUI runesToAddText;
    public TextMeshProUGUI runesCountText;

    [Header("QUICK SLOTS")]
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;
    [SerializeField] Image spellItemQuickSlotIcon;
    [SerializeField] TextMeshProUGUI quickSlotItemCount;
    [SerializeField] Image quickSlotItemQuickSlotIcon;
    [SerializeField] GameObject projectileQuickSlotsGameOject;
    [SerializeField] Image mainProjectileQuickSlotIcon;
    [SerializeField] TextMeshProUGUI mainProjectileCount;
    [SerializeField] Image secondaryProjectileQuickSlotIcon;
    [SerializeField] TextMeshProUGUI secondaryProjectileCount;

    [Header("Boss Health Bar")]
    public Transform bossHealthBarParent;
    public GameObject bossHealthBarObject;
    [HideInInspector] public UI_Boss_HP_Bar currentBossHealthBar;

    [Header("Crosshair")]
    public GameObject crossHair;

    public void ToggleHud(bool status)
    {
        if (status)
        {
            foreach (var canvas in canvasGroup)
            {
                canvas.alpha = 1;
            }
        }
        else
        {
            foreach (var canvas in canvasGroup)
            {
                canvas.alpha = 0;
            }
        }
    }

    public void RefeshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
        focusPointBar.gameObject.SetActive(false);
        focusPointBar.gameObject.SetActive(true);
    }

    public void SetRunesCount(int runesToAdd)
    {
        pendingRunesToAdd += runesToAdd;

        if(waitThenAddRunesCoroutine != null)
            StopCoroutine(waitThenAddRunesCoroutine); 

        waitThenAddRunesCoroutine = StartCoroutine(WaitThenUpdateRunesCount());
    }

    public IEnumerator WaitThenUpdateRunesCount()
    {
        float timer = runesUpdateCountDelayTimer;
        int runesToAdd   = pendingRunesToAdd;
        runesToAddText.text = "+ " + runesToAdd.ToString();
        runesToAddText.enabled = true;

        if (runesToAdd >= 0)
        {
            runesToAddText.text = "+ " + runesToAdd.ToString();
        }
        else
        {
            runesToAddText.text = "- " +Mathf.Abs( runesToAdd).ToString();
        }


        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (runesToAdd != pendingRunesToAdd)
            {
                runesToAdd = pendingRunesToAdd;
                runesToAddText.text ="+ " + runesToAdd.ToString();

            }

            yield return null;
        }

        runesToAddText.enabled = false;
        pendingRunesToAdd = 0;
        runesCountText.text = PlayerUIManager.instance.localPlayer.playerStatManager.runes.ToString();


        yield return null;
    }

    public void SetNewHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }

    public void SetNewStaminaValue(float oldValue , float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina);
    }

    public void SetNewFocusPointValue(int  oldValue, int newValue)
    {
        focusPointBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxFocusPointValue(int maxFocusPoints)
    {
        focusPointBar.SetMaxStat(maxFocusPoints);
    }

    public void SetRightWeaponQuickSlotIcon(int weaponId)
    {

        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponId);

        if (WorldItemDatabase.instance.GetWeaponByID(weaponId) == null)
        {
            Debug.Log("ITEM IS NULL");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }
        if(weapon.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        // THIS IS WHERE 
        rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        rightWeaponQuickSlotIcon.enabled = true;   
    }

    public void SetLeftWeaponQuickSlotIcon(int weaponId)
    {

        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponId);

        if (WorldItemDatabase.instance.GetWeaponByID(weaponId) == null)
        {
            Debug.Log("ITEM IS NULL");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }
        if (weapon.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        // THIS IS WHERE 
        leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        leftWeaponQuickSlotIcon.enabled = true;
    }

    public void SetSpellItemQuickSlotIcon(int spellID)
    {

        SpellItem spell = WorldItemDatabase.instance.GetSpellByID(spellID);

        if (spell == null)
        {
            Debug.Log("ITEM IS NULL");
            spellItemQuickSlotIcon.enabled = false;
            spellItemQuickSlotIcon.sprite = null;
            return;
        }
        if (spell.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            spellItemQuickSlotIcon.enabled = false;
            spellItemQuickSlotIcon.sprite = null;
            return;
        }

        // THIS IS WHERE 
        spellItemQuickSlotIcon.sprite = spell.itemIcon;
        spellItemQuickSlotIcon.enabled = true;
    }

    public void SetQuickSlotItemQuickSlotIcon(QuickSlotItem quickSlotItem)
    {

        if (quickSlotItem == null)
        {
            Debug.Log("ITEM IS NULL");
            quickSlotItemQuickSlotIcon.enabled = false;
            quickSlotItemQuickSlotIcon.sprite = null;
            quickSlotItemCount.enabled = false;
            return;
        }
        if (quickSlotItem.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            quickSlotItemQuickSlotIcon.enabled = false;
            quickSlotItemQuickSlotIcon.sprite = null;
            quickSlotItemCount.enabled = false;

            return;
        }

        // THIS IS WHERE 
        quickSlotItemQuickSlotIcon.sprite = quickSlotItem.itemIcon;
        quickSlotItemQuickSlotIcon.enabled = true;

        if (quickSlotItem.isConsumable)
        {
            quickSlotItemCount.text = quickSlotItem.GetCurrentAmount(PlayerUIManager.instance.localPlayer).ToString();
            quickSlotItemCount.enabled = true;
        }else
        {
            quickSlotItemCount.enabled = false;
                
        }
    }

    public void ToggleProjectileQuickSlotsVisibility(bool status)
    {
        projectileQuickSlotsGameOject.SetActive(status);
    }

    public void SetMainProjectileQuickSlotIcon(RangedProjectileItem projectileItem )
    {


        if (projectileItem == null)
        {
            Debug.Log("ITEM IS NULL");
            mainProjectileQuickSlotIcon.enabled = false;
            mainProjectileQuickSlotIcon.sprite = null;
            mainProjectileCount.enabled = false;
            return;
        }
        if (projectileItem.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            mainProjectileQuickSlotIcon.enabled = false;
            mainProjectileQuickSlotIcon.sprite = null;
            mainProjectileCount.enabled = false;
            return;
        }

        // THIS IS WHERE 
        mainProjectileQuickSlotIcon.sprite = projectileItem.itemIcon;
        mainProjectileCount.text = projectileItem.currentAmmoAmount.ToString();
        mainProjectileQuickSlotIcon.enabled = true;
        mainProjectileCount.enabled = true;

    }

    public void SetSecondaryProjectileQuickSlotIcon(RangedProjectileItem projectileItem)
    {


        if (projectileItem == null)
        {
            Debug.Log("ITEM IS NULL");
            secondaryProjectileQuickSlotIcon.enabled = false;
            secondaryProjectileQuickSlotIcon.sprite = null;
            secondaryProjectileCount.enabled = false;
            return;
        }
        if (projectileItem.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            secondaryProjectileQuickSlotIcon.enabled = false;
            secondaryProjectileQuickSlotIcon.sprite = null;
            secondaryProjectileCount.enabled = false;
            return;
        }

        // THIS IS WHERE 
        secondaryProjectileQuickSlotIcon.sprite = projectileItem.itemIcon;
        secondaryProjectileCount.text = projectileItem.currentAmmoAmount.ToString();
        secondaryProjectileQuickSlotIcon.enabled = true;
        secondaryProjectileCount.enabled = true;

    }

}
