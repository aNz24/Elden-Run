using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXManager : CharacterSFXManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();
        player  = GetComponent<PlayerManager>();    
    }
    public override void PlayBlockSFX()
    {
        PlaySoundFX(WorldSFXManger.instance.ChooseRandomSFXFromArray(player.playerCombatManager.currentWeaponBeingUsed.blockingSFX));
    }

    public override void PlayFootStepSFX()
    {
        base.PlayFootStepSFX();

        WorldSFXManger.instance.AlertNearbyCharacterToSound(transform.position, 2);
    }
}
