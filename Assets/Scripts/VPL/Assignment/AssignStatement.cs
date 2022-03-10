using System;
using System.Collections;
using UnityEngine;
using TMPro;

public abstract class AssignStatement<T> : Statement
{
    [SerializeField]
    protected TMP_Dropdown _variableDropdown;

    protected IExpression<T> _expression;

    [SerializeField]
    private TMP_InputField _variableInput;

    private ICar _car;
    private IObstacleSpawn _spawner;

    public void Construct(
        ICar car,
        IExpression<T> expression,
        IObstacleSpawn spawner,
        TMP_InputField variableInput = null,
        TMP_Dropdown variableDropdown = null
    )
    {
        _car = car;
        _expression = expression;
        _variableInput = variableInput;
        _variableDropdown = variableDropdown;
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

    /// <summary>
    /// Assigns a result of an expression to a given variable.
    /// </summary>
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
            UpdatePlayerFuel(result);
        }
        else if (Environment.ContainsKey(variable))
        {
            Environment[variable] = result;
        }
        else
        {
            Environment.Add(variable, result);
        }
    }

    /// <summary>
    /// Handles assignment to update player fuel, only allowing it to be decreased.
    /// </summary>
    private void UpdatePlayerFuel(T result)
    {
        float fuelResult = Convert.ToSingle(result);

        if (fuelResult < _car.Fuel)
        {
            _car.Fuel = fuelResult;
            _spawner.SpawnObstacle();
            Environment["fuel"] = result;
        }
    }
}
