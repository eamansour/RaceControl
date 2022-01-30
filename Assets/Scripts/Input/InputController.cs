using UnityEngine;

public class InputController : MonoBehaviour, IInputController
{
    private string _inputVerticalAxis = "Vertical";
    private string _inputHorizontalAxis = "Horizontal";

    public float VerticalInput { get => Input.GetAxis(_inputVerticalAxis); }
    public float HorizontalInput { get => Input.GetAxis(_inputHorizontalAxis); }
    public Vector3 MousePosition { get => Input.mousePosition; }
}
