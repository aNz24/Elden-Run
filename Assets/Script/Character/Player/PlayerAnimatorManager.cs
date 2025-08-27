using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager player;


    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    private void OnAnimatorMove()
    {
        if (applyRootMotion)
        {
            Vector3 verlocity =player.animator.deltaPosition;
            player.characterController.Move(verlocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }

    
}
