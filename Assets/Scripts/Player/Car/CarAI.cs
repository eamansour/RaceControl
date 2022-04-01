using System;
using UnityEngine;

public class CarAI : MonoBehaviour, ICarAI
{
    private const float MaxSteerAngle = 45f;
    private const float DistanceThreshold = 1f;
    private const float FuelThreshold = 40f;
    private const float ReverseAcceleration = -0.5f;

    [SerializeField]
    private bool _randomiseSkill = false;

    [SerializeField]
    private float _skillMultiplier = 1f;

    private PitStop[] _pitStops;
    private ICar _car;
    private IPlayerManager _player;
    private float _maxSpeed = 100f;
    private Vector3 _targetPosition;

    private Checkpoint _pitEntry;
    private int _checkpointsLayer;

    private CapsuleCollider _collider;

    private void Start()
    {
        _player = GetComponent<IPlayerManager>();
        _collider = GetComponent<CapsuleCollider>();
        _car = _player.PlayerCar;
        _targetPosition = _player.TargetCheckpoint.GetPosition();

        GameObject pitStopsParent = GameObject.Find("PitStops");
        if (pitStopsParent)
        {
            _pitStops = pitStopsParent.GetComponentsInChildren<PitStop>();
        }

        GameObject pitEntryObject = GameObject.Find("PitEntry");
        if (pitEntryObject)
        {
            _pitEntry = pitEntryObject.GetComponent<Checkpoint>();
        }
        _checkpointsLayer = LayerMask.NameToLayer("Checkpoints");

        // Randomise the AI player's skill level between 60% and 100%
        if (_randomiseSkill)
        {
            _skillMultiplier = Mathf.Clamp(UnityEngine.Random.value, 0.6f, 1f);
        }
    }

    private void FixedUpdate()
    {
        if (_player.CurrentControl != ControlMode.AI) return;

        // Only allow the car to move when the player has started the level
        if (GameManager.LevelStarted)
        {
            _car.SteerDir = GetSteerDirection();
            _car.Acceleration = _car.InPit ? 0f : GetAcceleration(_car.SteerDir);

            // The car needs to pit
            if (_car.Fuel <= FuelThreshold || _player.IsRetiring)
            {
                Checkpoint targetCheckpoint = _player.TargetCheckpoint;
                if (targetCheckpoint.Next.IsStartFinish)
                {
                    _targetPosition = _pitEntry.GetPosition();
                }
            }
        }
        else
        {
            _car.Acceleration = 0f;
            _car.SteerDir = 0f;
        }
    }

    /// <summary>
    /// Calculates and returns the appropriate steering direction based on target information.
    /// </summary>
    private float GetSteerDirection()
    {
        Vector3 directionToTarget = GetAvoidanceDirection((_targetPosition - transform.position).normalized);
        float targetAngle = Vector3.SignedAngle(-transform.forward, directionToTarget, Vector3.up);
        float steering = Mathf.Clamp(targetAngle / MaxSteerAngle, -1f, 1f);

        // Invert steering when reversing
        float dotProduct = Vector3.Dot(-transform.forward, directionToTarget);
        if (dotProduct < 0f && _car.Acceleration < 0f)
        {
            steering = -steering;
        }

        return steering;
    }

    /// <summary>
    /// Calculates and returns the appropriate acceleration based on target position
    /// and current speed information.
    /// </summary>
    private float GetAcceleration(float steerAmount)
    {
        Vector3 directionToTarget = (_targetPosition - transform.position).normalized;
        float dotProduct = Vector3.Dot(-transform.forward, directionToTarget);

        float acceleration = 0f;

        // Force the AI to brake if they are travelling faster than the max speed
        if (_car.GetSpeedInMPH() >= _maxSpeed)
        {
            return -1f;
        }

        // If the target is in front of the car, accelerate - Otherwise, reverse
        if (Math.Round(dotProduct, 2) >= 0f)
        {
            float absoluteSteer = Mathf.Abs(steerAmount);
            acceleration = absoluteSteer > 0.5f && _car.GetSpeedInMPH() < 10
                ? 1f
                : Mathf.Clamp(1f - absoluteSteer, 0.1f, 1f);
        }
        else
        {
            acceleration = ReverseAcceleration;
        }
        return acceleration;
    }

    /// <inheritdoc />
    public void SetTarget(Checkpoint newTarget)
    {
        _targetPosition = newTarget.GetPosition();
    }

    /// <inheritdoc />
    public void GoToPit()
    {
        foreach (PitStop pit in _pitStops)
        {
            if (pit.IsFree)
            {
                _targetPosition = pit.transform.position;
                pit.IsFree = false;
                break;
            }
        }
    }

    /// <summary>
    /// Determine the AI's next target and maximum speed when a checkpoint is triggered.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _checkpointsLayer) return;

        Checkpoint checkpointHit = other.GetComponent<Checkpoint>();
        _maxSpeed = checkpointHit.MaxSpeed * _skillMultiplier;

        if (other.gameObject.name != "PitEntry")
        {
            _targetPosition = _player.TargetCheckpoint.GetPosition();
        }
        else if (_targetPosition == _pitEntry.GetPosition())
        {
            GoToPit();
        }
    }

    /// <summary>
    /// Detects if an opponent's car is ahead of the AI and returns the detected Transform,
    /// or null otherwise.
    /// </summary>
    private Transform DetectCarAhead()
    {
        // Prevent the AI from detecting itself
        _collider.enabled = false;

        float rayDistance = 5f;
        float rayRadius = 0.5f;
        int bitMask = 1 << LayerMask.NameToLayer("Car");

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, rayRadius, -transform.forward, rayDistance, bitMask);

        _collider.enabled = true;

        if (hits.Length > 0 && hits[0].collider)
        {
            return hits[0].collider.transform;
        }
        return null;
    }

    /// <summary>
    /// Returns an adjusted direction vector to avoid an opponent ahead,
    /// or the same direction if there is nobody ahead of this car.
    /// </summary>
    private Vector3 GetAvoidanceDirection(Vector3 direction)
    {
        Transform detectedCar = DetectCarAhead();
        if (!detectedCar) return direction;

        // Make the AI desire to either avoid opponents or continue travelling to the target position
        float targetDistance = Vector3.Distance(transform.position, _targetPosition);
        float desireToStay = Mathf.Clamp(DistanceThreshold / targetDistance, 0.25f, 0.75f);
        float desireToAvoid = 1f - desireToStay;

        Vector3 avoidVector = Vector3.Reflect((detectedCar.position - transform.position).normalized, -detectedCar.right);
        Vector3 newDirection = ((avoidVector.normalized * desireToAvoid) + (direction * desireToStay)).normalized;

        return newDirection;
    }
}
