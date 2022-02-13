using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TestTools;
using System.Collections;

[Category("VPLTests")]
public class CarStatementTests
{
    private ICar _car;
    private IPlayerManager _player;
    private TMP_Dropdown _timerDropdown;

    private TestHelper _testHelper;
    private GameObject _testObject;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();
        _testHelper = _testObject.AddComponent<TestHelper>();
        
        _timerDropdown = new GameObject().AddComponent<TMP_Dropdown>();
        AddTestDropdownOptions(_timerDropdown);
        
        _car = Substitute.For<ICar>();
        _player = Substitute.For<IPlayerManager>();
    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SetActive(true);
            Object.Destroy(go);
        }
    }

    private void AddTestDropdownOptions(TMP_Dropdown testDropdown)
    {
        List<string> options = new List<string> { "5", "4", "3" };
        testDropdown.AddOptions(options);
    }

    [UnityTest]
    public IEnumerator AccelerateStatement_ShouldCallCarAccelerate()
    {
        Accelerate accelerate = _testObject.AddComponent<Accelerate>();
        _timerDropdown.transform.SetParent(accelerate.transform);
        accelerate.Construct(_car, _player, _timerDropdown);

        _testHelper.RunCoroutine(accelerate.Run());
        yield return null;

        _car.Received().Accelerate(5f);
    }

    [UnityTest]
    public IEnumerator BrakeStatement_ShouldCallCarBrake()
    {
        Brake brake = _testObject.AddComponent<Brake>();
        _timerDropdown.transform.SetParent(brake.transform);
        brake.Construct(_car, _player, _timerDropdown);

        _testHelper.RunCoroutine(brake.Run());
        yield return null;

        _car.Received().Brake(5f);
    }

    [UnityTest]
    public IEnumerator TurnStatement_ShouldCallCarTurn()
    {
        Turn turn = _testObject.AddComponent<Turn>();
        TMP_Dropdown directionDropdown = _testObject.AddComponent<TMP_Dropdown>();
        AddTestDropdownOptions(directionDropdown);

        _timerDropdown.transform.SetParent(turn.transform);
        turn.Construct(directionDropdown);
        turn.Construct(_car, _player, _timerDropdown);

        _testHelper.RunCoroutine(turn.Run());
        yield return null;

        _car.ReceivedWithAnyArgs().Turn(default, 5f);
    }

    [Test]
    public void WaitStatement_ShouldNotAffectCar()
    {
        Wait wait = _testObject.AddComponent<Wait>();
        _timerDropdown.transform.SetParent(wait.transform);
        wait.Construct(_car, _player, _timerDropdown);

        _testHelper.RunCoroutine(wait.Run());

        _car.DidNotReceiveWithAnyArgs().Accelerate(default);
        _car.DidNotReceiveWithAnyArgs().Brake(default);
        _car.DidNotReceiveWithAnyArgs().Turn(default, default);
    }

    [UnityTest]
    public IEnumerator Autopilot_ShouldSetControlToAI()
    {
        Autopilot autopilot = _testObject.AddComponent<Autopilot>();
        autopilot.Construct(_car, _player, _timerDropdown);

        _testHelper.RunCoroutine(autopilot.Run());
        yield return null;

        Assert.AreEqual(ControlType.AI, _player.CurrentControl);
    }

    [UnityTest]
    public IEnumerator ManualControl_ShouldSetControlToHuman()
    {
        ManualControl manualControl = _testObject.AddComponent<ManualControl>();
        manualControl.Construct(_car, _player, _timerDropdown);

        _testHelper.RunCoroutine(manualControl.Run());
        yield return null;

        Assert.AreEqual(ControlType.Human, _player.CurrentControl);
    }

    [UnityTest]
    public IEnumerator Retire_ShouldCallPlayerRetire()
    {
        Retire retire = _testObject.AddComponent<Retire>();
        _car.InPit.Returns(true);
        retire.Construct(_car, _player, _timerDropdown);

        _testHelper.RunCoroutine(retire.Run());
        yield return null;

        _player.Received().RetirePlayer();
    }

    [UnityTest]
    public IEnumerator NewCar_ShouldCreateNewPlayer()
    {
        NewCar newCar = _testObject.AddComponent<NewCar>();
        GameObject testPlayer = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Test/TestPlayer"));
        GameObject spawnPoint = new GameObject("CarSpawnPoint");

        newCar.Construct(_car, _player);
        newCar.Construct(testPlayer);

        yield return null;
        _testHelper.RunCoroutine(newCar.Run());
        yield return null;
        
        Assert.IsTrue(GameManager.Players.Count > 1);
    }
}
