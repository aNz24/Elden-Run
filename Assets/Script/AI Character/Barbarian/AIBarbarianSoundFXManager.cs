using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBarbarianSoundFXManager : CharacterSFXManager
{
    [Header("Whooshes")]
    public AudioClip[] whooshes;

    [Header("Impact")]
    public AudioClip[] impacts;

    public virtual void PlayImpactSFX()
    {
        if (impacts.Length > 0)
            PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(impacts));
    }
}
