using UnityEngine;

public class CheckpointObjective : Objective
{
    [SerializeField]
    private Checkpoint _requiredCheckpoint;

    public void Construct(Checkpoint requiredCheckpoint)
    {
        _requiredCheckpoint = requiredCheckpoint;
    }

    public override void UpdateCompletion()
    {
        Passed = (Player.LastCheckpoint == _requiredCheckpoint);
    }
}