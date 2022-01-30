using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Properties for the camera to follow a target
    [field: SerializeField]
    public GameObject Target { get; set; }

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private Vector3 _eulerRotation;

    [SerializeField]
    private float _offsetSmoothing;

    private void Start()
    {
        transform.eulerAngles = _eulerRotation;
    }

    // Continuously move the camera over time based on the target's position
    private void FixedUpdate()
    {
        transform.position =
            Vector3.Lerp(transform.position, Target.transform.position + _offset, _offsetSmoothing * Time.deltaTime);
    }
}
