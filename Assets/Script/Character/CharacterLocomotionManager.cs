using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check & Jumping")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 yVelocity; // THE FORCE AT WHICH OUR CHARACTER IS PULLED UP OR DOWN (JUMPING OR FALLING)
    [SerializeField] protected float groundedYVelocity = -20; // THE FORCE AT WHICH OUR CHARACTER IS STICKING TO THE GROUND WHILST THEY ARE GROUNDED 
    [SerializeField] protected float fallStartYVelocity = -5;
    protected bool fallingVelocityHAsBeenSet = false;
    protected float inAirTimer = 0;

    [Header("Flags")]
    public bool isRolling= false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool canRun = true;
    public bool canRoll = true;
    public bool isGrounded = true;


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
      
    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if (character.characterLocomotionManager.isGrounded)
        {
            // iF WE ARE NOT ATTEMPTING TO JUMP OR MOVE UPWARD
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHAsBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            // IF WE ARE NOT JUMPING, AND OUR FALLING VELOCITY HAS NOT BEEN SET
            if (!character.characterNetWorkManager.isJumping.Value && !fallingVelocityHAsBeenSet)
            {
                fallingVelocityHAsBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            character.animator.SetFloat("inAirTimer", inAirTimer);
            yVelocity.y += gravityForce * Time.deltaTime;

        }

        character.characterController.Move(yVelocity * Time.deltaTime);

    }

    protected void HandleGroundCheck()
    {
        character.characterLocomotionManager.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    // DRAWS OUR GROUND CHECK SPHERE IN SCENE VIEW
    protected void OnDrawGizmosSelected()
    {
        //Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }

    public void EnableCanRotate()
    {
        canRotate = true;
    }

    public void DisableCanRotate()
    {
        canRotate = false;
    }
}
