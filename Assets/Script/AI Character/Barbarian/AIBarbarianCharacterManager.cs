using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBarbarianCharacterManager : AIBossCharacterManager
{


    [HideInInspector] public AIBarbarianSoundFXManager barbariansoundFXManager;
    [HideInInspector] public AIBarbarianCombatManager barbarianCombatManager;
    
    protected override void Awake()
    {
        base.Awake();

        barbariansoundFXManager =GetComponent<AIBarbarianSoundFXManager>();
        barbarianCombatManager = GetComponent<AIBarbarianCombatManager>();
    }
}
