using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;

    [Header("Rotation Limits")]
    [SerializeField] private float minYRotation = -90f;
    [SerializeField] private float maxYRotation = 90f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate rotations
        yRotation += mouseX;
        xRotation -= mouseY;

        // Clamp vertical rotation to prevent flipping
        xRotation = Mathf.Clamp(xRotation, minYRotation, maxYRotation);

        // Apply rotation to camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
