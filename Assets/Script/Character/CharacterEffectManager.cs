using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectManager : MonoBehaviour
{
    // FROCESS INSTANT EFFECTS (TAKE DAMAME , HEAL)
    // FROCESS TIMED EFFECTS (POISON )
    // FROCESS STATIC EFFECTS (  ADDING/BUFFS )

    CharacterManager character;

    [Header("Current Active FX")]
    public GameObject activeSpellWarnUpFX;
    public GameObject activeDrawProjectileFX;
    public GameObject activeQuickSlotItemFX;

    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterVFX;
    [SerializeField] GameObject criticalbloodSplatterVFX;

    [Header("Static Effects")]
    public List<StaticCharacterEffect> staticEffects = new List<StaticCharacterEffect>();


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffects(InstantCharacterEffects effect)
    {
        //TAKE IN AN EFFECT
        effect.ProcessEffect(character);
    }

    public void PlayeBloodSplatterVFX(Vector3 contactPoint)
    {
        if (bloodSplatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }

    public void PlayCriticalBloodSplatterVFX(Vector3 contactPoint)
    {
        if (criticalbloodSplatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(criticalbloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.criticalbloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }

    public void AddStaticEffect(StaticCharacterEffect effect)
    {
        staticEffects.Add(effect);
        effect.ProccessStaticEffect(character);

        for(int i = staticEffects.Count -1;i>-1; i--)
        {
            if(staticEffects[i] == null)
                staticEffects.RemoveAt(i);
        }
    }

    public void RemoveStaticEffect(int effectID)
    {
        StaticCharacterEffect effect;

        for (int i = 0; i < staticEffects.Count; i++)
        {
            if (staticEffects[i] != null)
            {
                if (staticEffects[i].staticEffectID == effectID)
                {
                    effect = staticEffects[i];
                    effect.RemoveStaticEffect(character);
                    staticEffects.Remove(effect);
                }
            }
        }

        for (int i = staticEffects.Count - 1; i > -1; i--)
        {
            if (staticEffects[i] == null)
                staticEffects.RemoveAt(i);
        }
    }
}
