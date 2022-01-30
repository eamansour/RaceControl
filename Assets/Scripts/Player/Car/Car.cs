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

    // Car-related constants
    private const float DefaultTorque = 100f;
    private const float Steering = 20f;
    private const float BrakeTorque = 100f;

    // Physics properties for a car
    [SerializeField]
    private Transform _centreOfMass;
    private Rigidbody _carRigidbody;

    private float _motorTorque = DefaultTorque;

    [SerializeField]
    private float _fuelBurnRate = 1f;
    private Wheel[] _wheels;

    // Constructor-like method to initialise serialized fields
    public void Construct(Transform centreOfMass)
    {
        _centreOfMass = centreOfMass;
    }

    // Retrieves the inital values for the car's physics components and wheels
    private void Start()
    {
        _carRigidbody = GetComponent<Rigidbody>();
        _carRigidbody.centerOfMass = _centreOfMass.localPosition;
        _carRigidbody.isKinematic = true;

        _wheels = GetComponentsInChildren<Wheel>();
    }

    // Updates the car's properties every frame
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

    // Sets the car to accelerate for a given amount of time (in seconds)
    public IEnumerator Accelerate(float time)
    {
        Acceleration = 1f;
        Braking = 0f;
        yield return new WaitForSeconds(time);
        Acceleration = 0f;
    }

    // Sets the car to brake for a given amount of time (in seconds)
    public IEnumerator Brake(float time)
    {
        Braking = 1f;
        Acceleration = 0f;
        yield return new WaitForSeconds(time);
        Braking = 0f;
    }

    // Sets the car to turn in a direction between 1 and -1 for a given amount of time (in seconds)
    public IEnumerator Turn(float dir, float time)
    {
        SteerDir = dir;
        yield return new WaitForSeconds(time);
        SteerDir = 0f;
    }

    // Locks or unlocks the car's physics properties, controlling its ability to move
    public void SetCarLock(bool locked)
    {
        _carRigidbody.isKinematic = locked;
        _carRigidbody.velocity = Vector3.zero;
    }

    // Resets the car's acceleration and steering controls
    public void ResetControl()
    {
        StopAllCoroutines();
        Acceleration = 0f;
        SteerDir = 0f;
        _motorTorque = DefaultTorque;
    }

    // Returns the car's speed in miles per hour (MPH)
    public int GetSpeedInMPH()
    {
        return (int)Math.Round(_carRigidbody.velocity.magnitude * 2.237f, 0);
    }

    // Returns an integer representation of the car's fuel property
    public int GetFuelInt()
    {
        return (int)Math.Round(Fuel, 0);
    }
}
