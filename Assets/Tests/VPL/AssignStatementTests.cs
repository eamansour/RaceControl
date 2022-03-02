using NUnit.Framework;
using UnityEngine;
using TMPro;
using NSubstitute;
using UnityEngine.UI;
using System.Collections.Generic;

[Category("VPLTests")]
public class AssignStatementTests
{
    private AssignFloat _assignFloat;
    private AssignNode _assignNode;
    private TestHelper _testHelper;
    private TMP_InputField _variableInput;
    private TMP_Dropdown _variableDropdown;
    private IExpression<float> _floatExpression;
    private IExpression<Node<IPlayerManager>> _nodeExpression;
    private ICar _car;
    private IObstacleSpawn _spawner;
    private GameObject _testObject;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();

        _assignFloat = _testObject.AddComponent<AssignFloat>();
        _assignNode = _testObject.AddComponent<AssignNode>();

        _testHelper = _testObject.AddComponent<TestHelper>();
        _variableInput = _testObject.AddComponent<TMP_InputField>();
        _variableDropdown = new GameObject().AddComponent<TMP_Dropdown>();

        _floatExpression = Substitute.For<IExpression<float>>();
        _nodeExpression = Substitute.For<IExpression<Node<IPlayerManager>>>();

        _car = Substitute.For<ICar>();
        _spawner = Substitute.For<IObstacleSpawn>();

        _assignFloat.Construct(_car, _floatExpression, _spawner, _variableInput);
        _assignNode.Construct(_car, _nodeExpression, _spawner, variableDropdown: _variableDropdown);

        Statement.SetUpEnvironment();
    }

    [TearDown]
    public void TearDown()
    {
        _variableDropdown.ClearOptions();
        Object.Destroy(_testObject);
        Object.Destroy(_variableDropdown.gameObject);
    }

    [Test]
    public void AssignFloatRun_AddsFloatVariableToEnvironmentIfNew(
        [Values(5.1f, 30f, -10f)] float result
    )
    {
        _variableInput.text = "test";
        _floatExpression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignFloat.Run());

        Assert.IsTrue(Statement.Environment.ContainsKey("test"));
        Assert.AreEqual(result, Statement.Environment["test"]);
    }

    [Test]
    public void AssignFloatRun_UpdatesFloatVariableInEnvironmentIfExists(
        [Values(5.1f, 30f, -10f)] float result
    )
    {
        Statement.Environment.Add("test", 1f);

        _variableInput.text = "test";
        _floatExpression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignFloat.Run());

        Assert.AreEqual(result, Statement.Environment["test"]);
    }

    [Test]
    public void AssignNodeRun_AddsNodeVariableToEnvironmentIfNew()
    {
        _assignNode.Construct(false);
        _variableDropdown.AddOptions(new List<string> { "test" });

        Node<IPlayerManager> testNode = new Node<IPlayerManager>();
        _nodeExpression.EvaluateExpression().Returns(testNode);

        _testHelper.RunCoroutine(_assignNode.Run());

        Assert.IsTrue(Statement.Environment.ContainsKey("test"));
        Assert.AreEqual(testNode, Statement.Environment["test"]);
    }

    [Test]
    public void AssignNodeRun_UpdatesNextNodeIfSet()
    {
        _assignNode.Construct(true);
        _variableDropdown.AddOptions(new List<string> { "test" });

        Node<IPlayerManager> testNode = new Node<IPlayerManager>();
        Node<IPlayerManager> nextNode = new Node<IPlayerManager>();
        Statement.Environment.Add("test", testNode);

        _nodeExpression.EvaluateExpression().Returns(nextNode);

        _testHelper.RunCoroutine(_assignNode.Run());

        Assert.AreEqual(nextNode, testNode.Next);
    }

    [Test]
    public void AssignFloatRun_ReducesCarFuelAndVariable(
        [Values(5.1f, 30f, -10f)] float result
    )
    {
        _car.Fuel.Returns(100f);

        _variableInput.text = "fuel";
        _floatExpression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignFloat.Run());

        _car.Received(1).Fuel = result;
        Assert.AreEqual(result, Statement.Environment["fuel"]);
    }

    [Test]
    public void AssignFloatRun_SpawnsObstacleWhenReducingFuel(
        [Values(5.1f, 30f, -10f)] float result
    )
    {
        _car.Fuel.Returns(100f);

        _variableInput.text = "fuel";
        _floatExpression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignFloat.Run());

        _spawner.Received(1).SpawnObstacle();
    }

    [Test]
    public void AssignFloatRun_DoesNotIncreaseFuelOrSpawnObstacle(
        [Values(100f, 101f, 100.1f)] float result
    )
    {
        _car.Fuel.Returns(100f);

        _variableInput.text = "fuel";
        _floatExpression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignFloat.Run());

        _car.DidNotReceive().Fuel = result;
        _spawner.DidNotReceive().SpawnObstacle();
    }

    [Test]
    public void AssignFloatRun_AddsNAVariableWhenNoVariableIsInputted(
        [Values(-1f, 0f, 1f)] float result
    )
    {
        _variableInput.text = "";
        _floatExpression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignFloat.Run());

        Assert.IsTrue(Statement.Environment.ContainsKey("NA"));
        Assert.AreEqual(result, Statement.Environment["NA"]);
    }
}
