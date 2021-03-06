using UnityEngine;

public class RaceFlag : MonoBehaviour, IRaceFlag
{
    public enum FlagType { None, YellowFlag, RedFlag }

    [field: SerializeField]
    public FlagType Flag { get; private set; }

    // Rendering materials for the relevant race flags
    [SerializeField]
    private Material _redMaterial;

    [SerializeField]
    private Material _yellowMaterial;

    private Renderer _flagRenderer;

    [SerializeField]
    private bool _randomize = false;

    private SpeedObjective _maxSpeedObjective;
    private CheckpointObjective _avoidCheckpointObjective;

    private void Start()
    {
        _flagRenderer = GetComponent<Renderer>();
        _maxSpeedObjective = GetComponent<SpeedObjective>();
        _avoidCheckpointObjective = GetComponent<CheckpointObjective>();

        if (Flag == FlagType.YellowFlag)
        {
            SetYellowFlag();
        }
        else if (Flag == FlagType.RedFlag)
        {
            SetRedFlag();
        }
    }

    private void Update()
    {
        if (!GameManager.LevelStarted)
        {
            if (_randomize)
            {
                Flag = FlagType.None;
            }
            return;
        }

        if (_randomize && (Flag == FlagType.None))
        {
            // Randomise the flag
            if (Random.value < 0.5f)
            {
                SetYellowFlag();
            }
            else
            {
                SetRedFlag();
            }
        }

        if (Flag == FlagType.RedFlag)
        {
            _avoidCheckpointObjective.UpdateCompletion();
            if (_avoidCheckpointObjective.Failed && !GameManager.LevelEnded)
            {
                GameManager.LevelFail();
            }
        }
    }

    // Enforce yellow flag rules while a player is in the trigger area
    private void OnTriggerStay(Collider other)
    {
        GameObject collidedObject = other.gameObject;
        if (!collidedObject.CompareTag("Player")) return;

        if (Flag == FlagType.YellowFlag)
        {
            _maxSpeedObjective.UpdateCompletion();
            if (_maxSpeedObjective.Failed && !GameManager.LevelEnded)
            {
                GameManager.LevelFail();
            }
        }
    }

    // Sets the yellow flag state
    private void SetYellowFlag()
    {
        _flagRenderer.material = _yellowMaterial;
        Flag = FlagType.YellowFlag;
    }

    // Sets the red flag state
    private void SetRedFlag()
    {
        _flagRenderer.material = _redMaterial;
        Flag = FlagType.RedFlag;
    }
}
