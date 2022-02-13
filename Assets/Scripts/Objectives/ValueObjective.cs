using System;
using UnityEngine;

public class ValueObjective : Objective
{
    [SerializeField]
    private int _targetValue = 0;

    [SerializeField]
    private string _variableName = "";

    public void Construct(string targetVariable, int targetValue)
    {
        _variableName = targetVariable;
        _targetValue = targetValue;
    }

    // Determines if a given variable has a value matching the target value
    public override void UpdateCompletion()
    {
        int currentValue = (int)Math.Round(Statement.Environment.Get<float>(_variableName));
        Passed = (currentValue == _targetValue);
    }
}
