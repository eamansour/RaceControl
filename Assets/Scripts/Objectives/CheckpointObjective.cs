using UnityEngine;

public class CheckpointObjective : Objective
{
    [SerializeField]
    private Checkpoint _requiredCheckpoint;

    public void Construct(Checkpoint requiredCheckpoint)
    {
        _requiredCheckpoint = requiredCheckpoint;
    }

    // Determines if the player has passed the required checkpoint
    public override bool IsComplete()
    {
        return (Player.LastCheckpoint == _requiredCheckpoint);
    }
}