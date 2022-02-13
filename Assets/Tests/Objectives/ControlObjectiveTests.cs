using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class ControlObjectiveTests
{
    private ControlObjective _controlObjective;
    private IPlayerManager _player;
    private List<ControlType> _lapControl;

    [SetUp]
    public void SetUp()
    {
        _controlObjective = new GameObject().AddComponent<ControlObjective>();
        _player = Substitute.For<IPlayerManager>();
        _lapControl = new List<ControlType>();

        _controlObjective.Construct(_player);
        _controlObjective.Construct(_lapControl);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_controlObjective.gameObject);
    }


    [Test]
    public void UpdateCompletion_PassesObjectiveIfLapCounterIsGreaterThanControlListCount()
    {
        _lapControl.Add(ControlType.Human);
        _player.CurrentLap.Returns(4);

        _controlObjective.UpdateCompletion();

        Assert.IsTrue(_controlObjective.Passed);
    }

    [Test]
    public void UpdateCompletion_DoesNotPassIfLapCounterIsLessThanListCountAndCorrectControlIsSet()
    {
        _lapControl.Add(ControlType.Human);
        _player.CurrentLap.Returns(0);
        _player.CurrentControl.Returns(ControlType.Human);

        _controlObjective.UpdateCompletion();

        Assert.IsFalse(_controlObjective.Passed);
    }

    [Test]
    public void UpdateCompletion_FailsIfWrongControlMethodIsSet()
    {
        _lapControl.Add(ControlType.Human);
        _player.CurrentLap.Returns(0);
        _player.CurrentControl.Returns(ControlType.Program);

        _controlObjective.UpdateCompletion();

        Assert.IsTrue(_controlObjective.Failed);
    }
}
