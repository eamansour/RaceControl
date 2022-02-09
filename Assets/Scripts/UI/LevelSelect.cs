using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public static string UnlockedSceneKey = "UnlockedScene";

    [SerializeField]
    private Button[] _levelButtons;

    private void Start()
    {
        int currentSceneIndex = MenuManager.GetActiveSceneIndex();
        int unlockedSceneIndex = PlayerPrefs.GetInt(UnlockedSceneKey, currentSceneIndex + 1);
        for (int i = 1; i <= _levelButtons.Length; i++)
        {
            // Disable all levels that have not been unlocked yet
            if (currentSceneIndex + i > unlockedSceneIndex)
            {
                _levelButtons[i - 1].interactable = false;
            }
        }
    }

    public void OnClick()
    {
        SoundManager.PlaySound("LevelClick");
    }

    public void OnMouseOver()
    {
        SoundManager.PlaySound("LevelHover");
    }
}
