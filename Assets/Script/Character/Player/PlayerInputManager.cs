    using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{

    public static PlayerInputManager instance;
    public PlayerManager player;


    PlayerControls playerControls;

    [Header("Camera Movement Input")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("lock On Input")]
    [SerializeField] bool lockOn_Input;
    [SerializeField] bool lockOn_Left_Input;
    [SerializeField] bool lockOn_Right_Input;
    private Coroutine lockOnCoroutine;

    [Header("Player Movement Input")]
    [SerializeField] Vector2 movementInput;
    public float vertical_Input;
    public float horizontal_Input;
    public float moveAmount;

    [Header("Player Actions Input")]
    [SerializeField] bool dogeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool switch_Right_Weapon_Input = false;
    [SerializeField] bool switch_Left_Weapon_Input = false;
    [SerializeField] bool switch_Quick_Slot_Item_Input = false;
    [SerializeField] bool interaction_Input = false;
    [SerializeField] bool use_Item_Input = false;

    [Header("Bumper Inputs")]
    [SerializeField] bool RB_Input = false;
    [SerializeField] bool hold_RB_Input = false;
    [SerializeField] bool LB_Input = false;
    [SerializeField] bool hold_LB_Input = false;


    [Header("Two Hand Inputs")]
    [SerializeField] bool two_Hand_Input = false;
    [SerializeField] bool two_Hand_Right_WP_Input = false;
    [SerializeField] bool two_Hand_Left_WP_Input = false;

    [Header("Qued Inputs")]
    [SerializeField] private bool input_Qued_IsActive  = false;
    [SerializeField] float default_Que_Input_Time = 0.35f;
    [SerializeField] float que_Input_Timer = 0;
    [SerializeField] bool que_RB_Input = false;
    [SerializeField] bool que_RT_Input = false;

    [Header("Trigger Inputs")]
    [SerializeField] bool RT_Input = false;
    [SerializeField] bool hold_RT_Input = false;
    [SerializeField] bool LT_Input = false;

    [Header("UI INPUTS")]
    [SerializeField] bool openCharacterMenuInput = false;
    [SerializeField] bool closeMenuInput = false;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {

        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;

        if (playerControls != null)
        {
            playerControls.Disable();

        }

    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
     


        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;

            if (playerControls != null)
            {
                playerControls.Enable();

            }

            Cursor.lockState = CursorLockMode.Locked;

        }
        else
        {
            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }

            Cursor.lockState = CursorLockMode.None;

        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

            //ACTION
            playerControls.PlayerActions.Doge.performed += i => dogeInput = true;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
            playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;
            playerControls.PlayerActions.SwitchQuickSlot.performed += i => switch_Quick_Slot_Item_Input = true;
            playerControls.PlayerActions.Interact.performed += i => interaction_Input = true;
            playerControls.PlayerActions.X.performed += i => use_Item_Input = true;




            //BUMBER
            playerControls.PlayerActions.RB.performed += i => RB_Input = true;
            playerControls.PlayerActions.LB.performed += i => LB_Input = true;
            playerControls.PlayerActions.LB.canceled += i => player.playerNetWorkManager.isBlocking.Value =false;
            playerControls.PlayerActions.LB.canceled += i => player.playerNetWorkManager.isAiming.Value =false;
            playerControls.PlayerActions.HoldRB.performed += i => hold_RB_Input = true;
            playerControls.PlayerActions.HoldRB.canceled += i => hold_RB_Input = false;
            //playerControls.PlayerActions.HoldLB.performed += i => hold_LB_Input = true;
            //playerControls.PlayerActions.HoldLB.canceled += i => hold_LB_Input = false;

            //TRIGGER
            playerControls.PlayerActions.RT.performed += i => RT_Input = true;

            playerControls.PlayerActions.HoldRT.performed += i => hold_RT_Input = true;
            playerControls.PlayerActions.HoldRT.canceled += i => hold_RT_Input = false;

            playerControls.PlayerActions.HoldRT1.performed += i => hold_RT_Input = true;
            playerControls.PlayerActions.HoldRT1.canceled += i => hold_RT_Input = false;

            playerControls.PlayerActions.LT.performed += i => LT_Input = true;


            // TWO HAND 
            playerControls.PlayerActions.TwoHandWeapon.performed += i => two_Hand_Input = true;
            playerControls.PlayerActions.TwoHandWeapon.canceled += i => two_Hand_Input = false;

            playerControls.PlayerActions.TwoHandRightWeapon.performed += i => two_Hand_Right_WP_Input = true;
            playerControls.PlayerActions.TwoHandRightWeapon.canceled += i => two_Hand_Right_WP_Input = false;

            playerControls.PlayerActions.TwoHandLeftWeapon.performed += i => two_Hand_Left_WP_Input = true;
            playerControls.PlayerActions.TwoHandLeftWeapon.canceled += i => two_Hand_Left_WP_Input = false;



            // LOCK ON INPUT
            playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
            playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
            playerControls.PlayerActions.SeekRIghtLockOnTarget.performed += i => lockOn_Right_Input = true;


            //HODING THE INPUT, SETS THE BOOL TO TRUE
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            //RELEASING THE INPUT, SETS THE BOOL TO FALE
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            // QUED INPUTS
            playerControls.PlayerActions.QueRB.performed += i => QueInput(ref que_RB_Input);
            playerControls.PlayerActions.QuedRT.performed += i => QueInput(ref que_RT_Input);

            // UI INPUTS
            playerControls.PlayerActions.Doge.performed += i => closeMenuInput = true;
            playerControls.PlayerActions.OpenCharacterMenu.performed += i => openCharacterMenuInput = true;

        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();

            }
            else
            {
                playerControls.Disable();

            }
        }
    }

    private void Update()
    {
        HandleAllInput();
    }

    private void HandleAllInput()
    {
        HandleUseItemInput();
        HandleTwoHandInput();
        HandleLockOnInput();
        HandleLockOnOnSwitchTargetInput();
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDogeInput();
        HandleSprintInput();
        HandleJumpInput();  
        HandleRBInput();
        HandleHoldRBInput();
        HandleLBInput();
        HandleRTInput();
        HandleChargeRTInput();
        HandleLTInput();
        HandleSwitchRightWeaponInput();
        HandleSwitchLeftWeaponInput();
        HandleSwitchQuickSlotItemInput();
        HadnleQuedInputs();
        HandleInteractionInput();
        HandleCloseUIInput();
        HandleOpenCharacterMenuInput();
    }
     
    private void HandleTwoHandInput()
    {
        if (!two_Hand_Input) return;

        if (two_Hand_Right_WP_Input)
        {
            RB_Input = false;
            two_Hand_Right_WP_Input = false;
            player.playerNetWorkManager.isBlocking.Value = false;

            if (player.playerNetWorkManager.isTwoHandingWeapon.Value)
            {
                player.playerNetWorkManager.isTwoHandingWeapon.Value = false;
                return;
            }
            else
            {
                player.playerNetWorkManager.isTwoHandingRightWeapon.Value = true;
                return;
            }
        }
        else if (two_Hand_Left_WP_Input)
        {
            LB_Input = false;
            two_Hand_Left_WP_Input = false;
            player.playerNetWorkManager.isBlocking.Value = false;

            if (player.playerNetWorkManager.isTwoHandingWeapon.Value)
            {
                player.playerNetWorkManager.isTwoHandingWeapon.Value = false;
                return;
            }
            else
            {
                player.playerNetWorkManager.isTwoHandingLeftWeapon.Value = true;
                return;
            }
        }

    }

    //Lock On
    private void HandleLockOnInput()
    {
        if(player.playerNetWorkManager.isLockedOn.Value)
        {
            if(player.playerCombatManager.currentTarget == null)
                return;

            if (player.playerCombatManager.currentTarget.isDead.Value)
            {
                player.playerNetWorkManager.isLockedOn.Value = false;
            }

            if (lockOnCoroutine != null)
                StopCoroutine(lockOnCoroutine);

            lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());

        }


        if (lockOn_Input && player.playerNetWorkManager.isLockedOn.Value)
        {
            lockOn_Input = false;
            PlayerCamera.instance.ClearLockOnTarget();
            player.playerNetWorkManager.isLockedOn.Value = false;

            return;
        }

        if (lockOn_Input && !player.playerNetWorkManager.isLockedOn.Value)
        {
            lockOn_Input = false;

            PlayerCamera.instance.HandleLocatingLockOnTargets();

            if(PlayerCamera.instance.nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.playerNetWorkManager.isLockedOn.Value =true;
            }

        }
    }

    //USE ITEM
    private void HandleUseItemInput()
    {
        if (use_Item_Input)
        {
            use_Item_Input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (player.playerInventoryManager.currentQuickSlotItem != null)
            {
                player.playerInventoryManager.currentQuickSlotItem.AttemptToUseItem(player);

                player.playerNetWorkManager.NotifyServerOfQuickSlotItemActionServerRpc
                    (NetworkManager.Singleton.LocalClientId , player.playerInventoryManager.currentQuickSlotItem.itemID);
            }
        }
    }

    private void HandleLockOnOnSwitchTargetInput()
    {
        if (lockOn_Left_Input)
        {
            lockOn_Left_Input = false;

            if (player.playerNetWorkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                }
            }
        }

        if (lockOn_Right_Input)
        {
            lockOn_Right_Input = false;

            if (player.playerNetWorkManager.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.rightLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                }
            }
        }
    }

    //Movement
    private void HandlePlayerMovementInput()
    {
        vertical_Input = movementInput.y;
        horizontal_Input = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(vertical_Input) + Mathf.Abs(horizontal_Input));

        if(moveAmount <= 0.5 &&  moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        if (player == null)
            return;

        if (moveAmount != 0)
        {
            player.playerNetWorkManager.isMoving.Value = true;
        }
        else
        {
            player.playerNetWorkManager.isMoving.Value = false;
        }

        if (!player.playerLocomotionManager.canRun)
        {
            if (moveAmount > .5f)
                moveAmount = .5f;

            if (vertical_Input > .5f)
                vertical_Input = .5f;

            if (horizontal_Input > .5f)
                horizontal_Input = .5f;
        }

        if (player.playerNetWorkManager.isLockedOn.Value && !player.playerNetWorkManager.isSprinting.Value)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontal_Input, vertical_Input, player.playerNetWorkManager.isSprinting.Value);
            return; 
        }

        if (player.playerNetWorkManager.isAiming.Value)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontal_Input, vertical_Input, player.playerNetWorkManager.isSprinting.Value);
            return;
        }


        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetWorkManager.isSprinting.Value);

    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    //Actions
    private void HandleDogeInput()
    {
        if (dogeInput)
        {
            dogeInput = false;

            if(PlayerUIManager.instance.menuWindowIsOpen) 
                return;

            player.playerLocomotionManager.AttemptToPerfromDoge();
        }
    }

    private void HandleSprintInput()
    {
        if (player == null || player.playerNetWorkManager == null)
        {
            return;
        }

        if (sprintInput)
        {
            player.playerLocomotionManager.HandleSrinting();
        }
        else
        {
            player.playerNetWorkManager.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            // IF HAVE A UI WINDOW OPEN, SIMPLY RETURN WITHOUT DOING ANYTHING

            // ATTEMP TO PERFROM JUMP
            player.playerLocomotionManager.AttemptToPerformJump();
        }
    }

    private void HandleRBInput()
    {
        if (two_Hand_Input)
            return;
        if (RB_Input)
        {
            RB_Input = false;


            player.playerNetWorkManager.SetCharacterActionHand(true);

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);

        }
    }

    private void HandleHoldRBInput()
    {
        if (hold_RB_Input)
        {
            player.playerNetWorkManager.isHodingArrow.Value = true;
        }
        else
        {
            player.playerNetWorkManager.isHodingArrow.Value = false;
        }
    }

    private void HandleLBInput()
    {
        if (two_Hand_Input)
            return;
        if (LB_Input)
        {
            LB_Input = false;


            player.playerNetWorkManager.SetCharacterActionHand(false);

            if (player.playerNetWorkManager.isTwoHandingRightWeapon.Value)
            {
                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_LB_Action, player.playerInventoryManager.currentRightHandWeapon);

            }
            else
            {
                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oh_LB_Action, player.playerInventoryManager.currentLeftHandWeapon);

            }


        }
    }

    private void HandleHoldLBInput()
    {
        if (hold_LB_Input)
        {

        }
        else
        {

        }
    }

    private void HandleRTInput()
    {
        if (RT_Input)
        {
            RT_Input = false;


            player.playerNetWorkManager.SetCharacterActionHand(true);

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action, player.playerInventoryManager.currentRightHandWeapon);

        }
    }

    private void HandleLTInput()
    {
        if (LT_Input)
        {
            LT_Input = false;

            WeaponItem weaponPerformingAction = player.playerCombatManager.SelectWeaponToPerformAshOfWar();

            weaponPerformingAction.ashOfWarAction.AttempToPerformAction(player);

        }
    }

    private void HandleChargeRTInput()
    {
        if (player.isPerformingAction)
        {
            if (player.playerNetWorkManager.isUsingRightHand.Value)
            {
               player.playerNetWorkManager.isChargingAttack.Value = hold_RT_Input;
            }
        }
    }

    private void HandleSwitchRightWeaponInput()
    {
        if (switch_Right_Weapon_Input)
        {
            switch_Right_Weapon_Input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (player.isPerformingAction)
                return;

            if(player.playerCombatManager.isUsingItem)
                return;

            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    private void HandleSwitchLeftWeaponInput()
    {
        if (switch_Left_Weapon_Input)
        {
            switch_Left_Weapon_Input = false;
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerEquipmentManager.SwitchLeftWeapon();
        }
    }

    private void HandleSwitchQuickSlotItemInput()
    {
        if (switch_Quick_Slot_Item_Input)
        {
            switch_Quick_Slot_Item_Input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            player.playerEquipmentManager.SwitchQuickSlotItem();
        }
    }

    private void HandleInteractionInput()
    {
        if (interaction_Input)
        {
            interaction_Input = false;

            player.playerInteractionManager.Interact();
        }
    }

    private void QueInput(ref bool quedInput)
    {
        que_RB_Input = false;
        que_RT_Input = false;
       // que_LB_Input = false;
       // que_LT_Input = false;

        if(player.isPerformingAction || player.playerNetWorkManager.isJumping.Value)
        {
            quedInput = true;   
            que_Input_Timer = default_Que_Input_Time;
            input_Qued_IsActive = true;
        }
    }

    private void ProcessQuedInput()
    {
        if(player.isDead.Value) 
            return;

        if(que_RB_Input)
            RB_Input = true;

        if (que_RT_Input)
            RT_Input = true;
    }

    private void HadnleQuedInputs()
    {
        if (input_Qued_IsActive)
        {
            if (que_Input_Timer > 0)
            {
                que_Input_Timer -= Time.deltaTime;
                ProcessQuedInput();
            }
            else
            {
                que_RB_Input =false; 
                que_RT_Input =false; 
                
                input_Qued_IsActive=false;
                que_Input_Timer =0;
            }
        }
    }

    private void HandleOpenCharacterMenuInput()
    {
        if (openCharacterMenuInput)
        {
            openCharacterMenuInput = false;

            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            PlayerUIManager.instance.CloseAllMenuWindow();
            PlayerUIManager.instance.playerUICharacterMenuManager.OpenMenu();
            
        }
    }

    private void HandleCloseUIInput()
    {
        if (closeMenuInput)
        {
            closeMenuInput = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
            {
                PlayerUIManager.instance.CloseAllMenuWindow();
            }
        }
    }
}
