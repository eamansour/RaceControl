using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    
    // Fields for pause menu, level success, and level failure UIs
    [SerializeField]
    private GameObject _pauseMenuUI;

    [SerializeField]
    private GameObject _levelSuccessUI;

    [SerializeField]
    private GameObject _levelFailUI;

    private ILevelFeedback _levelFeedback;

    public void Construct(GameObject pauseMenuUI, GameObject levelSuccessUI, GameObject levelFailUI, ILevelFeedback levelFeedback)
    {
        _pauseMenuUI = pauseMenuUI;
        _levelSuccessUI = levelSuccessUI;
        _levelFailUI = levelFailUI;
        _levelFeedback = levelFeedback;
    }

    private void Start()
    {
        if (_levelFeedback == null || _levelFeedback.Equals(null))
        {
            _levelFeedback = GetComponentInChildren<ILevelFeedback>(true);
        }
    }

    // Runs every frame, checking if the player tries to pause the game
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Pauses the current level
    public void PauseGame()
    {
        IsPaused = true;
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        SoundManager.PlaySound("Pause");
    }

    // Resumes the current level
    public void ResumeGame()
    {
        IsPaused = false;
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    // Restarts the current level
    public void RestartLevel()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // Returns the user to the main menu
    public void ExitToMenu()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Displays the "Level Success" UI
    public void DisplayLevelSuccess()
    {
        _levelSuccessUI.SetActive(true);
        _levelFeedback.DisplayFeedback(true);
    }

    // Displays the "Level Fail" UI
    public void DisplayLevelFail()
    {
        _levelFailUI.SetActive(true);
        _levelFeedback.DisplayFeedback(false);
    }
}
