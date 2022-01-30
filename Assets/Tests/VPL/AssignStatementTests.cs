using NUnit.Framework;
using UnityEngine;
using TMPro;
using NSubstitute;
using UnityEngine.UI;

[Category("VPLTests")]
public class AssignStatementTests
{
    private AssignFloat _assignStatement;
    private TestHelper _testHelper;
    private TMP_InputField _variableInput;
    private IExpression<float> _expression;
    private ICar _car;
    private IObstacleSpawn _spawner;
    private GameObject _testObject;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();
        _assignStatement = _testObject.AddComponent<AssignFloat>();
        _testHelper = _testObject.AddComponent<TestHelper>();
        _variableInput = _testObject.AddComponent<TMP_InputField>();

        _expression = Substitute.For<IExpression<float>>();
        _car = Substitute.For<ICar>();
        _spawner = Substitute.For<IObstacleSpawn>();

        _assignStatement.Construct(_car, _expression, _variableInput, _spawner);
        Statement.SetUpEnvironment();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_testObject);
    }

    [Test]
    public void AssignRun_AddsVariableToEnvironmentIfNew(
        [Values(5.1f, 30f, -10f)] float result
    )
    {
        _variableInput.text = "test";
        _expression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignStatement.Run());

        Assert.IsTrue(Statement.Environment.ContainsKey("test"));
        Assert.AreEqual(result, Statement.Environment["test"]);
    }

    [Test]
    public void AssignRun_UpdatesVariableInEnvironmentIfExists(
        [Values(5.1f, 30f, -10f)] float result
    )
    {
        Statement.Environment.Add("test", 1f);

        _variableInput.text = "test";
        _expression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignStatement.Run());

        Assert.AreEqual(result, Statement.Environment["test"]);
    }

    [Test]
    public void AssignRun_ReducesCarFuelAndVariable(
        [Values(5.1f, 30f, -10f)] float result
    )
    {
        _car.Fuel.Returns(100f);

        _variableInput.text = "fuel";
        _expression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignStatement.Run());

        _car.Received(1).Fuel = result;
        Assert.AreEqual(result, Statement.Environment["fuel"]);
    }

    [Test]
    public void AssignRun_SpawnsObstacleWhenReducingFuel(
        [Values(5.1f, 30f, -10f)] float result
    )
    {
        _car.Fuel.Returns(100f);

        _variableInput.text = "fuel";
        _expression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignStatement.Run());

        _spawner.Received(1).SpawnObstacle();
    }

    [Test]
    public void AssignRun_DoesNotIncreaseFuelOrSpawnObstacle(
        [Values(100f, 101f, 100.1f)] float result
    )
    {
        _car.Fuel.Returns(100f);

        _variableInput.text = "fuel";
        _expression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignStatement.Run());

        _car.DidNotReceive().Fuel = result;
        _spawner.DidNotReceive().SpawnObstacle();
    }

    [Test]
    public void AssignRun_AddsNAVariableWhenNoVariableIsInputted(
        [Values(-1f, 0f, 1f)] float result
    )
    {
        _variableInput.text = "";
        _expression.EvaluateExpression().Returns(result);

        _testHelper.RunCoroutine(_assignStatement.Run());

        Assert.IsTrue(Statement.Environment.ContainsKey("NA"));
        Assert.AreEqual(result, Statement.Environment["NA"]);
    }
}
