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

    private static ICar s_car;
    private static IObstacleSpawn s_spawner;

    public void Construct(
        ICar car,
        IExpression<T> expression,
        IObstacleSpawn spawner,
        TMP_InputField variableInput = null,
        TMP_Dropdown variableDropdown = null
    )
    {
        s_car = car;
        _expression = expression;
        _variableInput = variableInput;
        _variableDropdown = variableDropdown;
        s_spawner = spawner;
    }

    private void Start()
    {
        if (s_car == null || s_car.Equals(null))
        {
            IPlayerManager currentPlayer = GameManager.CurrentPlayer;
            s_car = currentPlayer.PlayerCar;
            s_spawner = currentPlayer.AttachedGameObject.GetComponentInChildren<IObstacleSpawn>();
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
        IPlayerManager currentPlayer = GameManager.CurrentPlayer;
        if (s_car != currentPlayer.PlayerCar)
        {
            s_car = currentPlayer.PlayerCar;
            s_spawner = currentPlayer.AttachedGameObject.GetComponentInChildren<IObstacleSpawn>();
        }
        
        float fuelResult = Convert.ToSingle(result);

        if (fuelResult < s_car.Fuel)
        {
            s_car.Fuel = fuelResult;
            s_spawner.SpawnObstacle();
            Environment["fuel"] = result;
        }
    }
}
