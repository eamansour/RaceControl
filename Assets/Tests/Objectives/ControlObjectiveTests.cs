using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class ControlObjectiveTests
{
    private ControlObjective _controlObjective;
    private IPlayerManager _player;
    private List<PlayerManager.ControlMethod> _lapControl;

    [SetUp]
    public void SetUp()
    {
        _controlObjective = new GameObject().AddComponent<ControlObjective>();
        _player = Substitute.For<IPlayerManager>();
        _lapControl = new List<PlayerManager.ControlMethod>();

        _controlObjective.Construct(_player);
        _controlObjective.Construct(_lapControl);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_controlObjective.gameObject);
    }


    [Test]
    public void IsComplete_ReturnsTrueIfLapCounterIsGreaterThanControlListCount()
    {
        _lapControl.Add(PlayerManager.ControlMethod.Human);
        _player.CurrentLap.Returns(4);

        bool result = _controlObjective.IsComplete();

        Assert.IsTrue(result);
    }

    [Test]
    public void IsComplete_ReturnsFalseIfLapCounterIsLessThanListCountAndCorrectControlIsSet()
    {
        _lapControl.Add(PlayerManager.ControlMethod.Human);
        _player.CurrentLap.Returns(0);
        _player.CurrentControl.Returns(PlayerManager.ControlMethod.Human);

        bool result = _controlObjective.IsComplete();

        Assert.IsFalse(result);
    }

    [Test]
    public void IsComplete_ReturnsFalseAndFailsIfWrongControlMethodIsSet()
    {
        _lapControl.Add(PlayerManager.ControlMethod.Human);
        _player.CurrentLap.Returns(0);
        _player.CurrentControl.Returns(PlayerManager.ControlMethod.Program);

        bool result = _controlObjective.IsComplete();

        Assert.IsFalse(result);
        Assert.IsTrue(_controlObjective.Failed);

    }
}
