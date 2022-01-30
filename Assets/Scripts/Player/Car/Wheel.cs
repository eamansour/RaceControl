using UnityEngine;

public class Wheel : MonoBehaviour
{
    // Wheel-related properties, mapping to wheel collider properties
    public float SteeringAngle { get; set; }
    public float MotorTorque { get; set; }
    public float BrakeTorque { get; set; }

    [SerializeField]
    private bool _isSteering;
    private Transform _wheelTransform;
    private WheelCollider _wheelCollider;
    
    private void Start()
    {
        _wheelTransform = GetComponentInChildren<MeshRenderer>().transform;
        _wheelCollider = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        _wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        _wheelTransform.position = pos;
        _wheelTransform.rotation = rot;
    }

    private void FixedUpdate()
    {
        _wheelCollider.motorTorque = MotorTorque;
        _wheelCollider.brakeTorque = BrakeTorque;

        // Update steering angle of wheels used for steering
        if (_isSteering)
        {
            _wheelCollider.steerAngle = SteeringAngle;
        }
    }
}
