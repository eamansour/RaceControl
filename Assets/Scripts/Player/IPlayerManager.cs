using UnityEngine;

public interface IPlayerManager
{
    GameObject AttachedGameObject { get; }
    Checkpoint TargetCheckpoint { get; }
    Checkpoint LastCheckpoint { get; }
    Checkpoint RecentCheckpoint { get; }
    int CurrentLap { get; }
    float CurrentLapTime { get; }
    float BestLapTime { get; }
    ControlMode CurrentControl { get; set; }
    ICar PlayerCar { get; }
    bool IsRetiring { get; set; }

    void Construct(ICar car, ICarAI carAI, Checkpoint startCheckpoint);

    /// <summary>
    /// Gets the distance to the player's target checkpoint.
    /// </summary>
    float DistanceToTarget();

    /// <summary>
    /// Gets the player's position in the current race.
    /// </summary>
    int GetRacePosition();

    /// <summary>
    /// Resets the player and its race progress to its initial state.
    /// </summary>
    void ResetPlayer();

    /// <summary>
    /// Retires the player from the race, locking it in place.
    /// </summary>
    void RetirePlayer();

    /// <summary>
    /// Updates the player's race progress by copying properties from another
    /// player.
    /// </summary>
    void CopyRaceProgress(IPlayerManager otherPlayer);

    /// <summary>
    /// Unlocks the player's physics properties to be able to start a race.
    /// </summary>
    void StartPlayer();
}
