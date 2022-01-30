using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Returns the current scene's build index
    public static int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    // Loads the next scene
    public void NextScene()
    {
        int nextSceneIndex = GetActiveSceneIndex() + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    // Loads a given level
    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    // Terminates the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Plays a button click sound when a menu button is clicked
    public void OnClick()
    {
        SoundManager.Instance.PlaySound("ButtonClick");
    }

    // Plays a button hover sound when the cursor hovers over a button
    public void OnMouseOver()
    {
        SoundManager.Instance.PlaySound("ButtonHover");
    }
}