using UnityEngine;

public class MinSpeedObjective : Objective
{
    [SerializeField]
    private float _minSpeed = 0;

    public void Construct(float minSpeed)
    {
        _minSpeed = minSpeed;
    }

    public override void UpdateCompletion()
    {
        Passed = (Player.PlayerCar.GetSpeedInMPH() >= _minSpeed);
    }
}
