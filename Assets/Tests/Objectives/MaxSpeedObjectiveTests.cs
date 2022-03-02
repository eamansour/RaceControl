using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class MaxSpeedObjectiveTests
{
    private MaxSpeedObjective _maxSpeedObjective;
    private ICar _car;
    private IPlayerManager _player;

    [SetUp]
    public void SetUp()
    {
        _car = Substitute.For<ICar>();
        _player = Substitute.For<IPlayerManager>();
        _player.PlayerCar.Returns(_car);

        _maxSpeedObjective = new GameObject().AddComponent<MaxSpeedObjective>();
        _maxSpeedObjective.Construct(_player);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_maxSpeedObjective.gameObject);
    }

    [Test]
    public void UpdateCompletion_MaxSpeed_FailsIfMaxSpeedExceeded(
        [Values(11, 12)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _maxSpeedObjective.Construct(10);

        _maxSpeedObjective.UpdateCompletion();

        Assert.IsTrue(_maxSpeedObjective.Failed);
    }

    [Test]
    public void UpdateCompletion_MaxSpeed_DoesNotFailIfMaxSpeedNotExceeded(
        [Values(8, 9, 10)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _maxSpeedObjective.Construct(10);

        _maxSpeedObjective.UpdateCompletion();

        Assert.IsFalse(_maxSpeedObjective.Failed);
    }
}
