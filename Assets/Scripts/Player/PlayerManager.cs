using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour, IPlayerManager
{
    public GameObject AttachedGameObject { get; private set; }
    public Checkpoint TargetCheckpoint { get; private set; }
    public Checkpoint LastCheckpoint { get; private set; }
    public Checkpoint RecentCheckpoint { get; private set; }
    public int CurrentLap { get; private set; } = 0;
    public ControlMode CurrentControl { get; set; }
    public ICar PlayerCar { get; private set; }
    public bool IsRetiring { get; set; } = false;
    public float CurrentLapTime { get; private set; } = 0f;
    public float BestLapTime { get; private set; } = Mathf.Infinity;

    [SerializeField]
    private ControlMode _levelControl = ControlMode.Program;

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
        if (CurrentControl == ControlMode.Human)
        {
            PlayerCar.Acceleration = _inputController.VerticalInput;
            PlayerCar.SteerDir = _inputController.HorizontalInput;
        }
        CurrentLapTime += (GameManager.LevelStarted && !GameManager.LevelEnded) ? Time.deltaTime : 0f;
    }

    /// <summary>
    /// Updates the player's race progress and control behaviour when a checkpoint is triggered
    /// </summary>
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
            CurrentControl = ControlMode.AI;
            TargetCheckpoint = collidedCheckpoint.Next;
            _carAI.SetTarget(TargetCheckpoint);
            _carAI.GoToPit();
        }

        // Transfer control back to the player when the car exits the pit lane
        if (collidedCheckpoint.name == "PitExit" && gameObject.CompareTag("Player"))
        {
            if (CurrentControl == ControlMode.AI)
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
                NextLap();
            }
            LastCheckpoint = TargetCheckpoint;
            TargetCheckpoint = TargetCheckpoint.Next;
        }
    }

    /// <inheritdoc />
    public float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, TargetCheckpoint.GetPosition());
    }

    /// <inheritdoc />
    public void ResetPlayer()
    {
        PlayerCar.SetCarLock(true);
        PlayerCar.ResetControl();
        PlayerCar.Fuel = 100f;
        Console.Paused = false;

        transform.position = _initialPosition;
        transform.rotation = _initialRotation;

        CurrentLap = 0;
        CurrentLapTime = 0f;
        BestLapTime = Mathf.Infinity;

        TargetCheckpoint = _startCheckpoint;
        LastCheckpoint = _startCheckpoint;
        RecentCheckpoint = _startCheckpoint;
        CurrentControl = _levelControl;

        _carAI.SetTarget(_startCheckpoint);
    }

    /// <inheritdoc />
    public void StartPlayer()
    {
        PlayerCar.SetCarLock(false);
    }

    /// <inheritdoc />
    public void RetirePlayer()
    {
        PlayerCar.SetCarLock(true);
        PlayerCar.ResetControl();
        CurrentControl = _levelControl;
        Console.Paused = false;
    }

    /// <inheritdoc />
    public int GetRacePosition()
    {
        List<IPlayerManager> players = GameManager.Players;
        return players.IndexOf(this) + 1;
    }

    /// <inheritdoc />
    public void CopyRaceProgress(IPlayerManager otherPlayer)
    {
        CurrentLap = otherPlayer.CurrentLap;
        CurrentLapTime = otherPlayer.CurrentLapTime;
        BestLapTime = otherPlayer.BestLapTime;
        CurrentControl = otherPlayer.CurrentControl;
        
        TargetCheckpoint = otherPlayer.TargetCheckpoint;
        LastCheckpoint = otherPlayer.TargetCheckpoint;
        RecentCheckpoint = otherPlayer.TargetCheckpoint;

        _carAI.SetTarget(TargetCheckpoint);
    }

    /// <summary>
    /// Starts the player's next lap.
    /// </summary>
    private void NextLap()
    {
        if (CurrentLap != 0)
        {
            BestLapTime = Mathf.Min(BestLapTime, CurrentLapTime);
        }
        CurrentLapTime = 0f;
        CurrentLap++;
    }
}
