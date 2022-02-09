using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

[Category("PlayerTests")]
public class PlayerManagerTests
{
    private PlayerManager _playerManager;
    private GameObject _testObject;
    private Checkpoint _testCheckpoint;
    private ICar _car;
    private ICarAI _carAI;


    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _playerManager = _testObject.AddComponent<PlayerManager>();
        _testObject.AddComponent<GameManager>();
        _testObject.AddComponent<InputController>();

        _car = Substitute.For<ICar>();
        _carAI = Substitute.For<ICarAI>();
        _testCheckpoint = new GameObject().AddComponent<Checkpoint>();

        _playerManager.Construct(_car, _carAI, _testCheckpoint);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_testObject);
        Object.Destroy(_testCheckpoint.gameObject);
    }

    [Test]
    public void StartPlayer_UnlocksPlayerCar()
    {
        _playerManager.StartPlayer();

        _car.Received(1).SetCarLock(false);
    }

    [Test]
    public void DistanceToTarget_ReturnsDistanceFromPlayerToTargetCheckpoint(
        [Values(10, -5, 0)] int target
    )
    {
        _testCheckpoint.transform.position = new Vector3(target, 0, 0);
        float distance = _playerManager.DistanceToTarget();

        Assert.AreEqual(Mathf.Abs(target), distance);
    }

    [Test]
    public void GetRacePosition_ReturnsPlayerPositionInLevel()
    {
        IPlayerManager otherPlayer = Substitute.For<IPlayerManager>();
        GameManager.Players.Insert(0, otherPlayer);

        int position = _playerManager.GetRacePosition();

        Assert.AreEqual(2, position);
    }

    [Test]
    public void RetirePlayer_LocksAndResetsCarControl()
    {
        _playerManager.RetirePlayer();

        _car.Received(1).ResetControl();
        _car.Received(1).SetCarLock(true);
    }

    [Test]
    public void SetRaceProgress_UpdatesPlayerRaceInformation()
    {
        Checkpoint newCheckpoint = new GameObject().AddComponent<Checkpoint>();
        _playerManager.SetRaceProgress(5, PlayerManager.ControlMethod.AI, newCheckpoint);

        Assert.AreEqual(5, _playerManager.CurrentLap);
        Assert.AreEqual(PlayerManager.ControlMethod.AI, _playerManager.CurrentControl);
        Assert.AreEqual(newCheckpoint, _playerManager.TargetCheckpoint);

        Object.Destroy(newCheckpoint.gameObject);
    }

    [Test]
    public void ResetPlayer_LocksCarAndResetsControl()
    {
        _playerManager.ResetPlayer();

        _car.Received(1).SetCarLock(true);
        _car.Received(1).ResetControl();
    }

    [Test]
    public void ResetPlayer_ResetsPlayerPosition()
    {
        _testObject.transform.position = new Vector3(10, 5, 5);
        _playerManager.ResetPlayer();

        Assert.AreEqual(Vector3.zero, _testObject.transform.position);
    }

    [Test]
    public void ResetPlayer_ResetsFuelAndRaceProgress()
    {
        Checkpoint newCheckpoint = new GameObject().AddComponent<Checkpoint>();
        _car.Fuel = 50f;
        _playerManager.SetRaceProgress(10, PlayerManager.ControlMethod.AI, newCheckpoint);

        _playerManager.ResetPlayer();

        Assert.AreEqual(100f, _car.Fuel);
        Assert.AreEqual(0, _playerManager.CurrentLap);
        Assert.AreEqual(_testCheckpoint, _playerManager.TargetCheckpoint);

        Object.Destroy(newCheckpoint.gameObject);
    }
}
