using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager player;
    public Camera cameraObject;

    public Transform cameraPivotTranform;
    public float cameraPivotYPositionOffset = 1.5f;

    [Header("Camera Settings")]
    [SerializeField]private float cameraSmoothSpeed = 1f;
    [SerializeField] float upAndDownRotationSpeed = 50;
    [SerializeField] float leftAndRightRotationSpeed = 50;
    [SerializeField] float minimumPivot = -30;
    [SerializeField] float maximumPivot = 60;
    [SerializeField] float cameraCollisionRadius = .2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;
    private float targetCameraZPosition;

    [Header("Lock On")]
    [SerializeField] float lockOnRadius = 20;
    [SerializeField] float minimunViewableAngle = -50;
    [SerializeField] float maximunViewableAngle = 50;
    [SerializeField] float lockOnTargetFollowSpeed = .2f;
    [SerializeField] float setCameraHeightSpeed = 1;
    [SerializeField] float unLockedCameraHeight = 1.65f;
    [SerializeField] float lockedCameraHeight = 2.0f;
    private Coroutine cameraLockOnHeightCoroutine;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockOnTarget;
    public CharacterManager rightLockOnTarget;


    [Header("Ranged Aim")]
    private Transform followTransformWhenAiming;
    public Vector3 aimDirection;

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

        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraAction()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotation();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        if (player.playerNetWorkManager.isAiming.Value)
        {
            Vector3 targetCameraPositon = Vector3.SmoothDamp(transform.position, player.playerCombatManager.lockOnTranfrom.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPositon;
        }
        else
        {
            Vector3 targetCameraPositon = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPositon;
        }
    }

    private void HandleRotation()
    {
        //THIS ROTATES THE GAME OBJECT

        if (player.playerNetWorkManager.isAiming.Value)
        {
            HandleAimRotation();
        }
        else
        {
            HandleStandardRotations();
        }

    }

    private void HandleAimRotation()
    {
        if (!player.playerLocomotionManager.isGrounded)
            player.playerNetWorkManager.isAiming.Value = false;

        if (player.isPerformingAction)
            return;

        aimDirection = cameraObject.transform.forward.normalized;

        Vector3 cameraRotationY = Vector3.zero;
        Vector3 cameraRotationX = Vector3.zero;

        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle,minimumPivot , maximumPivot);

        cameraRotationY.y= leftAndRightLookAngle;
        cameraRotationX.x = upAndDownLookAngle;

        cameraObject.transform.localEulerAngles = new Vector3(upAndDownLookAngle, leftAndRightLookAngle, 0);
    }

    private void HandleStandardRotations()
    {
        if (player.playerNetWorkManager.isLockedOn.Value)
        {
            Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTranfrom.position - transform.position;
            rotationDirection.Normalize();
            rotationDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

            //THIS ROTATES THE PIVOT OBJECT
            rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTranfrom.position - cameraPivotTranform.transform.position;
            rotationDirection.Normalize();

            targetRotation = Quaternion.LookRotation(rotationDirection);
            cameraPivotTranform.transform.rotation = Quaternion.Lerp(cameraPivotTranform.transform.rotation, targetRotation, lockOnTargetFollowSpeed);

            leftAndRightLookAngle = transform.eulerAngles.y;
            upAndDownLookAngle = transform.eulerAngles.x;
        }
        else
        {
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTranform.localRotation = targetRotation;
        }
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTranform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTranform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTranform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);

        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        if (player.playerNetWorkManager.isAiming.Value)
        {
            cameraObjectPosition.z =0;
            cameraObject.transform.localPosition = cameraObjectPosition;
            return;
        }
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, .2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

    public void HandleLocatingLockOnTargets()
    {
        float shortDistance = Mathf.Infinity;
        float shortDistanceOfRightTarget = Mathf.Infinity;
        float shortDistanceOfLeftTarget = -Mathf.Infinity;


        //
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.instance.GetCharacterLayer());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

            if (lockOnTarget != null)
            {
                Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                if (lockOnTarget.isDead.Value)
                    continue;

                if (lockOnTarget.transform.root == player.transform.root)
                    continue;


                if (viewableAngle > minimunViewableAngle && viewableAngle < maximunViewableAngle)
                {
                    RaycastHit hit;


                    if (Physics.Linecast(player.playerCombatManager.lockOnTranfrom.position, lockOnTarget.characterCombatManager.lockOnTranfrom.position,
                        out hit,
                        WorldUtilityManager.instance.GetEnviroLayer()))
                    {
                        continue;
                    }
                    else
                    {
                        availableTargets.Add(lockOnTarget);

                    }
                }

            }
        }
        for (int k = 0; k < availableTargets.Count; k++)
        {
            if (availableTargets[k] != null)
            {
                float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);

                if (distanceFromTarget < shortDistance)
                {
                    shortDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }
                // IF WE ARE ALREADY LOCKED ON WHEN SEARCHING FOR TARGETS, SEARCH FOR OUR NEAREST LEFT/RIGHT TARGETS
                if (player.playerNetWorkManager.isLockedOn.Value)
                {
                    // Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    Vector3 relativeEnemyPosition = transform.InverseTransformPoint(availableTargets[k].transform.position);

                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;


                    if (availableTargets[k] == player.playerCombatManager.currentTarget)
                        continue;

                    //CHECK THE LEFT SIDE
                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortDistanceOfLeftTarget)
                    {
                        shortDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockOnTarget = availableTargets[k];
                    }
                    //CHECK THE RIGHT SIDE
                    else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortDistanceOfRightTarget)
                    {
                        shortDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockOnTarget = availableTargets[k];
                    }
                }
            }
            else
            {
                ClearLockOnTarget();
                player.playerNetWorkManager.isLockedOn.Value = false;
            }
        }

    }

    public void SetLockCameraHeight()
    {
        if (cameraLockOnHeightCoroutine != null)
        {
            StopCoroutine(cameraLockOnHeightCoroutine);
        }
        cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
    }

    public void ClearLockOnTarget()
    {
        nearestLockOnTarget = null;
        leftLockOnTarget = null;
        rightLockOnTarget = null;
        availableTargets.Clear();
    }

    public IEnumerator WaitThenFindNewTarget()
    {
        while (player.isPerformingAction )
        {
            yield return null;
        }

        ClearLockOnTarget ();
        HandleLocatingLockOnTargets();

        if(nearestLockOnTarget != null )
        {
            player.playerCombatManager.SetTarget(nearestLockOnTarget);
            player.playerNetWorkManager.isLockedOn.Value =true;
        }

        yield return null;

    }

    public IEnumerator SetCameraHeight()
    {
        float duration = 1;
        float timer = 0;

        Vector3 velocity = Vector3.zero;
        Vector3 newLockedCameraHeight = new Vector3(cameraPivotTranform.transform.localPosition.x, lockedCameraHeight);
        Vector3 newUnlockedCameraHeight = new Vector3(cameraPivotTranform.transform.localPosition.x, unLockedCameraHeight);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTranform.transform.localPosition =
                        Vector3.SmoothDamp(cameraPivotTranform.transform.localPosition, newLockedCameraHeight, ref velocity, setCameraHeightSpeed);

                    cameraPivotTranform.transform.localRotation =
                        Quaternion.Slerp(cameraPivotTranform.transform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);
                }
                else
                {
                    cameraPivotTranform.transform.localPosition =
                        Vector3.SmoothDamp(cameraPivotTranform.transform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);
                }
            }

            yield return null;
        }

        if (player != null)
        {
            if (player.playerCombatManager.currentTarget != null)
            {
                cameraPivotTranform.transform.localPosition = newLockedCameraHeight;
                cameraPivotTranform.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                cameraPivotTranform.transform.localPosition = newUnlockedCameraHeight;
            }
        }

        yield return null;

    }

}
