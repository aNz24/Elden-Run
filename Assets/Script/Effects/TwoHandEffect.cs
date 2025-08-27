using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Character/Static Effects/Two Handing Effect")]
public class TwoHandEffect : StaticCharacterEffect
{
    [SerializeField] int strengthGainedFromTwoHandingWeapon;

    public override void ProccessStaticEffect(CharacterManager character)
    {
        base.ProccessStaticEffect(character);

        if (character.IsOwner)
        {
            strengthGainedFromTwoHandingWeapon = Mathf.RoundToInt(character.characterNetWorkManager.strength.Value / 2);
            Debug.Log("STRENGTH GAINED: " + strengthGainedFromTwoHandingWeapon);
            character.characterNetWorkManager.strengthModifier.Value += strengthGainedFromTwoHandingWeapon;
        }
    }

    public override void RemoveStaticEffect(CharacterManager character)
    {
        base.RemoveStaticEffect(character);

        if (character.IsOwner)
        {
            character.characterNetWorkManager.strengthModifier.Value -= strengthGainedFromTwoHandingWeapon;
        }
    }
}
