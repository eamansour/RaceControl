using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using UnityEngine.UI;

[Category("VPLTests")]
public class ConditionTests
{
    private IfCondition _ifCondition;
    private ElifCondition _elifCondition;
    private ElseCondition _elseCondition;

    private IExpression<bool> _expression;
    private ICar _car;
    private IPlayerManager _player;

    private TestHelper _testHelper;
    private GameObject _testObject;

    [SetUp]
    public void SetUp()
    {
        // Create root object
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();
        _testHelper = _testObject.AddComponent<TestHelper>();
        SetParent(new GameObject(), _testObject);

        // Add components to test
        _ifCondition = _testObject.AddComponent<IfCondition>();
        _elifCondition = _testObject.AddComponent<ElifCondition>();
        _elseCondition = _testObject.AddComponent<ElseCondition>();
        
        // Create child object to hold condition statements
        GameObject temp = new GameObject();
        temp.AddComponent<Image>();
        SetParent(temp, _testObject);

        // Create dummy "retire" statement to test calls against
        _car = Substitute.For<ICar>();
        _player = Substitute.For<IPlayerManager>();
        Retire dummyStatement = temp.AddComponent<Retire>();
        dummyStatement.Construct(_car, _player);

        _expression = Substitute.For<IExpression<bool>>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_testObject);
    }

    // Helper method to set an object's parent in the Unity hierarchy
    private void SetParent(GameObject toChild, GameObject parent)
    {
        toChild.transform.SetParent(parent.transform);
        toChild.transform.SetAsLastSibling();
    }

    [Test]
    public void IfCondition_RunsBlockIfTrue()
    {
        _ifCondition.Construct(_expression, false, -1);
        _expression.EvaluateExpression().Returns(true);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_ifCondition.Run());
        
        _player.Received(1).RetirePlayer();
    }

    [Test]
    public void IfCondition_DoesNotRunBlockIfFalse()
    {
        _ifCondition.Construct(_expression, false, -1);
        _expression.EvaluateExpression().Returns(false);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_ifCondition.Run());
        
        _player.DidNotReceive().RetirePlayer();
    }

    [Test]
    public void ElifCondition_RunsBlockIfTrueAndPreviousConditionDidNotRun()
    {
        _elifCondition.Construct(_expression, false, 0);
        _expression.EvaluateExpression().Returns(true);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elifCondition.Run());
        
        _player.Received(1).RetirePlayer();
    }

    [Test]
    public void ElifCondition_DoesNotRunBlockIfPreviousRuns()
    {
        _elifCondition.Construct(_expression, true, 0);
        _expression.EvaluateExpression().Returns(false);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elifCondition.Run());
        
        _player.DidNotReceive().RetirePlayer();
    }

    [Test]
    public void ElifCondition_DoesNotRunBlockIfFalse()
    {
        _elifCondition.Construct(_expression, true, 0);
        _expression.EvaluateExpression().Returns(true);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elifCondition.Run());
        
        _player.DidNotReceive().RetirePlayer();
    }

    [Test]
    public void ElseCondition_RunsBlockIfPreviousDidNotRun()
    {
        _elseCondition.Construct(_expression, false, 0);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elseCondition.Run());
        
        _player.Received().RetirePlayer();
    }

    [Test]
    public void ElseCondition_DoesNotRunBlockIfPreviousRuns()
    {
        _elseCondition.Construct(_expression, true, 0);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elseCondition.Run());
        
        _player.DidNotReceive().RetirePlayer();
    }
}