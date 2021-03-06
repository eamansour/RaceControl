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
    private TMP_Dropdown _testDropdown;

    private TestHelper _testHelper;
    private GameObject _testObject;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();
        _testHelper = _testObject.AddComponent<TestHelper>();

        _testDropdown = new GameObject().AddComponent<TMP_Dropdown>();
        AddTestDropdownOptions(_testDropdown);

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
        GameManager.Players.Clear();
        Statement.Environment.Clear();
    }

    private void AddTestDropdownOptions(TMP_Dropdown testDropdown)
    {
        List<string> options = new List<string> { "5", "1", "i" };
        testDropdown.AddOptions(options);
    }

    [UnityTest]
    public IEnumerator AccelerateStatement_ShouldCallCarAccelerate()
    {
        Accelerate accelerate = _testObject.AddComponent<Accelerate>();
        _testDropdown.transform.SetParent(accelerate.transform);
        accelerate.Construct(_car, _player, _testDropdown);

        _testHelper.RunCoroutine(accelerate.Run());
        yield return null;

        _car.Received().Accelerate(5f);
    }

    [UnityTest]
    public IEnumerator BrakeStatement_ShouldCallCarBrake()
    {
        Brake brake = _testObject.AddComponent<Brake>();
        _testDropdown.transform.SetParent(brake.transform);
        brake.Construct(_car, _player, _testDropdown);

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

        _testDropdown.transform.SetParent(turn.transform);
        turn.Construct(directionDropdown);
        turn.Construct(_car, _player, _testDropdown);

        _testHelper.RunCoroutine(turn.Run());
        yield return null;

        _car.ReceivedWithAnyArgs().Turn(default, 5f);
    }

    [Test]
    public void WaitStatement_ShouldNotAffectCar()
    {
        Wait wait = _testObject.AddComponent<Wait>();
        _testDropdown.transform.SetParent(wait.transform);
        wait.Construct(_testDropdown);

        _testHelper.RunCoroutine(wait.Run());

        _car.DidNotReceiveWithAnyArgs().Accelerate(default);
        _car.DidNotReceiveWithAnyArgs().Brake(default);
        _car.DidNotReceiveWithAnyArgs().Turn(default, default);
    }

    [UnityTest]
    public IEnumerator Autopilot_ShouldSetControlToAI()
    {
        Autopilot autopilot = _testObject.AddComponent<Autopilot>();
        autopilot.Construct(_car, _player, _testDropdown);

        _testHelper.RunCoroutine(autopilot.Run());
        yield return null;

        Assert.AreEqual(ControlMode.AI, _player.CurrentControl);
    }

    [UnityTest]
    public IEnumerator ManualControl_ShouldSetControlToHuman()
    {
        ManualControl manualControl = _testObject.AddComponent<ManualControl>();
        manualControl.Construct(_car, _player, _testDropdown);

        _testHelper.RunCoroutine(manualControl.Run());
        yield return null;

        Assert.AreEqual(ControlMode.Human, _player.CurrentControl);
    }

    [UnityTest]
    public IEnumerator Retire_ShouldCallPlayerRetireWhenInPit()
    {
        Retire retire = _testObject.AddComponent<Retire>();
        _car.InPit.Returns(true);
        retire.Construct(_car, _player, _testDropdown);

        _testHelper.RunCoroutine(retire.Run());
        yield return null;

        _player.Received().RetirePlayer();
    }

    [UnityTest]
    public IEnumerator Retire_ShouldNotCallPlayerRetireWhenNotInPit()
    {
        Retire retire = _testObject.AddComponent<Retire>();
        _car.InPit.Returns(false);
        retire.Construct(_car, _player, _testDropdown);

        _testHelper.RunCoroutine(retire.Run());
        yield return null;

        _player.DidNotReceive().RetirePlayer();
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

    [UnityTest]
    public IEnumerator CarList_ShouldUpdateCurrentPlayer()
    {
        CarListStatement carList = _testObject.AddComponent<CarListStatement>();
        Autopilot dummyStatement = _testObject.AddComponent<Autopilot>();

        IPlayerManager newPlayer = Substitute.For<IPlayerManager>();
        GameManager.Players.AddRange(new List<IPlayerManager> { _player, newPlayer });

        // Select player at index 1 (newPlayer)
        _testDropdown.value = 1;

        carList.Construct(_car, _player, _testDropdown);
        carList.Construct(dummyStatement);

        yield return null;
        _testHelper.RunCoroutine(carList.Run());
        yield return null;

        Assert.AreSame(newPlayer, GameManager.CurrentPlayer);
    }

    [UnityTest]
    public IEnumerator CarList_ShouldRunStatementOnNewPlayer()
    {
        CarListStatement carList = _testObject.AddComponent<CarListStatement>();
        Autopilot dummyStatement = _testObject.AddComponent<Autopilot>();

        IPlayerManager newPlayer = Substitute.For<IPlayerManager>();
        GameManager.Players.AddRange(new List<IPlayerManager> { _player, newPlayer });

        // Select player at index 1 (newPlayer)
        _testDropdown.value = 1;

        carList.Construct(_car, _player, _testDropdown);
        carList.Construct(dummyStatement);

        yield return null;
        _testHelper.RunCoroutine(carList.Run());
        yield return null;

        // Should have executed the dummy "autopilot" statement
        newPlayer.Received(1).CurrentControl = ControlMode.AI;
    }

    [UnityTest]
    public IEnumerator CarList_ShouldSetPlayerWithValidIndexVariable()
    {
        CarListStatement carList = _testObject.AddComponent<CarListStatement>();
        Autopilot dummyStatement = _testObject.AddComponent<Autopilot>();

        IPlayerManager newPlayer = Substitute.For<IPlayerManager>();
        GameManager.Players.AddRange(new List<IPlayerManager> { _player, newPlayer });
        Statement.Environment.Add("i", 1);

        // Select "i" option
        _testDropdown.value = 2;

        carList.Construct(_car, _player, _testDropdown);
        carList.Construct(dummyStatement);

        yield return null;
        _testHelper.RunCoroutine(carList.Run());
        yield return null;

        // Should have executed the dummy "autopilot" statement
        newPlayer.Received(1).CurrentControl = ControlMode.AI;
    }

    [UnityTest]
    public IEnumerator CarList_ShouldSetZeroPlayerWithInvalidIndexVariable()
    {
        CarListStatement carList = _testObject.AddComponent<CarListStatement>();
        Autopilot dummyStatement = _testObject.AddComponent<Autopilot>();

        IPlayerManager newPlayer = Substitute.For<IPlayerManager>();
        GameManager.Players.AddRange(new List<IPlayerManager> { _player, newPlayer });
        Statement.Environment.Add("i", 5);

        // Select "i" option
        _testDropdown.value = 2;

        carList.Construct(_car, _player, _testDropdown);
        carList.Construct(dummyStatement);

        yield return null;
        _testHelper.RunCoroutine(carList.Run());
        yield return null;

        // Should have executed the dummy "autopilot" statement
        _player.Received(1).CurrentControl = ControlMode.AI;
    }
}
