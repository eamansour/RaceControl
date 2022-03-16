using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action<IPlayerManager> OnPlayerUpdated;
    public static IPlayerManager CurrentPlayer { get; private set; }
    public static List<IPlayerManager> Players { get; private set; } = new List<IPlayerManager>();
    public static IInputController InputController { get; private set; }

    public static bool LevelEnded { get; private set; } = false;
    public static bool LevelStarted { get; private set; } = false;

    private static ILevelMenu s_levelMenu;
    private static CameraFollow s_cameraFollow;
    private static CameraController s_cameraController;
    private static IPlayerManager[] s_startPlayers;
    private static IObjective[] s_objectives;
    private static int s_remainingObjectives = 0;

    [SerializeField]
    private LevelMenu _levelMenu;

    [SerializeField]
    private bool _sortPlayers = true;

    private float _updateTimer = 0.5f;

    public void Construct(
        IPlayerManager currentPlayer = null,
        IObjective[] objectives = null,
        IPlayerManager[] startPlayers = null,
        ILevelMenu levelMenu = null
    )
    {
        CurrentPlayer = currentPlayer;
        s_objectives = objectives ?? new IObjective[0];
        s_startPlayers = startPlayers ?? new IPlayerManager[0];
        s_levelMenu = levelMenu;
    }

    private void Awake()
    {
        Time.timeScale = 1f;

        Players = new List<IPlayerManager>();
        InputController = GetComponent<IInputController>();

        LevelStarted = false;
        LevelEnded = false;

        s_levelMenu = _levelMenu;
        s_cameraFollow = FindObjectOfType<CameraFollow>();
        s_cameraController = s_cameraFollow
            ? s_cameraFollow.GetComponent<CameraController>()
            : FindObjectOfType<CameraController>();

        s_objectives = GetComponents<IObjective>();
        s_remainingObjectives = s_objectives.Length;

        s_startPlayers = FindObjectsOfType<PlayerManager>();
        Players.AddRange(s_startPlayers);

        if (s_cameraFollow)
        {
            CurrentPlayer = s_cameraFollow.Target.GetComponent<IPlayerManager>();
        }
        else if (s_startPlayers.Length > 0)
        {
            CurrentPlayer = s_startPlayers[0];
        }
        else
        {
            Debug.LogWarning("CurrentPlayer not found: No camera target or start players set.");
        }
    }

    /// <summary>
    /// Runs every frame, checks if the objectives have been met once the level has been started.
    /// </summary>
    private void Update()
    {
        if (!LevelStarted || LevelEnded) return;

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

                if (!objective.Passed)
                {
                    objective.UpdateCompletion();
                    if (objective.Passed)
                    {
                        objective.UpdateUI(true);
                        s_remainingObjectives--;
                    }
                }
            }
            _updateTimer = 0.5f;
        }
    }

    /// <summary>
    /// Sets the level to its started state.
    /// </summary>
    public static void StartLevel()
    {
        LevelStarted = true;
        foreach (IPlayerManager player in Players)
        {
            player.StartPlayer();
        }
    }

    /// <summary>
    /// Resets the level to its initial state.
    /// </summary>
    public static void ResetLevel()
    {
        LevelStarted = false;
        s_remainingObjectives = s_objectives.Length;

        if (s_cameraController)
        {
            s_cameraController.ResetCamera();
        }

        foreach (IPlayerManager player in Players)
        {
            player.ResetPlayer();
        }

        Players.Clear();
        Players.AddRange(s_startPlayers);

        foreach (IObjective objective in s_objectives)
        {
            objective.Reset();
        }
    }

    /// <summary>
    /// Update the current player to a new player.
    /// </summary>
    public static void SetPlayer(IPlayerManager newPlayer)
    {
        CurrentPlayer = newPlayer;

        if (s_cameraFollow)
        {
            s_cameraFollow.Target = newPlayer.AttachedGameObject;
        }

        if (OnPlayerUpdated != null)
        {
            OnPlayerUpdated(newPlayer);
        }
    }

    /// <summary>
    /// Ends the level as a failure.
    /// </summary>
    public static void LevelFail()
    {
        LevelEnded = true;
        s_levelMenu.DisplayLevelFail();
        SoundManager.PlaySound("Fail");
    }

    /// <summary>
    /// Ends the level as a success and unlocks the next level.
    /// </summary>
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
        SoundManager.PlaySound("Win");
    }

    /// <summary>
    /// Sorts the current players according to their progress in the level.
    /// </summary>
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
