using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

[Category("UITests")]
public class MenuManagerTests
{
    MenuManager _menuManager;

    [SetUp]
    public void SetUp()
    {
        _menuManager = new GameObject().AddComponent<MenuManager>();
        SceneManager.LoadScene(0);
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator NextScene_ShouldIncrementSceneIndex()
    {
        _menuManager.NextScene();
        yield return new WaitForSeconds(0.1f);

        Scene newScene = SceneManager.GetActiveScene();
        Assert.AreEqual(1, newScene.buildIndex);
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator LoadLevel_ShouldLoadSpecifiedScene()
    {
        _menuManager.LoadLevel("Level-1");
        yield return new WaitForSeconds(0.1f);

        Scene newScene = SceneManager.GetActiveScene();
        Assert.AreEqual("Level-1", newScene.name);
    }
}
