using System;
using System.Collections;
using UnityEngine;
using TMPro;

public abstract class AssignStatement<T> : Statement
{
    [SerializeField]
    private TMP_InputField _variableInput;

    [SerializeField]
    protected TMP_Dropdown _variableDropdown;

    protected IExpression<T> _expression;

    private ICar _car;
    private IObstacleSpawn _spawner;

    public void Construct(ICar car, IExpression<T> expression, TMP_InputField variableInput, IObstacleSpawn spawner)
    {
        _car = car;
        _expression = expression;
        _variableInput = variableInput;
        _spawner = spawner;
    }

    private void Start()
    {
        if (_car == null || _car.Equals(null))
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            _car = playerObject.GetComponent<ICar>();
            _spawner = playerObject.GetComponentInChildren<IObstacleSpawn>();
        }

        if (_expression == null || _expression.Equals(null))
        {
            _expression = GetComponentInChildren<IExpression<T>>();
        }
    }

    // Assigns a result of an expression to a given variable
    public override IEnumerator Run()
    {
        string variable = "";
        if (_variableDropdown) 
        {
            variable = _variableDropdown.options[_variableDropdown.value].text;
        }
        else
        {
            variable = string.IsNullOrEmpty(_variableInput.text) ? "NA" : _variableInput.text;
        }

        if (variable == "lap") yield break;
        T result = _expression.EvaluateExpression();
        
        // Update the player's fuel level, assign a new variable, or update an existing variable
        if (variable.ToLower() == "fuel" && Environment.ContainsKey("fuel"))
        {
            float fuelResult = Convert.ToSingle(result);

            // Allow the fuel to be decreased using assign statements 
            if (fuelResult < _car.Fuel)
            {
                _car.Fuel = fuelResult;
                _spawner.SpawnObstacle();
                Environment["fuel"] = result;
            }
        }
        else if (Environment.ContainsKey(variable))
        {
            Environment[variable] = result;
        }
        else
        {
            Environment.Add(variable, result);
        }
        yield break;
    }
}
