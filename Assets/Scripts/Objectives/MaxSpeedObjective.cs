using UnityEngine;

public class MaxSpeedObjective : Objective
{
    [SerializeField]
    private float _maxSpeed = Mathf.Infinity;

    public void Construct(float maxSpeed)
    {
        _maxSpeed = maxSpeed;
    }

    public override void UpdateCompletion()
    {
        Failed = (Player.PlayerCar.GetSpeedInMPH() > _maxSpeed);
    }
}
