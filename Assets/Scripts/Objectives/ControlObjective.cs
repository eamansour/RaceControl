using System.Collections.Generic;
using UnityEngine;

public class ControlObjective : Objective
{
    [SerializeField]
    private List<ControlType> _lapControl = new List<ControlType>();

    public void Construct(List<ControlType> lapControl)
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
