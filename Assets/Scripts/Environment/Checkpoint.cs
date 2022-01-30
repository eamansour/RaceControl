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

    // Returns the checkpoint's position
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    // Returns the checkpoint's rotation
    public Vector3 GetRotation()
    {
        return transform.localEulerAngles;
    }
}
