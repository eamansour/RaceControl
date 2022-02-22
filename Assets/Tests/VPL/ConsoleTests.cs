using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using UnityEngine.UI;

[Category("VPLTests")]
public class ConsoleTests
{
    private Console _console;
    private IPlayerManager _player;
    private ICar _car;
    private GameObject _testObject;

    [SetUp]
    public void SetUp()
    {
        _player = Substitute.For<IPlayerManager>();
        _car = Substitute.For<ICar>();

        // Create root object
        _testObject = new GameObject();
        _testObject.AddComponent<GameManager>();
        _testObject.AddComponent<InputController>();

        // Add components to test
        _console = _testObject.AddComponent<Console>();

        // Create 3 dummy "autopilot" statements to test calls against
        for (int i = 0; i < 3; i ++)
        {
            CreateDummyStatement();
        }
    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SetActive(true);
            Object.Destroy(go);
        }
        Statement.Environment.Clear();
    }

    private void CreateDummyStatement()
    {
        GameObject temp = new GameObject();
        temp.AddComponent<Image>();
        Autopilot dummyStatement = temp.AddComponent<Autopilot>();
        dummyStatement.Construct(_car, _player);
        temp.transform.SetParent(_testObject.transform);
    }

    [UnityTest]
    public IEnumerator StartProgram_RunsProgramStatements()
    {
        _console.StartProgram(false);
        yield return null;

        _player.Received(1).CurrentControl = ControlMode.AI;
        yield return null;
        
        _player.Received(2).CurrentControl = ControlMode.AI;
        yield return null;

        _player.Received(3).CurrentControl = ControlMode.AI;
    }

    [Test]
    public void StartProgram_StartsLevel()
    {
        _console.StartProgram(false);
        Assert.IsTrue(GameManager.LevelStarted);
    }

    [Test]
    public void StartProgram_SetsUpEnvironment()
    {
        Assert.IsEmpty(Statement.Environment);
        
        _console.StartProgram(false);

        Assert.IsNotEmpty(Statement.Environment);
        
    }

    [UnityTest]
    public IEnumerator StartProgram_DoesNotRunStatementsWhenConsolePaused()
    {
        Console.Paused = true;
        _console.StartProgram(false);
        yield return null;

        _player.DidNotReceive().CurrentControl = ControlMode.AI;

        Console.Paused = false;
        yield return null;
        yield return null;

        // No longer paused
        _player.Received(1).CurrentControl = ControlMode.AI;
    }

    [UnityTest]
    public IEnumerator StopProgram_StopsLevel()
    {
        GameManager.StartLevel();

        _console.StopProgram();
        yield return null;

        Assert.IsFalse(GameManager.LevelStarted);
    }
}
