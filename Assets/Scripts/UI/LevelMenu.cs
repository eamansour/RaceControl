using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour, ILevelMenu
{
    private const string EngineSoundName = "Engine";
    private const string PauseSoundName = "Pause";

    public static bool IsPaused = false;

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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void PauseGame()
    {
        IsPaused = true;
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        if (SoundManager.GetSource(EngineSoundName) && SoundManager.GetSource(PauseSoundName))
        {
            SoundManager.StopSound(EngineSoundName);
            SoundManager.PlaySound(PauseSoundName);
        }
    }

    /// <inheritdoc />
    public void ResumeGame()
    {
        IsPaused = false;
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        if (SoundManager.GetSource(EngineSoundName))
        {
            SoundManager.PlaySound(EngineSoundName);
        }
    }

    /// <inheritdoc />
    public void RestartLevel()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    /// <inheritdoc />
    public void ExitToMenu()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    /// <inheritdoc />
    public void DisplayLevelSuccess()
    {
        _levelSuccessUI.SetActive(true);
        _levelFeedback.DisplayFeedback(true);
    }

    /// <inheritdoc />
    public void DisplayLevelFail()
    {
        _levelFailUI.SetActive(true);
        _levelFeedback.DisplayFeedback(false);
    }
}
