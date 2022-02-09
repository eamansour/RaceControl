using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static List<IPlayerManager> Players { get; private set; } = new List<IPlayerManager>();
    public static IInputController InputController { get; private set; }

    public static bool LevelEnded { get; private set; } = false;
    public static bool LevelStarted { get; private set; } = false;

    private static LevelMenu s_levelMenu;
    private static IPlayerManager[] s_startPlayers;
    private static Objective[] s_objectives;
    private static int s_remainingObjectives = 0;

    [SerializeField]
    private LevelMenu _levelMenu;

    [SerializeField]
    private bool _sortPlayers = true;

    private float _updateTimer = 0.5f;

    private void Awake()
    {
        Time.timeScale = 1f;

        Players = new List<IPlayerManager>();
        InputController = GetComponent<IInputController>();

        LevelStarted = false;
        LevelEnded = false;

        s_levelMenu = _levelMenu;

        s_objectives = GetComponents<Objective>();
        s_remainingObjectives = s_objectives.Length;

        s_startPlayers = FindObjectsOfType<PlayerManager>();
        Players.AddRange(s_startPlayers);
    }

    // Runs every frame, checks if the objectives have been met once the level has been started
    private void Update()
    {
        if (LevelStarted && !LevelEnded)
        {
            _updateTimer -= Time.deltaTime;

            if (s_objectives.Length > 0 && s_remainingObjectives <= 0)
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

                foreach (Objective objective in s_objectives)
                {
                    if (objective.Failed)
                    {
                        LevelFail();
                        break;
                    }

                    if (!objective.Passed && objective.IsComplete())
                    {
                        objective.Passed = true;
                        objective.UpdateUI(true);
                        s_remainingObjectives--;
                    }
                }
                _updateTimer = 0.5f;
            }
        }
    }

    // Starts the level
    public static void StartLevel()
    {
        LevelStarted = true;
        foreach (IPlayerManager player in Players)
        {
            player.StartPlayer();
        }
    }

    // Resets the level to its initial state
    public static void ResetLevel()
    {
        LevelStarted = false;
        s_remainingObjectives = s_objectives.Length;

        foreach (IPlayerManager player in Players)
        {
            player.ResetPlayer();
        }

        Players.Clear();
        Players.AddRange(s_startPlayers);

        foreach (Objective objective in s_objectives)
        {
            objective.Reset();
        }
    }

    // Ends the level as a fail
    public static void LevelFail()
    {
        LevelEnded = true;
        s_levelMenu.DisplayLevelFail();
    }

    // Ends the level as a success and unlocks the next level
    private void LevelWin()
    {
        LevelEnded = true;
        int furthestUnlockedScene = PlayerPrefs.GetInt(LevelSelect.UnlockedSceneKey, -1);
        int nextSceneIndex = MenuManager.GetActiveSceneIndex() + 1;
        if (furthestUnlockedScene < nextSceneIndex)
        {
            PlayerPrefs.SetInt(LevelSelect.UnlockedSceneKey, MenuManager.GetActiveSceneIndex() + 1);
        }
        s_levelMenu.DisplayLevelSuccess();
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
}
