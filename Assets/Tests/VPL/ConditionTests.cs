using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using UnityEngine.UI;
using UnityEngine.TestTools;
using System.Collections;

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
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SetActive(true);
            Object.Destroy(go);
        }
    }

    // Helper method to set an object's parent in the Unity hierarchy
    private void SetParent(GameObject toChild, GameObject parent)
    {
        toChild.transform.SetParent(parent.transform);
        toChild.transform.SetAsLastSibling();
    }

    [UnityTest]
    public IEnumerator IfCondition_RunsBlockIfTrue()
    {
        _ifCondition.Construct(_expression, false, -1);
        _expression.EvaluateExpression().Returns(true);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_ifCondition.Run());
        yield return null;
        
        _player.Received(1).RetirePlayer();
    }

    [UnityTest]
    public IEnumerator IfCondition_DoesNotRunBlockIfFalse()
    {
        _ifCondition.Construct(_expression, false, -1);
        _expression.EvaluateExpression().Returns(false);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_ifCondition.Run());
        yield return null;
        
        _player.DidNotReceive().RetirePlayer();
    }

    [UnityTest]
    public IEnumerator ElifCondition_RunsBlockIfTrueAndPreviousConditionDidNotRun()
    {
        _elifCondition.Construct(_expression, false, 0);
        _expression.EvaluateExpression().Returns(true);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elifCondition.Run());
        yield return null;
        
        _player.Received(1).RetirePlayer();
    }

    [UnityTest]
    public IEnumerator ElifCondition_DoesNotRunBlockIfPreviousRuns()
    {
        _elifCondition.Construct(_expression, true, 0);
        _expression.EvaluateExpression().Returns(false);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elifCondition.Run());
        yield return null;
        
        _player.DidNotReceive().RetirePlayer();
    }

    [UnityTest]
    public IEnumerator ElifCondition_DoesNotRunBlockIfFalse()
    {
        _elifCondition.Construct(_expression, true, 0);
        _expression.EvaluateExpression().Returns(true);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elifCondition.Run());
        yield return null;
        
        _player.DidNotReceive().RetirePlayer();
    }

    [UnityTest]
    public IEnumerator ElseCondition_RunsBlockIfPreviousDidNotRun()
    {
        _elseCondition.Construct(_expression, false, 0);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elseCondition.Run());
        yield return null;
        
        _player.Received().RetirePlayer();
    }

    [UnityTest]
    public IEnumerator ElseCondition_DoesNotRunBlockIfPreviousRuns()
    {
        _elseCondition.Construct(_expression, true, 0);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_elseCondition.Run());
        yield return null;
        
        _player.DidNotReceive().RetirePlayer();
    }
}