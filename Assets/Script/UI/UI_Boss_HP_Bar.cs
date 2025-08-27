using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Boss_HP_Bar : UI_StatBars   
{
    [SerializeField] AIBossCharacterManager bossCharacter;

    public void EnableBossHPBar(AIBossCharacterManager boss)
    {
        bossCharacter = boss;
        bossCharacter.aiCharacteNetworkManager.currentHealth.OnValueChanged += OnBossHPChanged;
        SetMaxStat(bossCharacter.characterNetWorkManager.maxHealth.Value);
        SetStat(bossCharacter.aiCharacteNetworkManager.currentHealth.Value);
        GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.characterName;    
    }

    private void OnDestroy()
    {
         bossCharacter.aiCharacteNetworkManager.currentHealth.OnValueChanged -= OnBossHPChanged;
    }

    private void OnBossHPChanged(int oldValue , int newValue)
    {
        SetStat(newValue);

        if(newValue  <= 0)
        {
            RemoveHPBar(2.5f);
        }
    }

    public void RemoveHPBar( float time)
    {
        Destroy(gameObject, time);
    }
}
