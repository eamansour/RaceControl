using UnityEngine;

public class MinSpeedObjective : Objective
{
    [SerializeField]
    private float _minSpeed = 0;

    public void Construct(float minSpeed)
    {
        _minSpeed = minSpeed;
    }

    // Determine whether the player has met the minimum speed required
    public override void UpdateCompletion()
    {
        Passed = (Player.PlayerCar.GetSpeedInMPH() >= _minSpeed);
    }
}
