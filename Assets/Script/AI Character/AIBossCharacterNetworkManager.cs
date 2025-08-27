using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossCharacterNetworkManager : AICharacteNetworkManager
{
    AIBossCharacterManager aiBossCharacter;

    protected override void Awake()
    {
        base.Awake();
        aiBossCharacter = GetComponent<AIBossCharacterManager>();
    }

    public override void OnHPChanged(int oldValue, int newValue)
    {
        base.OnHPChanged(oldValue, newValue);

        if (aiBossCharacter.IsOwner)
        {
            if (currentHealth.Value <= 0)
                return;

            float healthNeededForShift = maxHealth.Value *(aiBossCharacter.minimumHealthPercentageToShift /100);
            if (currentHealth.Value <= healthNeededForShift)
            {
                aiBossCharacter.PhaseShift();
            }
        }

       
    }
}
