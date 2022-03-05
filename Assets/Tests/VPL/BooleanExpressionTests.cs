using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TestTools;
using System.Collections;

[Category("VPLTests")]
[Category("ExpressionTests")]
public class BooleanExpressionTests
{
    private GameObject _testObject;
    private BooleanExpression _boolExpression;
    private TMP_InputField _leftOperand;
    private TMP_InputField _rightOperand;
    private TMP_Dropdown _operatorDropdown;
    private TMP_Dropdown _logicalDropdown;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();

        _boolExpression = _testObject.AddComponent<BooleanExpression>();
        
        _operatorDropdown = new GameObject().AddComponent<TMP_Dropdown>();
        _logicalDropdown = new GameObject().AddComponent<TMP_Dropdown>();

        _leftOperand = new GameObject().AddComponent<TMP_InputField>();
        _rightOperand = new GameObject().AddComponent<TMP_InputField>();

        BooleanExpression prefab =
            Resources.Load<BooleanExpression>("Prefabs/VPL/Expressions/BooleanExpression");

        _operatorDropdown.AddOptions(new List<string> { "==", "!=", "<", ">", "<=", ">=" });
        _logicalDropdown.AddOptions(new List<string> { "...", "and", "or" });

        _boolExpression.Construct(_leftOperand, _rightOperand, _operatorDropdown);
        _boolExpression.Construct(_logicalDropdown, prefab);
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

    [Test]
    public void BoolEquality_ShouldReturnOneIfEqualAndZeroIfNot(
        [Values("5", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        _operatorDropdown.value = 0;
        _logicalDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        bool result = _boolExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) == float.Parse(right), result);
    }

    [Test]
    public void BoolInequality_ShouldReturnOneIfNotEqualAndZeroIfEqual(
        [Values("5", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        _operatorDropdown.value = 1;
        _logicalDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        bool result = _boolExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) != float.Parse(right), result);
    }

    [Test]
    public void BoolLessThan_ShouldReturnOneIfLessThanRightAndZeroIfGreaterOrEqual(
        [Values("1", "3", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        _operatorDropdown.value = 2;
        _logicalDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        bool result = _boolExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) < float.Parse(right), result);
    }

    [Test]
    public void BoolGreaterThan_ShouldReturnOneIfGreaterThanRightAndZeroIfLessOrEqual(
        [Values("1", "-6", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        _operatorDropdown.value = 3;
        _logicalDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        bool result = _boolExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) > float.Parse(right), result);
    }

    [Test]
    public void BoolLessEqual_ShouldReturnOneIfLessOrEqualToRightAndZeroIfGreater(
        [Values("1", "-6", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        _operatorDropdown.value = 4;
        _logicalDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        bool result = _boolExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) <= float.Parse(right), result);
    }

    [Test]
    public void BoolGreaterEqual_ShouldReturnOneIfGreaterOrEqualToRightAndZeroIfLess(
        [Values("1", "-6", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        _operatorDropdown.value = 5;
        _logicalDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        bool result = _boolExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) >= float.Parse(right), result);
    }

    [Test]
    public void InvalidOperand_ShouldBeTreatedAsZero(
        [Values("2", "-5", "-20", "0")] string right
    )
    {
        _operatorDropdown.value = 0;
        _logicalDropdown.value = 0;
        _leftOperand.text = "BAD INPUT";
        _rightOperand.text = right;

        bool result = _boolExpression.EvaluateExpression();

        Assert.AreEqual(0f == float.Parse(right), result);
    }

    [Test]
    public void BothInvalidOperands_ShouldBeTreatedAsZeroComparison(
        [Values("BAD", "123BAD", "!£$%^&123")] string left,
        [Values("BAD", "xyzABC")] string right
    )
    {
        _operatorDropdown.value = 0;
        _logicalDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        bool result = _boolExpression.EvaluateExpression();

        Assert.AreEqual(true, result);
    }

    [UnityTest]
    public IEnumerator LogicalAnd_ShouldReturnTrueIfBothExpressionsAreTrue(
        [Values("5")] string left,
        [Values("5")] string right
    )
    {
        _operatorDropdown.value = 0;
        _logicalDropdown.value = 1;
        _leftOperand.text = left;
        _rightOperand.text = right;

        yield return new WaitForSeconds(0.1f);
        BooleanExpression newExpression = GameObject.Find("BooleanExpression(Clone)").GetComponent<BooleanExpression>();
        newExpression.Construct(_leftOperand, _rightOperand, _operatorDropdown);
        
        bool result = _boolExpression.EvaluateExpression();

        Assert.IsTrue(result);
    }
}
