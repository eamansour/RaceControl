using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class ForLoop : CompoundStatement
{
    [SerializeField]
    private TMP_Dropdown _rangeStartDropdown;
    
    [SerializeField]
    private TMP_Dropdown _rangeEndDropdown;

    [SerializeField]
    private TMP_Dropdown _incrementDropdown;

    public void Construct(
        TMP_Dropdown rangeStartDropdown,
        TMP_Dropdown rangeEndDropdown,
        TMP_Dropdown incrementDropdown
    )
    {
        _rangeStartDropdown = rangeStartDropdown;
        _rangeEndDropdown = rangeEndDropdown;
        _incrementDropdown = incrementDropdown;
    }

    public override IEnumerator Run()
    {
        string selectedRangeStart = GetSelectedDropdownText(_rangeStartDropdown);
        string selectedRangeEnd = GetSelectedDropdownText(_rangeEndDropdown);

        // Retrieve and parse selected dropdown options
        int increment = Int32.Parse(GetSelectedDropdownText(_incrementDropdown));
        int startIndex = ParseSelectedRange(selectedRangeStart);
        int endIndex = ParseSelectedRange(selectedRangeEnd);
        int originalIndex = Int32.MaxValue;

        // Add index variable into the environment or store an existing index and update its value to allow scope
        if (Environment.ContainsKey("i"))
        {
            originalIndex = (int)Environment["i"];
            Environment["i"] = startIndex;
        }
        else
        {
            Environment.Add("i", startIndex);
        }

        // Translate for loop into while loop equivalent
        while (Environment.Get<int>("i") != endIndex)
        {
            yield return StartCoroutine(RunBlock());
            Environment["i"] = Environment.Get<int>("i") + increment;
        }

        // Return the index variable to its original index value to allow outer loops to continue
        if (originalIndex != Int32.MaxValue)
        {
            Environment["i"] = originalIndex;
        }
        else
        {
            Environment.Remove("i");
        }
    }

    // Returns the integer representation of a selected text
    private int ParseSelectedRange(string selectedText)
    {
        int playerCount = GameManager.Instance ? GameManager.Instance.Players.Count : 0;
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
