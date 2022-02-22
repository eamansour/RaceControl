using System.Collections;
using UnityEngine;

public class Console : MonoBehaviour
{
    public static bool Paused { get; set; } = false;

    private const float FailDelay = 10f;

    private bool _waitAndFail = true;

    private Statement _currentStatement;

    // Runs the player's inputted program
    private IEnumerator RunProgram()
    {
        foreach (Transform child in transform)
        {
            while (Paused)
            {
                yield return null;
            }

            _currentStatement = child.GetComponent<Statement>();

            _currentStatement.SetRunningColour();
            yield return StartCoroutine(_currentStatement.Run());
            _currentStatement.ResetColour();
        }

        // Wait before declaring level failure in case the car is still moving
        if (_waitAndFail)
        {
            yield return new WaitForSeconds(FailDelay);
            if (!GameManager.LevelEnded)
            {
                GameManager.LevelFail();
            }
        }
    }

    // Wrapper method to run inputted programs via a UI button
    public void StartProgram(bool failAfterDelay)
    {
        _waitAndFail = failAfterDelay;
        Statement.SetUpEnvironment();
        GameManager.StartLevel();
        StartCoroutine(RunProgram());
    }

    // Stops the inputted program, returning the player and camera to their original positions
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

    // Play an appropriate sound when clicking a start/stop program button
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
