using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera property configurations
    private float _speed = 40f;
    private float _xLimit = 50f;
    private float _zLimit = 70f;

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
        currentPosition.z += zInput * _speed * Time.deltaTime;
        currentPosition.x += xInput * _speed * Time.deltaTime;

        // Mouse panning
        if (mousePosition.y >= Screen.height) 
        {
            currentPosition.x += _speed * Time.deltaTime;
        }
        if (mousePosition.y <= 0)
        {
            currentPosition.x -= _speed * Time.deltaTime;
        }
        if (mousePosition.x >= Screen.width)
        {
            currentPosition.z -= _speed * Time.deltaTime;
        }
        if (mousePosition.x <= 0)
        {
            currentPosition.z += _speed * Time.deltaTime;
        }
        
        // Restrict camera movement to map
        currentPosition.x = Mathf.Clamp(currentPosition.x, -_xLimit, _xLimit);
        currentPosition.z = Mathf.Clamp(currentPosition.z, -_zLimit, _zLimit);
        
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
