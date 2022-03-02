using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class ForLoop : CompoundStatement
{
    [SerializeField]
    private TMP_InputField _indexVariableInput;

    [SerializeField]
    private TMP_Dropdown _rangeStartDropdown;

    [SerializeField]
    private TMP_Dropdown _rangeEndDropdown;

    [SerializeField]
    private TMP_Dropdown _incrementDropdown;

    public void Construct(
        TMP_InputField indexVariableInput,
        TMP_Dropdown rangeStartDropdown,
        TMP_Dropdown rangeEndDropdown,
        TMP_Dropdown incrementDropdown
    )
    {
        _indexVariableInput = indexVariableInput;
        _rangeStartDropdown = rangeStartDropdown;
        _rangeEndDropdown = rangeEndDropdown;
        _incrementDropdown = incrementDropdown;
    }

    public override IEnumerator Run()
    {
        string indexVariable = _indexVariableInput.text;
        string selectedRangeStart = GetSelectedDropdownText(_rangeStartDropdown);
        string selectedRangeEnd = GetSelectedDropdownText(_rangeEndDropdown);

        // Retrieve and parse selected inputs
        int increment = Int32.Parse(GetSelectedDropdownText(_incrementDropdown));
        int startIndex = ParseSelectedRange(selectedRangeStart);
        int endIndex = ParseSelectedRange(selectedRangeEnd);
        int originalIndex = Int32.MaxValue;

        // Add index variable into the environment or store an existing index and update its value to allow scope
        if (Environment.ContainsKey(indexVariable))
        {
            originalIndex = (int)Environment[indexVariable];
            Environment[indexVariable] = startIndex;
        }
        else
        {
            Environment.Add(indexVariable, startIndex);
        }

        // Translate for loop into while loop equivalent
        while (Environment.Get<int>(indexVariable) != endIndex)
        {
            yield return StartCoroutine(RunBlock());
            Environment[indexVariable] = Environment.Get<int>(indexVariable) + increment;
        }

        // Return the index variable to its original index value to allow outer loops to continue
        if (originalIndex != Int32.MaxValue)
        {
            Environment[indexVariable] = originalIndex;
        }
        else
        {
            Environment.Remove(indexVariable);
        }
    }

    /// <summary>
    /// Returns the integer representation of a selected range's text.
    /// <summary>
    private int ParseSelectedRange(string selectedText)
    {
        int playerCount = GameManager.Players.Count;
        switch (selectedText)
        {
            case "len(cars) - 1":
                return playerCount - 1;
            case "len(cars)":
                return playerCount;
            default:
                return Int32.Parse(selectedText);
        }
    }
}
