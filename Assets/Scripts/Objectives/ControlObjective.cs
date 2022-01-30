using System.Collections.Generic;
using UnityEngine;

public class ControlObjective : Objective
{
    [SerializeField]
    private List<PlayerManager.ControlMethod> _lapControl = new List<PlayerManager.ControlMethod>();

    public void Construct(List<PlayerManager.ControlMethod> lapControl)
    {
        _lapControl = lapControl;
    }

    public override bool IsComplete()
    {
        // Fail the objective if the current control method is incorrect
        int currentLap = Player.CurrentLap;
        if (currentLap < _lapControl.Count && Player.CurrentControl != _lapControl[currentLap])
        {
            Failed = true;
            return false;
        }

        return (currentLap >= _lapControl.Count);
    }
}
