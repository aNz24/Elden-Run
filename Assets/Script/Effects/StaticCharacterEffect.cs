using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCharacterEffect : ScriptableObject
{
    [Header("Effect I.D")]
    public int staticEffectID;

    public virtual void ProccessStaticEffect(CharacterManager character)
    {

    }

    public virtual void RemoveStaticEffect(CharacterManager character)
    {

    }
}
