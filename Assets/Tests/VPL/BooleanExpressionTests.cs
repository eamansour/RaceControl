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

    // Fields for logical operator tests
    private TMP_InputField _leftOperandNew;
    private TMP_InputField _rightOperandNew;
    private TMP_Dropdown _operatorDropdownNew;
    private TMP_Dropdown _logicalDropdownNew;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();

        GameObject expressionObject = new GameObject("TestExpression");
        expressionObject.AddComponent<Image>();
        _boolExpression = expressionObject.AddComponent<BooleanExpression>();
        _boolExpression.transform.SetParent(_testObject.transform);

        List<string> operatorList = new List<string> { "==", "!=", "<", ">", "<=", ">=" };
        List<string> logicalOperatorList = new List<string> { "...", "and", "or" };

        _operatorDropdown = CreateTestDropdown(operatorList);
        _logicalDropdown = CreateTestDropdown(logicalOperatorList);
        _operatorDropdownNew = CreateTestDropdown(operatorList);
        _logicalDropdownNew = CreateTestDropdown(logicalOperatorList);

        _leftOperand = CreateTestInputField();
        _rightOperand = CreateTestInputField();
        _leftOperandNew = CreateTestInputField();
        _rightOperandNew = CreateTestInputField();

        _boolExpression.Construct(_leftOperand, _rightOperand, _operatorDropdown);
        _boolExpression.Construct(_logicalDropdown, _boolExpression);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_testObject);
    }

    // Creates a new input field and sets its parent to the testObject
    private TMP_InputField CreateTestInputField()
    {
        TMP_InputField inputField = new GameObject().AddComponent<TMP_InputField>();
        inputField.transform.SetParent(_testObject.transform);
        return inputField;
    }

    // Creates a new dropdown with a given list of options and sets its parent to the testObject
    private TMP_Dropdown CreateTestDropdown(List<string> options)
    {
        TMP_Dropdown dropdown = new GameObject().AddComponent<TMP_Dropdown>();
        dropdown.transform.SetParent(_testObject.transform);
        dropdown.AddOptions(options);
        return dropdown;
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
    public IEnumerator LogicalAnd_ShouldReturnTrueIfBothExpressionsAreTrue()
    {
        _operatorDropdown.value = 0;
        _logicalDropdown.value = 1;
        _leftOperand.text = "5";
        _rightOperand.text = "5";

        _operatorDropdownNew.value = 0;
        _logicalDropdownNew.value = 0;
        _leftOperandNew.text = "-2.5";
        _rightOperandNew.text = "-2.5";

        yield return new WaitForSeconds(0.1f);

        BooleanExpression newExpression = GameObject.Find("TestExpression(Clone)").GetComponent<BooleanExpression>();
        newExpression.Construct(_leftOperandNew, _rightOperandNew, _operatorDropdownNew);
        newExpression.Construct(_logicalDropdownNew, _boolExpression);

        bool result = _boolExpression.EvaluateExpression();
        Assert.IsTrue(result);
    }

    [UnityTest]
    public IEnumerator LogicalAnd_ShouldReturnFalseIfAnExpressionIsFalse()
    {
        // Change logical operator to "and"
        _logicalDropdown.value = 1;

        _operatorDropdown.value = 0;
        _leftOperand.text = "5";
        _rightOperand.text = "5";

        _operatorDropdownNew.value = 0;
        _logicalDropdownNew.value = 0;
        _leftOperandNew.text = "1.5";
        _rightOperandNew.text = "2.5";

        yield return new WaitForSeconds(0.1f);

        BooleanExpression newExpression = GameObject.Find("TestExpression(Clone)").GetComponent<BooleanExpression>();
        newExpression.Construct(_leftOperandNew, _rightOperandNew, _operatorDropdownNew);
        newExpression.Construct(_logicalDropdownNew, _boolExpression);

        bool result = _boolExpression.EvaluateExpression();
        Assert.IsFalse(result);
    }

    [UnityTest]
    public IEnumerator LogicalOr_ShouldReturnTrueIfAtLeastOneExpressionIsTrue()
    {
        // Change logical operator to "or"
        _logicalDropdown.value = 2;

        _operatorDropdown.value = 0;
        _leftOperand.text = "5";
        _rightOperand.text = "5";

        _operatorDropdownNew.value = 0;
        _logicalDropdownNew.value = 0;
        _leftOperandNew.text = "0";
        _rightOperandNew.text = "2";

        yield return new WaitForSeconds(0.1f);

        BooleanExpression newExpression = GameObject.Find("TestExpression(Clone)").GetComponent<BooleanExpression>();
        newExpression.Construct(_leftOperandNew, _rightOperandNew, _operatorDropdownNew);
        newExpression.Construct(_logicalDropdownNew, _boolExpression);

        bool result = _boolExpression.EvaluateExpression();
        Assert.IsTrue(result);
    }

    [UnityTest]
    public IEnumerator LogicalOr_ShouldReturnFalseIfBothExpressionsAreFalse()
    {
        // Change logical operator to "or"
        _logicalDropdown.value = 2;

        _operatorDropdown.value = 0;
        _leftOperand.text = "1";
        _rightOperand.text = "5";

        _operatorDropdownNew.value = 0;
        _logicalDropdownNew.value = 0;
        _leftOperandNew.text = "0";
        _rightOperandNew.text = "2";

        yield return new WaitForSeconds(0.1f);

        BooleanExpression newExpression = GameObject.Find("TestExpression(Clone)").GetComponent<BooleanExpression>();
        newExpression.Construct(_leftOperandNew, _rightOperandNew, _operatorDropdownNew);
        newExpression.Construct(_logicalDropdownNew, _boolExpression);

        bool result = _boolExpression.EvaluateExpression();
        Assert.IsFalse(result);
    }

    [UnityTest]
    public IEnumerator NoLogicalOperator_ShouldDeleteNewExpression()
    {
        // Change logical operator to "and"
        _logicalDropdown.value = 1;

        _operatorDropdown.value = 0;
        _leftOperand.text = "1";
        _rightOperand.text = "5";

        _operatorDropdownNew.value = 0;
        _logicalDropdownNew.value = 0;
        _leftOperandNew.text = "0";
        _rightOperandNew.text = "2";

        yield return new WaitForSeconds(0.1f);

        BooleanExpression newExpression = GameObject.Find("TestExpression(Clone)").GetComponent<BooleanExpression>();
        Assert.IsNotNull(newExpression);

        // Change logical operator to none (default "..." option)
        _logicalDropdown.value = 0;

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(newExpression == null);
    }
}
