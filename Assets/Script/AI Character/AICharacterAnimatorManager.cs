using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterAnimatorManager : CharacterAnimatorManager
{
    AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();
    }

    private void OnAnimatorMove()
    {
        //HOST
        if (aiCharacter.IsOwner)
        {
            if (!aiCharacter.characterLocomotionManager.isGrounded)
            {
                return;
            }

            Vector3 velocity = aiCharacter.animator.deltaPosition;

            aiCharacter.characterController.Move(velocity);
            aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
        }
        //CLIENT
        else
        {
            if (!aiCharacter.characterLocomotionManager.isGrounded)
            {
                return;
            }

            Vector3 velocity = aiCharacter.animator.deltaPosition;

            aiCharacter.characterController.Move(velocity);
            aiCharacter.transform.position = Vector3.SmoothDamp(transform.position,
                aiCharacter.characterNetWorkManager.networkPosition.Value , 
                ref aiCharacter.characterNetWorkManager.networkPositionVelocity,
                aiCharacter.characterNetWorkManager.networkRotationSmoothTime);

            aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
        }
    }
}
