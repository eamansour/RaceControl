using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class LapObjectiveTests
{
    private LapObjective _lapObjective;
    private IPlayerManager _player;

    [SetUp]
    public void SetUp()
    {
        _lapObjective = new GameObject().AddComponent<LapObjective>();
        _player = Substitute.For<IPlayerManager>();
        _lapObjective.Construct(_player);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_lapObjective.gameObject);
    }

    [Test]
    public void IsComplete_ReturnsTrueIfRequiredLapCounterIsMet(
        [Values(4, 5, 6)] int lapCounter
    )
    {
        _player.CurrentLap.Returns(lapCounter);
        _lapObjective.Construct(4, false, false, _player);

        bool result = _lapObjective.IsComplete();

        Assert.IsTrue(result);
    }

    [Test]
    public void IsComplete_ReturnsFalseIfRequiredLapCounterIsNotMet(
        [Values(1, 2, 4)] int lapCounter
    )
    {
        _player.CurrentLap.Returns(lapCounter - 1);
        _lapObjective.Construct(5, false, false, _player);

        bool result = _lapObjective.IsComplete();

        Assert.IsFalse(result);
    }

    [Test]
    public void IsCompleteMustWin_ReturnsTrueIfLapCounterIsMetAndIsFirst()
    {
        _player.CurrentLap.Returns(3);
        _player.GetRacePosition().Returns(1);
        _lapObjective.Construct(3, true, false, _player);

        bool result = _lapObjective.IsComplete();

        Assert.IsTrue(result);
    }

    [Test]
    public void IsCompleteMustWin_FailsIfLapCounterIsMetAndIsNotFirst()
    {
        _player.CurrentLap.Returns(3);
        _player.GetRacePosition().Returns(2);
        _lapObjective.Construct(3, true, false, _player);

        bool result = _lapObjective.IsComplete();

        Assert.IsTrue(_lapObjective.Failed);
        Assert.IsFalse(result);
    }

    [Test]
    public void IsCompleteMustChangePlayer_FailsIfLapCounterIsMetAndHasNotChanged()
    {
        GameObject mockObject = new GameObject();

        _player.CurrentLap.Returns(3);
        _player.AttachedGameObject.Returns(mockObject);
        _lapObjective.Construct(3, false, true, _player);

        bool result = _lapObjective.IsComplete();

        Assert.IsTrue(_lapObjective.Failed);
        Assert.IsFalse(result);

        Object.Destroy(mockObject);
    }

    [Test]
    public void IsCompleteMustChangePlayer_ReturnsTrueIfLapCounterIsMetAndChanged()
    {
        GameObject mockObject = new GameObject();
        GameObject mockDifferent = new GameObject();
        IPlayerManager newPlayer = Substitute.For<IPlayerManager>();

        _player.AttachedGameObject.Returns(mockObject);

        newPlayer.CurrentLap.Returns(3);
        newPlayer.AttachedGameObject.Returns(mockDifferent);

        _lapObjective.Construct(3, false, true, _player);
        _lapObjective.Construct(newPlayer);

        bool result = _lapObjective.IsComplete();

        Assert.IsTrue(result);

        Object.Destroy(mockObject);
        Object.Destroy(mockDifferent);
    }
}
