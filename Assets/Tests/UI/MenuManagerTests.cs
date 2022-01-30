using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

[Category("UITests")]
public class MenuManagerTests
{
    [OneTimeSetUp]
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    [UnityTest]
    public IEnumerator NextScene_ShouldIncrementSceneIndex()
    {
        MenuManager menuManager = new GameObject().AddComponent<MenuManager>();

        menuManager.NextScene();
        yield return new WaitForSeconds(0.1f);

        Scene newScene = SceneManager.GetActiveScene();
        Assert.AreEqual(1, newScene.buildIndex);
    }

    [UnityTest]
    public IEnumerator LoadLevel_ShouldLoadSpecifiedScene()
    {
        MenuManager menuManager = new GameObject().AddComponent<MenuManager>();

        menuManager.LoadLevel("Level-1");
        yield return new WaitForSeconds(0.1f);

        Scene newScene = SceneManager.GetActiveScene();
        Assert.AreEqual("Level-1", newScene.name);
    }
}
