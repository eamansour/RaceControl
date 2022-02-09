using UnityEngine;

public class MaxSpeedObjective : Objective
{
    [SerializeField]
    private float _maxSpeed = Mathf.Infinity;

    // Determine whether the player has not gone over the maximum speed
    public void CheckMaxSpeed()
    {
        if (Player.PlayerCar.GetSpeedInMPH() > _maxSpeed)
        {
            GameManager.LevelFail();
        }
    }

    public override bool IsComplete()
    {
        if (Player.PlayerCar.GetSpeedInMPH() > _maxSpeed)
        {
            Failed = true;
            return false;
        }
        return true;
    }
}
