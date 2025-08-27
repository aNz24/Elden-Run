using UnityEngine;

public class TitleMenuScreenPreviewRotator : MonoBehaviour
{
    PlayerControls playerControl;

    [Header("Camera Input")]
    [SerializeField] private Vector2 cameraInput;
    [SerializeField] private float horizontalInput;

    [Header("Rotation")]
    [SerializeField] private float lookAngle;
    [SerializeField] private float rotationSpeed = 50f;

    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;

    //private void OnEnable()
    //{
    //    if (playerControl == null)
    //    {
    //        playerControl = new PlayerControls();
    //        playerControl.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
    //    }

    //    playerControl.Enable();
    //}

    //private void OnDisable()
    //{
    //    playerControl.Disable();
    //}

    private void Update()
    {
        horizontalInput = cameraInput.x;

        if (isRotatingLeft)
            lookAngle -= rotationSpeed * Time.deltaTime;

        if (isRotatingRight)
            lookAngle += rotationSpeed * Time.deltaTime;

        lookAngle += (horizontalInput * rotationSpeed) * Time.deltaTime;

        Vector3 cameraRotation = Vector3.zero;
        cameraRotation.y = lookAngle;
        transform.rotation = Quaternion.Euler(cameraRotation);
    }

    public void StartRotateLeft() => isRotatingLeft = true;
    public void StopRotateLeft() => isRotatingLeft = false;

    public void StartRotateRight() => isRotatingRight = true;
    public void StopRotateRight() => isRotatingRight = false;
}
