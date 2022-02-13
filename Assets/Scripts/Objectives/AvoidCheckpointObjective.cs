using UnityEngine;

public class AvoidCheckpointObjective : Objective
{
    [SerializeField]
    private Checkpoint _checkpointToAvoid;

    public void Construct(Checkpoint checkpointToAvoid)
    {
        _checkpointToAvoid = checkpointToAvoid;
    }

    public override void UpdateCompletion()
    {
        Failed = (Player.LastCheckpoint == _checkpointToAvoid);
    }
}
