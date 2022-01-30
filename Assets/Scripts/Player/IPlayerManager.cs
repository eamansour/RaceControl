using UnityEngine;

public interface IPlayerManager
{
    GameObject AttachedGameObject { get; }
    Checkpoint TargetCheckpoint { get; }
    Checkpoint LastCheckpoint { get; }
    int CurrentLap { get; }
    PlayerManager.ControlMethod CurrentControl { get; set; }
    ICar PlayerCar { get; }
    bool IsRetiring { get; set; }

    void Construct(ICar car, ICarAI carAI, Checkpoint startCheckpoint);
    float DistanceToTarget();
    int GetRacePosition();
    void ResetPlayer();
    void RetirePlayer();
    void SetRaceProgress(int currentLap, PlayerManager.ControlMethod currentControl, Checkpoint targetCheckpoint);
    void StartPlayer();
}