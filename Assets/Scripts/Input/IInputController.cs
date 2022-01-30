using UnityEngine;

public interface IInputController
{
    float VerticalInput { get; }
    float HorizontalInput { get; }
    Vector3 MousePosition { get; }
}
