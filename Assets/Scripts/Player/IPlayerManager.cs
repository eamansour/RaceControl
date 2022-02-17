using UnityEngine;

public interface IPlayerManager
{
    GameObject AttachedGameObject { get; }
    Checkpoint TargetCheckpoint { get; }
    Checkpoint LastCheckpoint { get; }
    Checkpoint RecentCheckpoint { get; }
    int CurrentLap { get; }
    ControlMode CurrentControl { get; set; }
    ICar PlayerCar { get; }
    bool IsRetiring { get; set; }

    void Construct(ICar car, ICarAI carAI, Checkpoint startCheckpoint);
    float DistanceToTarget();
    int GetRacePosition();
    void ResetPlayer();
    void RetirePlayer();
    void SetRaceProgress(int currentLap, ControlMode currentControl, Checkpoint targetCheckpoint);
    void StartPlayer();
}
