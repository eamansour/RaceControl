using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class SpeedObjectiveTests
{
    private SpeedObjective _speedObjective;
    private ICar _car;
    private IPlayerManager _player;

    [SetUp]
    public void SetUp()
    {
        _car = Substitute.For<ICar>();
        _player = Substitute.For<IPlayerManager>();
        _player.PlayerCar.Returns(_car);

        _speedObjective = new GameObject().AddComponent<SpeedObjective>();
        _speedObjective.Construct(_player);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_speedObjective.gameObject);
    }

    [Test]
    public void UpdateCompletion_MinSpeed_PassesObjectiveIfMinSpeedIsMet(
        [Values(40, 41, 50)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _speedObjective.Construct(40);

        _speedObjective.UpdateCompletion();

        Assert.IsTrue(_speedObjective.Passed);
    }

    [Test]
    public void UpdateCompletion_MinSpeed_DoesNotPassIfMinSpeedIsNotMet(
        [Values(10, 45, 49)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _speedObjective.Construct(50);

        _speedObjective.UpdateCompletion();

        Assert.IsFalse(_speedObjective.Passed);
    }

    [Test]
    public void UpdateCompletion_MaxSpeed_FailsIfMaxSpeedExceeded(
        [Values(11, 12)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _speedObjective.Construct(10, true);

        _speedObjective.UpdateCompletion();

        Assert.IsTrue(_speedObjective.Failed);
    }

    [Test]
    public void UpdateCompletion_MaxSpeed_DoesNotFailIfMaxSpeedNotExceeded(
        [Values(8, 9, 10)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _speedObjective.Construct(10, true);

        _speedObjective.UpdateCompletion();

        Assert.IsFalse(_speedObjective.Failed);
    }
}
