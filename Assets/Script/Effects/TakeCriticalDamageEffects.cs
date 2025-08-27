using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Critical Damage")]
public class TakeCriticalDamageEffects : TakeDamageEffect
{
    public override void ProcessEffect(CharacterManager character)
    {
        if (character.characterNetWorkManager.isInvulnerable.Value)
            return;

        if (character.isDead.Value)
            return;

        CalculateDamage(character);

        character.characterCombatManager.pendingCriticalDamage = finalDamageDealt;
    }

    protected override void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (characterCausingDamage != null)
        {

        }

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightingDamage + holyDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }


        character.characterStatManager.totalPoiseDamage -= poiseDamage;
        character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

        float remainingPoise = character.characterStatManager.basePoiseDefense +
            character.characterStatManager.offensivePoiseDamageBonus +
            character.characterStatManager.totalPoiseDamage;

        if (remainingPoise <= 0)
            poiseIsBroken = true;

        character.characterStatManager.poiseResetTimer = character.characterStatManager.defaultPoiseResetTime;
    }

}
