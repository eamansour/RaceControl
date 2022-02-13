using NUnit.Framework;
using UnityEngine;
using NSubstitute;

[Category("ObjectiveTests")]
public class MinSpeedObjectiveTests
{
    private MinSpeedObjective _minSpeedObjective;
    private ICar _car;
    private IPlayerManager _player;    

    [SetUp]
    public void SetUp()
    {
        _car = Substitute.For<ICar>();
        _player = Substitute.For<IPlayerManager>();
        _player.PlayerCar.Returns(_car);

        _minSpeedObjective = new GameObject().AddComponent<MinSpeedObjective>();
        _minSpeedObjective.Construct(_player);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_minSpeedObjective.gameObject);
    }

    [Test]
    public void UpdateCompletion_MinSpeed_PassesObjectiveIfMinSpeedIsMet(
        [Values(40, 41, 50)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _minSpeedObjective.Construct(40);

        _minSpeedObjective.UpdateCompletion();

        Assert.IsTrue(_minSpeedObjective.Passed);
    }

    [Test]
    public void UpdateCompletion_MinSpeed_DoesNotPassIfMinSpeedIsNotMet(
        [Values(10, 45, 49)] int speed
    )
    {
        _car.GetSpeedInMPH().Returns(speed);
        _minSpeedObjective.Construct(50);

        _minSpeedObjective.UpdateCompletion();

        Assert.IsFalse(_minSpeedObjective.Passed);
    }
}
