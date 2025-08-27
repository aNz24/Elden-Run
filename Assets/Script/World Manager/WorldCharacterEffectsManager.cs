using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager instance;

    [Header("VFX")]
    public GameObject bloodSplatterVFX;
    public GameObject criticalbloodSplatterVFX;
    public GameObject healingFlaskVFX;
    public GameObject deadSpotVFX;


    [Header("Damage")]
    public TakeDamageEffect takeDamageEffect;
    public TakeBlockDamageEffect takeBlockDamageEffect;
    public TakeCriticalDamageEffects takeCriticalDamageEffects;

    [Header("Two Hand")]
    public TwoHandEffect twoHandingEffect;

    [Header("Instant Effects")]
    [SerializeField] List<InstantCharacterEffects> instantEffect;


    [Header("Static Effects")]
    [SerializeField] List<StaticCharacterEffect> staticEffect;

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

        GeneralEffectIDs();
    }

    private void GeneralEffectIDs()
    {
        for(int i = 0; i < instantEffect.Count; ++i)
        {
            instantEffect[i].instantEffectID = i;
        }

        for (int i = 0; i < staticEffect.Count; ++i)
        {
            staticEffect[i].staticEffectID = i;
        }
    }
}
