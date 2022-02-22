using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

[Category("GameManagementTests")]
public class GameManagerTests
{
    private GameManager _gameManager;
    private GameObject _testObject;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _gameManager = _testObject.AddComponent<GameManager>();
        _testObject.AddComponent<CameraController>();
    }

    [TearDown]
    public void TearDown()
    {
        GameManager.Players.Clear();
        Object.Destroy(_testObject);
    }

    [Test]
    public void StartLevel_StartsAllPlayers()
    {
        List<IPlayerManager> testPlayers = new List<IPlayerManager>
        { 
            Substitute.For<IPlayerManager>(),
            Substitute.For<IPlayerManager>(),
            Substitute.For<IPlayerManager>()
        };

        GameManager.Players.AddRange(testPlayers);

        GameManager.StartLevel();

        testPlayers[0].Received(1).StartPlayer();
        testPlayers[1].Received(1).StartPlayer();
        testPlayers[2].Received(1).StartPlayer();
    }

    [Test]
    public void StartLevel_SetsLevelStarted()
    {
        GameManager.StartLevel();
        Assert.IsTrue(GameManager.LevelStarted);
    }

    [Test]
    public void ResetLevel_ResetsAllPlayers()
    {
        List<IPlayerManager> testPlayers = new List<IPlayerManager>
        { 
            Substitute.For<IPlayerManager>(),
            Substitute.For<IPlayerManager>(),
            Substitute.For<IPlayerManager>()
        };

        GameManager.Players.AddRange(testPlayers);

        GameManager.ResetLevel();

        testPlayers[0].Received(1).ResetPlayer();
        testPlayers[1].Received(1).ResetPlayer();
        testPlayers[2].Received(1).ResetPlayer();
    }

    [Test]
    public void ResetLevel_ResetsStartPlayers()
    {
        IPlayerManager[] startPlayers = { 
            Substitute.For<IPlayerManager>(),
            Substitute.For<IPlayerManager>()
        };
        _gameManager.Construct(startPlayers: startPlayers);

        GameManager.Players.Add(Substitute.For<IPlayerManager>());
        GameManager.ResetLevel();

        Assert.AreEqual(2, GameManager.Players.Count);
    }

    [Test]
    public void ResetLevel_ResetsAllObjectives()
    {
        IObjective[] objectives = { 
            Substitute.For<IObjective>(),
            Substitute.For<IObjective>()
        };

        _gameManager.Construct(objectives);
        GameManager.ResetLevel();

        objectives[0].Received(1).Reset();
        objectives[1].Received(1).Reset();
    }

    [Test]
    public void LevelFail_DisplaysLevelFailUI()
    {
        ILevelMenu levelMenu = Substitute.For<ILevelMenu>();
        _gameManager.Construct(levelMenu: levelMenu);

        GameManager.LevelFail();

        levelMenu.Received(1).DisplayLevelFail();
    }

    [Test]
    public void LevelFail_SetsLevelEnded()
    {
        _gameManager.Construct(levelMenu: Substitute.For<ILevelMenu>());

        GameManager.LevelFail();
        Assert.IsTrue(GameManager.LevelEnded);
    }
}
