using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

[Category("UITests")]
public class LevelMenuTests
{
    private LevelMenu _levelMenu;
    private ILevelFeedback _levelFeedback;
    private GameObject _pauseMenuUI;
    private GameObject _levelSuccessUI;
    private GameObject _levelFailUI;

    [SetUp]
    public void SetUp()
    {
        _levelMenu = new GameObject().AddComponent<LevelMenu>();
        _pauseMenuUI = new GameObject();
        _levelSuccessUI = new GameObject();
        _levelFailUI = new GameObject();
        _levelFeedback = Substitute.For<ILevelFeedback>();
        _levelMenu.Construct(_pauseMenuUI, _levelSuccessUI, _levelFailUI, _levelFeedback);
    }

    [TearDown]
    public void TearDown()
    {
        Time.timeScale = 1f;
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SetActive(true);
            Object.Destroy(go);
        }
    }

    [Test]
    public void Success_ShouldDisplayLevelSuccessUI()
    {
        _levelSuccessUI.SetActive(false);
        _levelFeedback.When(x => x.DisplayFeedback(true)).Do(x => {});

        _levelMenu.DisplayLevelSuccess();

        Assert.IsTrue(_levelSuccessUI.activeInHierarchy);
    }

    [Test]
    public void Fail_ShouldDisplayLevelFailUI()
    {
        _levelFailUI.SetActive(false);
        _levelFeedback.When(x => x.DisplayFeedback(false)).Do(x => {});

        _levelMenu.DisplayLevelFail();
        
        Assert.IsTrue(_levelFailUI.activeInHierarchy);
    }

    [UnityTest]
    public IEnumerator Pause_ShouldPauseGameAndDisplayPauseMenu()
    {
        _pauseMenuUI.SetActive(false);

        _levelMenu.PauseGame();
        yield return null;

        Assert.AreEqual(0f, Time.timeScale);
        Assert.IsTrue(_pauseMenuUI.activeInHierarchy);
    }

    [UnityTest]
    public IEnumerator Resume_ShouldResumeGameAndRemovePauseMenu()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        _levelMenu.ResumeGame();
        yield return null;

        Assert.AreEqual(1f, Time.timeScale);
        Assert.IsFalse(_pauseMenuUI.activeInHierarchy);
    }
}
