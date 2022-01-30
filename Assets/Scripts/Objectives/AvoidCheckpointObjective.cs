using UnityEngine;

public class AvoidCheckpointObjective : CheckpointObjective
{
    [SerializeField]
    private Checkpoint _checkpointToAvoid;

    public void CheckAvoidCheckpoint()
    {
        if (Player.LastCheckpoint == _checkpointToAvoid)
        {
            GameManager.Instance.LevelFail();
        }
    }
}
