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

    private MaxSpeedObjective _maxSpeedObjective;
    private AvoidCheckpointObjective _avoidCheckpointObjective;


    // Initialise required fields
    private void Start()
    {
        _flagRenderer = GetComponent<Renderer>();
        _maxSpeedObjective = GetComponent<MaxSpeedObjective>();
        _avoidCheckpointObjective = GetComponent<AvoidCheckpointObjective>();

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
        if (!GameManager.Instance.LevelStarted)
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
            _avoidCheckpointObjective.CheckAvoidCheckpoint();
        }
    }

    // Enforce yellow flag rules while a player is in the trigger area
    private void OnTriggerStay(Collider other)
    {
        GameObject collidedObject = other.gameObject;
        if (!collidedObject.CompareTag("Player")) return;

        if (Flag == FlagType.YellowFlag)
        {
            _maxSpeedObjective.CheckMaxSpeed();
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
