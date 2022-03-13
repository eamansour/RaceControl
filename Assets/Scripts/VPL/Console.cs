using System.Collections;
using UnityEngine;

public class Console : MonoBehaviour
{
    public static bool Paused { get; set; } = false;

    private const float FailDelay = 10f;

    private Statement _currentStatement;

    /// <summary>
    /// Runs the player's inputted program.
    /// </summary>
    private IEnumerator RunProgram(bool failAfterDelay)
    {
        foreach (Transform child in transform)
        {
            while (Paused)
            {
                yield return null;
            }

            _currentStatement = child.GetComponent<Statement>();

            _currentStatement.SetColor(Color.green);
            yield return StartCoroutine(_currentStatement.Run());
            _currentStatement.ResetColour();
        }

        // Wait before declaring level failure in case the car is still moving
        if (failAfterDelay)
        {
            yield return new WaitForSeconds(FailDelay);
            if (!GameManager.LevelEnded)
            {
                GameManager.LevelFail();
            }
        }
    }

    /// <summary>
    /// Wrapper method to run inputted programs via a UI button.
    /// </summary>
    public void StartProgram(bool failAfterDelay)
    {
        Statement.SetUpEnvironment();
        GameManager.StartLevel();
        StartCoroutine(RunProgram(failAfterDelay));
    }

    /// <summary>
    /// Stops the inputted program, returning the player and camera to their original positions.
    /// </summary>
    public void StopProgram()
    {
        if (_currentStatement)
        {
            _currentStatement.ResetColour();
        }

        StopAllCoroutines();
        foreach (Transform child in transform)
        {
            _currentStatement = child.GetComponent<Statement>();
            _currentStatement.StopAllCoroutines();
        }
        GameManager.ResetLevel();
    }

    /// <summary>
    /// Play an appropriate sound when clicking a start/stop program button.
    /// </summary>
    public void OnClick(bool start)
    {
        if (start)
        {
            SoundManager.PlaySound("StartProgram");
        }
        else
        {
            SoundManager.PlaySound("StopProgram");
        }
    }
}
