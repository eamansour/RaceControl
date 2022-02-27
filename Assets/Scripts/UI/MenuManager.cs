using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Gets the current scene's build index.
    /// </summary>
    public static int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Loads the next scene in the build.
    /// </summary>
    public void NextScene()
    {
        int nextSceneIndex = GetActiveSceneIndex() + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    /// <summary>
    /// Loads a level, given by name.
    /// </summary>
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    /// <summary>
    /// Terminates the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    // Plays a button click sound when a menu button is clicked
    public void OnClick()
    {
        SoundManager.PlaySound("ButtonClick");
    }

    // Plays a button hover sound when the cursor hovers over a button
    public void OnMouseOver()
    {
        SoundManager.PlaySound("ButtonHover");
    }
}