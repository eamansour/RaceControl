using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using NSubstitute;

[Category("UITests")]
public class LevelMenuSceneTests
{
    private LevelMenu _levelMenu;
    private ILevelFeedback _levelFeedback;
    public static int RestartOnce = 1;

    [SetUp]
    public void SetUp()
    {
        _levelMenu = new GameObject("LevelMenu").AddComponent<LevelMenu>();
        _levelFeedback = Substitute.For<ILevelFeedback>();
        _levelMenu.Construct(new GameObject(), new GameObject(), new GameObject(), _levelFeedback);
    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SetActive(true);
            Object.Destroy(go);
        }
    }

    [UnityTest]
    public IEnumerator ExitToMenu_ShouldLoadMenuScene()
    {
        _levelMenu.ExitToMenu();
        yield return new WaitForSeconds(0.5f);

        Assert.AreEqual(0, SceneManager.GetActiveScene().buildIndex);
    }

    [UnityTest]
    public IEnumerator Restart_ShouldReloadCurrentScene()
    {
        // Force the test to restart the scene once
        if (RestartOnce > 0)
        {
            Scene initialScene = SceneManager.GetActiveScene();
            _levelMenu.RestartLevel();
            RestartOnce--;
            yield return new WaitForSeconds(0.1f);
            Scene newScene = SceneManager.GetActiveScene();
    
            Assert.AreEqual("MainMenu", newScene.name);
        }
    }
}
