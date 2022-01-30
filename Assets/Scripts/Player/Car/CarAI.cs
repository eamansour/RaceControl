using UnityEngine;

public class CarAI : MonoBehaviour, ICarAI
{
    private const float MaxSteerAngle = 45f;
    private const float DistanceThreshold = 10f;
    private const float FuelThreshold = 40f;

    [SerializeField]
    private bool _randomiseSkill = false;

    [SerializeField]
    private float _skillMultiplier = 1f;

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

        GameObject pitEntryObject = GameObject.Find("PitEntry");
        if (pitEntryObject)
        {
            _pitEntry = pitEntryObject.GetComponent<Checkpoint>();
        }
        _checkpointsLayer = LayerMask.NameToLayer("Checkpoints");

        // Randomise the AI player's skill level between 60% and 100%
        if (_randomiseSkill)
        {
            _skillMultiplier = Mathf.Clamp(Random.value, 0.6f, 1f);
        }
    }

    private void FixedUpdate()
    {
        if (_player.CurrentControl != PlayerManager.ControlMethod.AI) return;

        // Only allow the car to move when the player has started the level
        if (GameManager.Instance.LevelStarted)
        {
            _car.SteerDir = GetSteerDir();
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

    // Calculates and returns the appropriate steering direction based on target information
    private float GetSteerDir()
    {
        Vector3 directionToTarget = GetAvoidanceDirection((_targetPosition - transform.position).normalized);
        float targetAngle = Vector3.SignedAngle(-transform.forward, directionToTarget, Vector3.up);

        return Mathf.Clamp(targetAngle / MaxSteerAngle, -1f, 1f);
    }

    // Calculates and returns the appropriate acceleration based on target position
    // and current speed information
    private float GetAcceleration(float steerAmount)
    {
        Vector3 directionToTarget = (_targetPosition - transform.position).normalized;
        float targetDistance = Vector3.Distance(transform.position, _targetPosition);
        float scalar = Vector3.Dot(-transform.forward, directionToTarget);

        float acceleration = 0f;

        // Force the AI to brake if they are travelling faster than the max speed
        if (_car.GetSpeedInMPH() >= _maxSpeed)
        {
            return -1f;
        }

        if (scalar > 0f)
        {
            // When the target is in front of the car, accelerate
            acceleration = Mathf.Clamp(1f - Mathf.Abs(steerAmount), 0.1f, 1f);
        }
        else
        {
            // Prevent the AI from reversing endlessly
            if (targetDistance > DistanceThreshold)
            {
                acceleration = 1f;
            }
            else
            {
                // When the target is behind the car, reverse
                acceleration = Mathf.Clamp(-1f + Mathf.Abs(steerAmount), -1f, -0.1f);
            }
        }
        return acceleration;
    }

    // Sets the AI's next target position to a given checkpoint's position
    public void SetTarget(Checkpoint newTarget)
    {
        _targetPosition = newTarget.GetPosition();
    }

    // Updates the AI's target to an available pit stop
    public void GoToPit()
    {
        foreach (PitStop pit in GameManager.PitStops)
        {
            if (pit.IsFree)
            {
                _targetPosition = pit.transform.position;
                pit.IsFree = false;
                break;
            }
        }
    }

    // Updates the AI's next target when a checkpoint is triggered
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

    // Returns an opponent's Transform if they are directly ahead of this car, or null otherwise 
    private Transform DetectCarAhead()
    {
        // Prevent the AI from detecting itself
        _collider.enabled = false;

        float rayDistance = 5f;
        float rayRadius = 0.5f;
        int bitMask = 1 << LayerMask.NameToLayer("Car");

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, rayRadius, -transform.forward, rayDistance, bitMask);

        _collider.enabled = true;

        Debug.DrawRay(transform.position - transform.forward * rayDistance, transform.up, Color.blue);
        if (hits.Length > 0 && hits[0].collider)
        {
            Debug.DrawRay(transform.position, -transform.forward * rayDistance, Color.red);
            return hits[0].collider.transform;
        }

        Debug.DrawRay(transform.position, -transform.forward * rayDistance, Color.black);
        return null;
    }

    // Returns an adjusted direction vector to avoid an opponent ahead,
    // or the same direction if there is nobody ahead of this car
    private Vector3 GetAvoidanceDirection(Vector3 direction)
    {
        Transform carAhead = DetectCarAhead();
        if (!carAhead) return direction;

        // Make the AI desire to either avoid opponents or continue travelling to the target position
        float targetDistance = Vector3.Distance(transform.position, _targetPosition);
        float desireToStay = Mathf.Clamp((DistanceThreshold / 2f) / targetDistance, 0.25f, 1f);
        float desireToAvoid = 1f - desireToStay;

        Vector3 avoidVector = Vector3.Reflect((carAhead.position - transform.position).normalized, -carAhead.right);
        Vector3 newDirection = ((avoidVector.normalized * desireToAvoid) + (direction * desireToStay)).normalized;

        Debug.DrawRay(transform.position, avoidVector * 5, Color.green);
        Debug.DrawRay(transform.position, newDirection * 5, Color.yellow);

        return newDirection;
    }
}
