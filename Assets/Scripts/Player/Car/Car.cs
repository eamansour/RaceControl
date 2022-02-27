using UnityEngine;
using System.Collections;
using System;

public class Car : MonoBehaviour, ICar
{
    public float Acceleration { get; set; } = 0f;
    public float SteerDir { get; set; } = 0f;
    public float Braking { get; private set; } = 0f;
    public float Fuel { get; set; } = 100f;
    public bool InPit { get; set; } = false;

    private const float DefaultTorque = 100f;
    private const float Steering = 20f;
    private const float BrakeTorque = 100f;

    [SerializeField]
    private Transform _centreOfMass;
    private Rigidbody _carRigidbody;

    private float _motorTorque = DefaultTorque;

    [SerializeField]
    private float _fuelBurnRate = 1f;
    private Wheel[] _wheels;

    public void Construct(Transform centreOfMass)
    {
        _centreOfMass = centreOfMass;
    }

    private void Start()
    {
        _carRigidbody = GetComponent<Rigidbody>();
        _carRigidbody.centerOfMass = _centreOfMass.localPosition;
        _carRigidbody.isKinematic = true;

        _wheels = GetComponentsInChildren<Wheel>();
    }

    private void Update()
    {
        // Simulate fuel usage by decreasing it when accelerating
        if (Acceleration > 0f && Fuel > 0f)
        {
            Fuel -= _fuelBurnRate * Time.deltaTime;
        }

        // Prevent the car from accelerating if there is no fuel left
        if (Fuel <= 0f)
        {
            _motorTorque = 0f;
        }

        foreach (Wheel wheel in _wheels)
        {
            wheel.MotorTorque = Acceleration * -_motorTorque;
            wheel.BrakeTorque = Braking * BrakeTorque;
            wheel.SteeringAngle = SteerDir * Steering;
        }
    }

    /// <inheritdoc />
    public IEnumerator Accelerate(float time)
    {
        Acceleration = 1f;
        Braking = 0f;
        yield return new WaitForSeconds(time);
        Acceleration = 0f;
    }

    /// <inheritdoc />
    public IEnumerator Brake(float time)
    {
        Braking = 1f;
        Acceleration = 0f;
        yield return new WaitForSeconds(time);
        Braking = 0f;
    }

    /// <inheritdoc />
    public IEnumerator Turn(float dir, float time)
    {
        SteerDir = dir;
        yield return new WaitForSeconds(time);
        SteerDir = 0f;
    }

    /// <inheritdoc />
    public void SetCarLock(bool locked)
    {
        _carRigidbody.isKinematic = locked;
        _carRigidbody.velocity = Vector3.zero;
    }

    /// <inheritdoc />
    public void ResetControl()
    {
        StopAllCoroutines();
        Acceleration = 0f;
        SteerDir = 0f;
        _motorTorque = DefaultTorque;
    }

    /// <inheritdoc />
    public int GetSpeedInMPH()
    {
        return (int)Math.Round(_carRigidbody.velocity.magnitude * 2.237f, 0);
    }

    /// <inheritdoc />
    public int GetFuelInt()
    {
        return (int)Math.Round(Fuel, 0);
    }
}
