using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerBossFight : MonoBehaviour
{
    [SerializeField] int bossID;
    private AIBossCharacterManager boss;

    private void OnTriggerEnter(Collider other)
    {
        boss = WorldAIManager.instance.GetBossCharacterByID(bossID);

        if (boss == null)
            return;

        boss.WakeBoss();

        boss.hasBeenDefeated.OnValueChanged -= OnBossDefeated;

        boss.hasBeenDefeated.OnValueChanged += OnBossDefeated;

        if (boss.hasBeenDefeated.Value)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnBossDefeated(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (boss != null)
        {
            boss.hasBeenDefeated.OnValueChanged -= OnBossDefeated;
        }
    }
}
