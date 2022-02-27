using UnityEngine;

public interface ILevelMenu
{
    void Construct(GameObject pauseMenuUI, GameObject levelSuccessUI, GameObject levelFailUI, ILevelFeedback levelFeedback);

    /// <summary>
    /// Displays the level failure UI.
    /// </summary>
    void DisplayLevelFail();

    /// <summary>
    /// Displays the level success UI.
    /// </summary>
    void DisplayLevelSuccess();

    /// <summary>
    /// Returns the user to the main menu.
    /// </summary>
    void ExitToMenu();

    /// <summary>
    /// Pauses the current level.
    /// </summary>
    void PauseGame();

    /// <summary>
    /// Restarts the current level.
    /// </summary>
    void RestartLevel();

    /// <summary>
    /// Resumes the currently paused level.
    /// </summary>
    void ResumeGame();
}
