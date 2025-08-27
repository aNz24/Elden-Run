using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintingSpeed = 6.5f;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] int sprintingStaminaCost = 2;

    [Header("Jump")]
    [SerializeField] float jumpStaminaCost = 25;
    [SerializeField] float jumpHeight = 4;
    [SerializeField] float jumpForwardSpeed = 5;
    [SerializeField] float freeFallSpeed = 2;
    private Vector3 jumpDirection;


    [Header("Doge")]
    private Vector3 rollDirection;
    [SerializeField] float dogeStaminaCost = 25;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.characterNetWorkManager.verticalMovement.Value = verticalMovement;
            player.characterNetWorkManager.horizontalMovement.Value = verticalMovement;
            player.characterNetWorkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetWorkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetWorkManager.horizontalMovement.Value;
            moveAmount = player.characterNetWorkManager.moveAmount.Value;

            // IF IN LOCKED ON, PASS  MOVE AMOUNT
            if (!player.playerNetWorkManager.isLockedOn.Value || player.playerNetWorkManager.isSprinting.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetWorkManager.isSprinting.Value);

            }
            // IF IN LOCKED ON, PASS HOZ AND VER
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalMovement, verticalMovement, player.playerNetWorkManager.isSprinting.Value);

            }

        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
        HandleJumpingMovement();
        HandleFreeFallMovement();
    }  

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.vertical_Input;
        horizontalMovement = PlayerInputManager.instance.horizontal_Input;
        moveAmount= PlayerInputManager.instance.moveAmount;
    }

    private void HandleGroundedMovement()
    {

        if (player.characterLocomotionManager.canMove || player.playerLocomotionManager.canRotate)
        {
            GetMovementValues();

        }
        if (!player.characterLocomotionManager.canMove)
            return;

        if (player.playerNetWorkManager.isAiming.Value)
        {
            moveDirection = transform.forward * verticalMovement;
            moveDirection = moveDirection + transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
        }
        else
        {
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
        }
    

        if(player.playerNetWorkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);

            }
        }
       
    }

    private void HandleJumpingMovement()
    {
        if (player.characterNetWorkManager.isJumping.Value)
        {
            player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
        }
    }

    private void HandleFreeFallMovement()
    {
        if (!player.characterLocomotionManager.isGrounded)
        {
            Vector3 freeFallDirection;

            freeFallDirection = PlayerCamera.instance.transform.forward  * PlayerInputManager.instance.vertical_Input;
            freeFallDirection = freeFallDirection + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontal_Input;
            freeFallDirection.y = 0;

            player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (player.isDead.Value)
            return;

        if (!player.characterLocomotionManager.canRotate)
            return;

        if (player.playerNetWorkManager.isAiming.Value)
        {
            HandleAimRotatitons();
        }
        else
        {
            HandleStandardRotation();
        }
    
    }

    private void HandleAimRotatitons()
    {
        Vector3 targetDirection;
        targetDirection = PlayerCamera.instance.cameraObject.transform.forward;
        targetDirection.y = 0;
        targetDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = finalRotation;
    }

    private void HandleStandardRotation()
    {
        if (player.playerNetWorkManager.isLockedOn.Value)
        {
            if (player.playerNetWorkManager.isSprinting.Value || player.playerLocomotionManager.isRolling)
            {
                Vector3 targetDirection = Vector3.zero;
                targetDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                targetDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero)
                    targetDirection = transform.forward;

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = finalRotation;
            }
            else
            {
                if (player.playerCombatManager.currentTarget == null)
                    return;

                Vector3 targetDirection;
                targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                targetDirection.y = 0;
                targetDirection.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = finalRotation;
            }
        }
        else
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;

            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }

    public void HandleSrinting()
    {
        if (player.isPerformingAction)
        {
            player.playerNetWorkManager.isSprinting.Value = false;
        }

        if(player.playerNetWorkManager.currentStamina.Value <=0)
        {
            player.playerNetWorkManager.isSprinting.Value = false;
            return;

        }

        // IF WE ARE MOVING , SPRINTING IS TRUE
        if (moveAmount >= 0.5)
        {
            player.playerNetWorkManager.isSprinting.Value = true;
        }
        // IF WE ARE STATIONARY/MOVING SLOWLY SPRINTING IS FALSE
        else
        {
            player.playerNetWorkManager.isSprinting.Value = false;
        }

        if(player.playerNetWorkManager.isSprinting.Value)
        {
            player.playerNetWorkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
        }
    }

    public void AttemptToPerfromDoge()
    {
        if (!player.playerLocomotionManager.canRoll)
            return;

        if (player.isPerformingAction)
            return;

        if (player.playerNetWorkManager.isJumping.Value)
            return;

        if (player.playerNetWorkManager.currentStamina.Value <= 0)
            return;

        if (player.playerNetWorkManager.isHodingArrow.Value)
            return;

        if (PlayerInputManager.instance.moveAmount > 0) {
        
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.vertical_Input;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontal_Input;
            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true,true);
            player.playerLocomotionManager.isRolling = true;

        }
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);

        }

        player.playerNetWorkManager.currentStamina.Value -= dogeStaminaCost;
        player.playerNetWorkManager.DestroyAllCurrentActionFXServerRpc();
    }

    public void AttemptToPerformJump()
    {
        if (player.isPerformingAction)
            return;

        if (player.playerNetWorkManager.currentStamina.Value <= 0)
            return;

        if (player.characterNetWorkManager.isJumping.Value)
            return;

        if (!player.characterLocomotionManager.isGrounded)
            return;

        if (player.playerCombatManager.isUsingItem)
            return;

        if (player.playerNetWorkManager.isHodingArrow.Value)
            return;

        //IF WE ARE TWO HANDING OUR WEAPON, PLAY THE  TWO HANDED JUMP ANIMATION, OTHERWISE PLAY THE ONE HANDED ANIMATION (TO DO)
        player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);

        player.characterNetWorkManager.isJumping.Value = true;

        player.playerNetWorkManager.currentStamina.Value -= jumpStaminaCost;

        jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.vertical_Input;
        jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontal_Input;
        jumpDirection.y = 0;

        if(jumpDirection != Vector3.zero)
        {
            if (player.playerNetWorkManager.isSprinting.Value)
            {
                jumpDirection *= 1;
            }
            else if (PlayerInputManager.instance.moveAmount > 0.5)
            {
                jumpDirection *= 0.5f;

            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5)
            {
                jumpDirection *= 0.25f;
            }
        }
      
    }

    public void ApplyJumpingVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);  
    }
}
