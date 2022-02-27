using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Statement : MonoBehaviour
{
    public static Dictionary<string, object> Environment = new Dictionary<string, object>();
    private Image _background;
    private Color _initialColor;

    private void Awake()
    {
        _background = GetComponent<Image>();
        _initialColor = _background.color;
    }

    /// <summary>
    /// Sets up the environment with pre-defined variables for use in the game.
    /// </summary>
    public static void SetUpEnvironment()
    {
        Environment.Clear();
        Environment.Add("fuel", 100f);
        Environment.Add("lap", 0);
    }

    /// <summary>
    /// Sets the background panel's colour to indicate the statement is running.
    /// </summary>
    public void SetRunningColour()
    {
        _background.color = Color.green;
    }

    /// <summary>
    /// Resets the background panel's colour to indicate that it is not running.
    /// </summary>
    public void ResetColour()
    {
        _background.color = _initialColor;
    }

    /// <summary>
    /// Executes the statement logic.
    /// </summary>
    public abstract IEnumerator Run();

    /// <summary>
    /// Helper method to get a given dropdown's selected option text.
    /// </summary>
    protected string GetSelectedDropdownText(TMP_Dropdown dropdown)
    {
        return dropdown.options[dropdown.value].text;
    }

    /// <summary>
    /// Helper method to get a selected dropdown option and convert it to a float.
    /// </summary>
    protected float GetSelectedToFloat(TMP_Dropdown dropdown)
    {
        return float.TryParse(GetSelectedDropdownText(dropdown), out float value) ? value : 0f;
    }
}
