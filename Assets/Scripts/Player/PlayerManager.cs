using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour, IPlayerManager
{
    // Properties to define the method of control for the player
    public enum ControlMethod { Human, Program, AI }

    public GameObject AttachedGameObject { get; private set; }
    public Checkpoint TargetCheckpoint { get; private set; }
    public Checkpoint LastCheckpoint { get; private set; }
    public Checkpoint RecentCheckpoint { get; private set; }
    public int CurrentLap { get; private set; } = 0;
    public ControlMethod CurrentControl { get; set; }
    public ICar PlayerCar { get; private set; }
    public bool IsRetiring { get; set; } = false;

    [SerializeField]
    private ControlMethod _levelControl = ControlMethod.Program;

    [SerializeField]
    private Checkpoint _startCheckpoint;
    private int _checkpointsLayer;

    private ICarAI _carAI;

    private Vector3 _initialPosition = Vector3.zero;
    private Quaternion _initialRotation;
    private IInputController _inputController;

    public void Construct(ICar car, ICarAI carAI, Checkpoint startCheckpoint)
    {
        PlayerCar = car;
        _carAI = carAI;
        _startCheckpoint = startCheckpoint;
        TargetCheckpoint = startCheckpoint;
        LastCheckpoint = startCheckpoint;
    }

    private void Awake()
    {
        AttachedGameObject = gameObject;
        
        PlayerCar = GetComponent<ICar>();
        CurrentControl = _levelControl;

        GameObject checkpoints = GameObject.Find("Checkpoints");
        if (!_startCheckpoint && checkpoints)
        {
            _startCheckpoint = checkpoints.transform.GetChild(0).GetComponent<Checkpoint>();
        }
        TargetCheckpoint = _startCheckpoint;
        LastCheckpoint = _startCheckpoint;
        RecentCheckpoint = _startCheckpoint;

        _checkpointsLayer = LayerMask.NameToLayer("Checkpoints");
        _carAI = GetComponent<ICarAI>();

        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }

    private void Start()
    {
        _inputController = GameManager.InputController;
    }

    private void Update()
    {
        // Link keyboard inputs to the player's car if a human has control
        if (CurrentControl == ControlMethod.Human)
        {
            PlayerCar.Acceleration = _inputController.VerticalInput;
            PlayerCar.SteerDir = _inputController.HorizontalInput;
        }
    }

    // Updates the player's race progress when a checkpoint is triggered
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _checkpointsLayer) return;

        Checkpoint collidedCheckpoint = other.GetComponent<Checkpoint>();
        RecentCheckpoint = collidedCheckpoint;

        // Transfer control to AI to drive to a pit stop
        if (collidedCheckpoint.name == "PitEntry"
            && (TargetCheckpoint.Next.IsStartFinish || LastCheckpoint.Next.IsStartFinish))
        {
            Console.Paused = true;
            CurrentControl = ControlMethod.AI;
            TargetCheckpoint = collidedCheckpoint.Next;
            _carAI.SetTarget(TargetCheckpoint);
            _carAI.GoToPit();
        }

        // Transfer control back to the player when the car exits the pit lane
        if (collidedCheckpoint.name == "PitExit" && gameObject.CompareTag("Player"))
        {
            if (CurrentControl == ControlMethod.AI)
            {
                PlayerCar.ResetControl();
                CurrentControl = _levelControl;
                Console.Paused = false;          
            }
        }

        // When the target checkpoint is triggered, update checkpoint/lap progress
        if (collidedCheckpoint == TargetCheckpoint)
        {
            if (TargetCheckpoint.IsStartFinish)
            {
                CurrentLap++;
            }
            LastCheckpoint = TargetCheckpoint;
            TargetCheckpoint = TargetCheckpoint.Next;
        }
    }

    // Returns the distance to the player's target checkpoint
    public float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, TargetCheckpoint.GetPosition());
    }

    // Resets the player and the level progress to their initial states
    public void ResetPlayer()
    {
        PlayerCar.SetCarLock(true);
        PlayerCar.ResetControl();
        PlayerCar.Fuel = 100f;
        Console.Paused = false;

        transform.position = _initialPosition;
        transform.rotation = _initialRotation;

        CurrentLap = 0;
        TargetCheckpoint = _startCheckpoint;
        LastCheckpoint = _startCheckpoint;
        RecentCheckpoint = _startCheckpoint;
        CurrentControl = _levelControl;

        _carAI.SetTarget(_startCheckpoint);
    }

    // Wrapper method to unlock the player's car's physics properties
    public void StartPlayer()
    {
        PlayerCar.SetCarLock(false);
    }

    // Retires the player from the race
    public void RetirePlayer()
    {
        PlayerCar.SetCarLock(true);
        PlayerCar.ResetControl();
        CurrentControl = _levelControl;
        Console.Paused = false;
    }

    // Returns the player's position in the race
    public int GetRacePosition()
    {
        List<IPlayerManager> players = GameManager.Players;
        return players.IndexOf(this) + 1;
    }

    // Updates the player's race progress
    public void SetRaceProgress(int currentLap, ControlMethod currentControl, Checkpoint targetCheckpoint)
    {
        CurrentLap = currentLap;
        CurrentControl = currentControl;
        TargetCheckpoint = targetCheckpoint;
        LastCheckpoint = targetCheckpoint;
        RecentCheckpoint = targetCheckpoint;
        _carAI.SetTarget(TargetCheckpoint);
    }
}
