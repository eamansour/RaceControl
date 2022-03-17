using UnityEngine;

public class SpeedObjective : Objective
{
    [SerializeField]
    private float _speed = 0;

    [SerializeField]
    private bool _speedIsLimit = false;

    public void Construct(float speed, bool speedIsLimit = false)
    {
        _speed = speed;
        _speedIsLimit = speedIsLimit;
    }

    public override void UpdateCompletion()
    {
        if (_speedIsLimit)
        {
            Failed = (Player.PlayerCar.GetSpeedInMPH() > _speed);
        }
        else
        {
            Passed = (Player.PlayerCar.GetSpeedInMPH() >= _speed);
        }
    }
}
