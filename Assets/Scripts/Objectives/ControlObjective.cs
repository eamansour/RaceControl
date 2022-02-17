using System.Collections.Generic;
using UnityEngine;

public class ControlObjective : Objective
{
    [SerializeField]
    private List<ControlMode> _lapControl = new List<ControlMode>();

    public void Construct(List<ControlMode> lapControl)
    {
        _lapControl = lapControl;
    }

    public override void UpdateCompletion()
    {
        // Fail the objective if the current control method is incorrect
        int currentLap = Player.CurrentLap;
        if (currentLap < _lapControl.Count && Player.CurrentControl != _lapControl[currentLap])
        {
            Failed = true;
            return;
        }

        Passed = (currentLap >= _lapControl.Count);
    }
}
