using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float verticalClamp = 85f;
    [SerializeField] private Transform player;

    private float verticalRotation = 0f;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();
    }

    void Update()
    {
        // Follow player position (not parented, so we do this manually)
        transform.position = player.position;

        Vector2 mouseDelta = playerInput.LookInput;

        // Horizontal rotation (Y axis) - no limits, can spin 360°
        transform.Rotate(Vector3.up, mouseDelta.x * sensitivity, Space.World);

        // Vertical rotation (X axis) - clamped to prevent flipping
        verticalRotation -= mouseDelta.y * sensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);

        transform.localEulerAngles = new Vector3(
            verticalRotation,
            transform.localEulerAngles.y,
            0f
        );
    }
}