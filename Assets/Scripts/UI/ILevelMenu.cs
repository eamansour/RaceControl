using UnityEngine;

public interface ILevelMenu
{
    void Construct(GameObject pauseMenuUI, GameObject levelSuccessUI, GameObject levelFailUI, ILevelFeedback levelFeedback);
    void DisplayLevelFail();
    void DisplayLevelSuccess();
    void ExitToMenu();
    void PauseGame();
    void RestartLevel();
    void ResumeGame();
}
