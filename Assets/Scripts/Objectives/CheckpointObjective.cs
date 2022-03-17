using UnityEngine;

public class CheckpointObjective : Objective
{
    [SerializeField]
    private Checkpoint _requiredCheckpoint;

    [SerializeField]
    private bool _avoidCheckpoint = false;

    public void Construct(Checkpoint requiredCheckpoint, bool avoidCheckpoint = false)
    {
        _requiredCheckpoint = requiredCheckpoint;
        _avoidCheckpoint = avoidCheckpoint;
    }

    public override void UpdateCompletion()
    {
        if (_avoidCheckpoint)
        {
            Failed = (Player.LastCheckpoint == _requiredCheckpoint);
        }
        else
        {
            Passed = (Player.LastCheckpoint == _requiredCheckpoint);
        }
    }
}