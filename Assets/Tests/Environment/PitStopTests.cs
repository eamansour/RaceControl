using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[Category("EnvironmentTests")]
public class PitStopTests
{
    private PitStop _pitStop;
    private GameObject _testPlayer;
    private IPlayerManager _playerManager;
    private Car _car;

    [SetUp]
    public void SetUp()
    {
        GameObject pitStopObject = new GameObject();
        _pitStop = pitStopObject.AddComponent<PitStop>();
        pitStopObject.AddComponent<BoxCollider>().isTrigger = true;
        Checkpoint exitCheckpoint = pitStopObject.AddComponent<Checkpoint>();
        _pitStop.Construct(exitCheckpoint);

        _testPlayer = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Test/TestPlayer"));
        _car = _testPlayer.GetComponent<Car>();
        _playerManager = _testPlayer.GetComponent<IPlayerManager>();
        _playerManager.Construct(_car, _testPlayer.GetComponent<ICarAI>(), exitCheckpoint);

        _testPlayer.SetActive(false);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_pitStop.gameObject);
        Object.Destroy(_testPlayer.gameObject);
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter_DoesNotServicePlayerIfNotAIControlled()
    {
        _testPlayer.SetActive(true);
        yield return null;

        Assert.IsTrue(_pitStop.IsFree);
        Assert.IsFalse(_car.InPit);
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter_DoesNotServiceObjectIfNotAPlayer()
    {
        BoxCollider testCollider = new GameObject().AddComponent<BoxCollider>();
        yield return null;

        Assert.IsTrue(_pitStop.IsFree);
        Assert.IsFalse(_car.InPit);

        Object.Destroy(testCollider.gameObject);
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter_PitsCarWhenAIControlled()
    {
        _playerManager.CurrentControl = PlayerManager.ControlMethod.AI;
        _car.Fuel = 50f;
        _testPlayer.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        Assert.IsFalse(_pitStop.IsFree);
        Assert.IsTrue(_car.InPit);
    }

    [UnityTest]
    public IEnumerator ServiceCar_RefuelsCarWhenBelowOneHundred()
    {
        _playerManager.CurrentControl = PlayerManager.ControlMethod.AI;
        _car.Fuel = 50f;
        _testPlayer.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(_car.Fuel > 50f);
    }

    [UnityTest]
    public IEnumerator ServiceCar_DoesNotRefuelAboveOneHundred()
    {
        _playerManager.CurrentControl = PlayerManager.ControlMethod.AI;
        _car.Fuel = 95f;
        _testPlayer.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(100f, _car.Fuel);
    }
}
