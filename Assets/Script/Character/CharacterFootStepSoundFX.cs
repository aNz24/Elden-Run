using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFootStepSoundFX : MonoBehaviour
{
    CharacterManager character;

    AudioSource audioSource;
    GameObject steppedOnObject;

    private bool hasTouchedGround=false;
    private bool hasPlayFootStepSFX=false;
   // [SerializeField] float distanceToGround = 0.05f;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        character = GetComponentInParent<CharacterManager>();
    }

    private void FixedUpdate()
    {
        CheckForFootStep();
    }

    private void CheckForFootStep()
    {
        if(character == null) return;

        if(!character.characterNetWorkManager.isMoving.Value) return;   

        RaycastHit hit;

        if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, 0.05f, WorldUtilityManager.instance.GetEnviroLayer()))
        {
            hasTouchedGround = true;

            if (!hasPlayFootStepSFX)
                steppedOnObject = hit.transform.gameObject;
        }
        else
        {
            hasTouchedGround =false;
            hasPlayFootStepSFX = false;
            steppedOnObject = null;
        }

        if(hasTouchedGround && !hasPlayFootStepSFX )
        {
            hasPlayFootStepSFX = true;
            PlayFootStepSFX();
        }
    }

    private void PlayFootStepSFX()
    {
        character.characterSFXManager.PlayFootStepSFX();
    }
}
