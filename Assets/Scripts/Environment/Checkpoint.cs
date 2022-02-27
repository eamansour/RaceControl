using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [field: SerializeField]
    public float MaxSpeed { get; private set; } = 0f;

    [field: SerializeField]
    public Checkpoint Next { get; private set; }

    [field: SerializeField]
    public bool IsStartFinish { get; private set; } = false;
    public int Index { get; private set; }

    private void Awake()
    {
        Index = transform.GetSiblingIndex();
    }

    /// <summary>
    /// Gets the checkpoint's current position.
    /// </summary>
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    /// <summary>
    /// Gets the checkpoint's current rotation.
    /// </summary>
    public Vector3 GetRotation()
    {
        return transform.localEulerAngles;
    }
}
