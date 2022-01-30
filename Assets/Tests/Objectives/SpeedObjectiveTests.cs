using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class SpeedObjectiveTests
{
    private MinSpeedObjective _minSpeedObjective;
    private MaxSpeedObjective _maxSpeedObjective;
    private ICar _car;
    private IPlayerManager _player;    

    [SetUp]
    public void SetUp()
    {
        _car = Substitute.For<ICar>();
        _player = Substitute.For<IPlayerManager>();
        _player.PlayerCar.Returns(_car);

        _minSpeedObjective = new GameObject().AddComponent<MinSpeedObjective>();
        _maxSpeedObjective = new GameObject().AddComponent<MaxSpeedObjective>();

        _minSpeedObjective.Construct(_player);
        _maxSpeedObjective.Construct(_player);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_minSpeedObjective.gameObject);
        Object.Destroy(_maxSpeedObjective.gameObject);
    }

    [Test]
    public void IsComplete_MinSpeed_ReturnsTrueIfMinSpeedIsMet(
        [Values(40, 41, 50)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _minSpeedObjective.Construct(40);

        bool result = _minSpeedObjective.IsComplete();

        Assert.IsTrue(result);
    }

    [Test]
    public void IsComplete_MinSpeed_ReturnsFalseIfMinSpeedIsNotMet(
        [Values(10, 45, 49)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _minSpeedObjective.Construct(50);

        bool result = _minSpeedObjective.IsComplete();

        Assert.IsFalse(result);
    }
}
