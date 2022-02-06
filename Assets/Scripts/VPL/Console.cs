using System.Collections;
using UnityEngine;

public class Console : MonoBehaviour
{
    public static bool Paused { get; set; } = false;

    [SerializeField]
    private CameraController _mainCamera;

    private bool _waitAndFail = true;

    private Statement _currentStatement;

    // Runs the player's inputted program
    private IEnumerator RunProgram()
    {
        // Skip a frame to ensure all level start systems are ready
        yield return null;

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
            yield return new WaitForSeconds(10f);
            if (!GameManager.Instance.IsSuccess)
            {
                GameManager.Instance.LevelFail();
            }
        }
    }

    // Wrapper method to run inputted programs via a UI button
    public void StartProgram(bool failAfterDelay)
    {
        _waitAndFail = failAfterDelay;
        Statement.SetUpEnvironment();
        GameManager.Instance.StartLevel();
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

        GameManager.Instance.ResetLevel();
        _mainCamera.ResetCamera();
    }

    // Play an appropriate sound when clicking a start/stop program button
    public void OnClick(bool start)
    {
        if (start)
        {
            SoundManager.Instance.PlaySound("StartProgram");
        }
        else
        {
            SoundManager.Instance.PlaySound("StopProgram");
        }
    }
}
