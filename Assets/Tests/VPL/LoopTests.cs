using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[Category("VPLTests")]
public class LoopTests
{
    private WhileLoop _whileLoop;
    private ForLoop _forLoop;

    private IExpression<bool> _expression;
    private ICar _car;
    private IPlayerManager _player;

    private TestHelper _testHelper;
    private GameObject _testObject;

    private TMP_Dropdown _rangeStartDropdown;
    private TMP_Dropdown _rangeEndDropdown;
    private TMP_Dropdown _incrementDropdown;

    [SetUp]
    public void SetUp()
    {
        // Create root object
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();
        _testHelper = _testObject.AddComponent<TestHelper>();
        SetParent(new GameObject(), _testObject);

        // Add components to test
        _whileLoop = _testObject.AddComponent<WhileLoop>();
        _forLoop = _testObject.AddComponent<ForLoop>();
        
        // Create child object to hold loop statements
        GameObject temp = new GameObject();
        temp.AddComponent<Image>();
        SetParent(temp, _testObject);

        // Create dummy "retire" statement to test calls against
        _car = Substitute.For<ICar>();
        _player = Substitute.For<IPlayerManager>();
        Retire dummyStatement = temp.AddComponent<Retire>();
        dummyStatement.Construct(_car, _player);

        _expression = Substitute.For<IExpression<bool>>();

        _rangeStartDropdown = CreateTestDropdown();
        _rangeEndDropdown = CreateTestDropdown();
        _incrementDropdown = CreateTestDropdown();

        _whileLoop.Construct(_expression);
        _forLoop.Construct(_rangeStartDropdown, _rangeEndDropdown, _incrementDropdown);
        Statement.SetUpEnvironment();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_testObject);
        Object.Destroy(_rangeStartDropdown.gameObject);
        Object.Destroy(_rangeEndDropdown.gameObject);
        Object.Destroy(_incrementDropdown.gameObject);
    }

    // Helper method to set an object's parent in the Unity hierarchy
    private void SetParent(GameObject toChild, GameObject parent)
    {
        toChild.transform.SetParent(parent.transform);
        toChild.transform.SetAsLastSibling();
    }

    private TMP_Dropdown CreateTestDropdown()
    {
        TMP_Dropdown testDropdown = new GameObject().AddComponent<TMP_Dropdown>();
        List<string> options = new List<string> { 
            "0",
            "1",
            "2",
            "len(cars)",
            "len(cars) - 1"
        };
        testDropdown.AddOptions(options);
        return testDropdown;
    }

    [UnityTest]
    public IEnumerator WhileLoop_DoesNotRunBlockIfFalse()
    {
        _expression.EvaluateExpression().Returns(false);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_whileLoop.Run());
        yield return null;

        _player.DidNotReceive().RetirePlayer();
    }

    [UnityTest]
    public IEnumerator WhileLoop_RepeatsBlockWhileTrue()
    {
        _expression.EvaluateExpression().Returns(true);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_whileLoop.Run());
        yield return null;

        // Test three iterations
        _player.Received(1).RetirePlayer();
        yield return null;

        _player.Received(2).RetirePlayer();
        yield return null;

        _player.Received(3).RetirePlayer();
        _expression.EvaluateExpression().Returns(false);
    }

    [UnityTest]
    public IEnumerator WhileLoop_StopsRepeatingBlockWhenFalse()
    {
        _expression.EvaluateExpression().Returns(true);
        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_whileLoop.Run());
        yield return null;

        _player.Received(1).RetirePlayer();
        _expression.EvaluateExpression().Returns(false);
        yield return null;

        _player.Received(2).RetirePlayer();
        yield return null;

        // While loop should have stopped
        _player.Received(2).RetirePlayer();
    }

    [UnityTest]
    public IEnumerator ForLoop_DoesNotRunBlockIfEnded()
    {
        _rangeStartDropdown.value = 0;
        _rangeEndDropdown.value = 0;
        _incrementDropdown.value = 1;

        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_forLoop.Run());
        yield return null;

        _player.DidNotReceive().RetirePlayer();
    }

    [UnityTest]
    public IEnumerator ForLoop_RepeatsBlockUntilEnd()
    {
        _rangeStartDropdown.value = 0;
        _rangeEndDropdown.value = 2;
        _incrementDropdown.value = 1;

        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_forLoop.Run());
        yield return null;

        // Test two iterations
        _player.Received(1).RetirePlayer();
        yield return null;

        _player.Received(2).RetirePlayer();
    }

    [UnityTest]
    public IEnumerator ForLoop_StopsRepeatingBlockWhenEnded()
    {
        _rangeStartDropdown.value = 0;
        _rangeEndDropdown.value = 2;
        _incrementDropdown.value = 1;

        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_forLoop.Run());
        yield return null;

        // Test two iterations
        _player.Received(1).RetirePlayer();
        yield return null;

        _player.Received(2).RetirePlayer();
        yield return null;

        // For loop should have ended
        _player.Received(2).RetirePlayer();
    }

    [Test]
    public void ForLoop_AddsIndexToEnvironment()
    {
        _rangeStartDropdown.value = 0;
        _rangeEndDropdown.value = 1;
        _incrementDropdown.value = 1;

        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_forLoop.Run());

        Assert.AreEqual(0, Statement.Environment["i"]);
    }

    [UnityTest]
    public IEnumerator ForLoop_UpdatesIndexInEnvironment()
    {
        _rangeStartDropdown.value = 0;
        _rangeEndDropdown.value = 2;
        _incrementDropdown.value = 1;

        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_forLoop.Run());

        Assert.AreEqual(0, Statement.Environment["i"]);
        yield return null;

        Assert.AreEqual(1, Statement.Environment["i"]);
    }

    [UnityTest]
    public IEnumerator ForLoop_RemovesIndexIfPreviouslyNonExistent()
    {
        _rangeStartDropdown.value = 0;
        _rangeEndDropdown.value = 1;
        _incrementDropdown.value = 1;

        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_forLoop.Run());

        Assert.AreEqual(0, Statement.Environment["i"]);
        yield return null;

        Assert.IsFalse(Statement.Environment.ContainsKey("i"));
    }

    [UnityTest]
    public IEnumerator ForLoop_ReturnsIndexToPreviousValueIfExisted()
    {
        _rangeStartDropdown.value = 0;
        _rangeEndDropdown.value = 1;
        _incrementDropdown.value = 1;
        Statement.Environment.Add("i", 20);

        _car.InPit.Returns(true);

        _testHelper.RunCoroutine(_forLoop.Run());

        Assert.AreEqual(0, Statement.Environment["i"]);
        yield return null;

        // For loop ended, should return "i" to original value
        Assert.AreEqual(20, Statement.Environment["i"]);
    }
}
