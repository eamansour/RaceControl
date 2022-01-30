using UnityEngine;
using System.Collections.Generic;

public interface IGameManager
{
    List<IPlayerManager> Players { get; }
    IInputController InputController { get; }
    bool IsSuccess { get; }
    bool IsFail { get; }
    bool LevelStarted { get; set; }

    void LevelFail();
    void LevelWin();
    void ResetLevel();
    void StartLevel();
}

public class GameManager : MonoBehaviour, IGameManager
{
    public static GameManager Instance { get; private set; }
    public static PitStop[] PitStops { get; private set; }

    public List<IPlayerManager> Players { get; private set; } = new List<IPlayerManager>();
    public IInputController InputController { get; private set; }

    public bool IsSuccess { get; private set; }
    public bool IsFail { get; private set; }
    public bool LevelStarted { get; set; } = false;

    [SerializeField]
    private LevelMenu _levelMenu;

    [SerializeField]
    private bool _sortPlayers = true;

    private Objective[] _objectives;
    private int _remainingObjectives = 0;

    private IPlayerManager[] _startPlayers;

    private float _updateTimer = 0.5f;

    private void Awake()
    {
        Instance = this;
        InputController = GetComponent<IInputController>();

        Time.timeScale = 1f;

        _objectives = GetComponents<Objective>();
        _remainingObjectives = _objectives.Length;
        IsSuccess = false;
        IsFail = false;

        GameObject pitStopsParent = GameObject.Find("PitStops");
        if (pitStopsParent)
        {
            PitStops = pitStopsParent.GetComponentsInChildren<PitStop>();
        }

        _startPlayers = FindObjectsOfType<PlayerManager>();
        Players.AddRange(_startPlayers);
    }

    // Runs every frame, checks if the objectives have been met once the level has been started
    private void Update()
    {
        if (LevelStarted && !IsSuccess && !IsFail)
        {
            _updateTimer -= Time.deltaTime;

            if (_objectives.Length > 0 && _remainingObjectives <= 0)
            {
                LevelWin();
                return;
            }

            if (_updateTimer <= 0f)
            {
                if (_sortPlayers)
                {
                    UpdatePlayerPositions();
                }

                foreach (Objective obj in _objectives)
                {
                    if (obj.Failed)
                    {
                        LevelFail();
                        break;
                    }

                    if (!obj.Passed && obj.IsComplete())
                    {
                        obj.Passed = true;
                        obj.UpdateUI(true);
                        _remainingObjectives--;
                    }
                }
                _updateTimer = 0.5f;
            }
        }
    }

    // Updates player race positions
    private void UpdatePlayerPositions()
    {
        Players.Sort((left, right) =>
        {

            // Players with more completed laps are ahead of those with fewer completed laps
            if (left.CurrentLap != right.CurrentLap)
            {
                return right.CurrentLap.CompareTo(left.CurrentLap);
            }

            // Players that have passed more checkpoints are ahead of those with fewer checkpoints passed
            if (left.TargetCheckpoint != right.TargetCheckpoint)
            {
                return right.LastCheckpoint.Index.CompareTo(left.LastCheckpoint.Index);
            }

            // Players on the same lap travelling to the same checkpoint are positioned by distance
            return left.DistanceToTarget().CompareTo(right.DistanceToTarget());
        });
    }

    // Ends the level as a success and unlocks the next level
    public void LevelWin()
    {
        IsSuccess = true;
        PlayerPrefs.SetInt(LevelSelect.UnlockedSceneKey, MenuManager.GetActiveSceneIndex() + 1);
        _levelMenu.DisplayLevelSuccess();
    }

    // Ends the level as a fail
    public void LevelFail()
    {
        IsFail = true;
        _levelMenu.DisplayLevelFail();
    }

    // Starts the level
    public void StartLevel()
    {
        LevelStarted = true;
        foreach (IPlayerManager player in Players)
        {
            player.StartPlayer();
        }
    }

    // Resets the level to its initial state
    public void ResetLevel()
    {
        LevelStarted = false;
        _remainingObjectives = _objectives.Length;

        foreach (IPlayerManager player in Players)
        {
            player.ResetPlayer();
        }

        Players.Clear();
        Players.AddRange(_startPlayers);

        foreach (Objective objective in _objectives)
        {
            objective.Reset();
        }
    }
}
