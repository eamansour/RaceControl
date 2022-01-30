using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Statement : MonoBehaviour
{
    // Define environment to store variables and their associated values, with default built-in variables
    public static Dictionary<string, object> Environment = new Dictionary<string, object>();
    private Image _background;
    private Color _initialColor;

    // Initialise fields
    private void Awake()
    {
        _background = GetComponent<Image>();
        _initialColor = _background.color;
    }

    // Sets up the environment with default key-value pairs
    public static void SetUpEnvironment()
    {
        Environment.Clear();
        Environment.Add("fuel", 100f);
        Environment.Add("lap", 0);
    }

    // Sets the background panel's colour to indicate the statement is running
    public void SetRunningColour()
    {
        _background.color = Color.green;
    }

    // Resets the background panel's colour to indicate that it is not running
    public void ResetColour()
    {
        _background.color = _initialColor;
    }

    // Executes the statement logic
    public abstract IEnumerator Run();

    protected string GetSelectedDropdownText(TMP_Dropdown dropdown)
    {
        return dropdown.options[dropdown.value].text;
    }
}
