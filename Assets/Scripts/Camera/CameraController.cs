using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float Speed = 40f;
    private const float XLimit = 50f;
    private const float ZLimit = 70f;
    private const int BoundaryOffset = 5;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    private IInputController _inputController;

    public void Construct(IInputController inputController)
    {
        _inputController = inputController;
    }

    private void Start()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;

        if (_inputController == null || _inputController.Equals(null))
        {
            _inputController = GameManager.InputController;
        }
    }

    private void Update()
    {
        if (LevelMenu.IsPaused) return;

        Vector3 currentPosition = transform.position;
        Vector3 mousePosition = _inputController.MousePosition;

        // Keyboard panning
        float zInput = -_inputController.HorizontalInput;
        float xInput = _inputController.VerticalInput;
        currentPosition.z += zInput * Speed * Time.deltaTime;
        currentPosition.x += xInput * Speed * Time.deltaTime;

        // Mouse panning
        if (mousePosition.y >= Screen.height - BoundaryOffset)
        {
            currentPosition.x += Speed * Time.deltaTime;
        }
        if (mousePosition.y <= 0 + BoundaryOffset)
        {
            currentPosition.x -= Speed * Time.deltaTime;
        }
        if (mousePosition.x >= Screen.width - BoundaryOffset)
        {
            currentPosition.z -= Speed * Time.deltaTime;
        }
        if (mousePosition.x <= 0 + BoundaryOffset)
        {
            currentPosition.z += Speed * Time.deltaTime;
        }

        // Restrict camera movement to map
        currentPosition.x = Mathf.Clamp(currentPosition.x, -XLimit, XLimit);
        currentPosition.z = Mathf.Clamp(currentPosition.z, -ZLimit, ZLimit);

        transform.position = currentPosition;
    }

    /// <summary>
    /// Resets the camera to its initial position and rotation in the world.
    /// </summary>
    public void ResetCamera()
    {
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }
}
