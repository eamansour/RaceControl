using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[Category("VPLTests")]
[Category("ExpressionTests")]
public class BooleanExpressionTests
{
    private GameObject _testObject;
    private BooleanExpression _booleanExpression;
    private TMP_InputField _leftOperand;
    private TMP_InputField _rightOperand;
    private TMP_Dropdown _operatorDropdown;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();

        _booleanExpression = _testObject.AddComponent<BooleanExpression>();
        _operatorDropdown = _testObject.AddComponent<TMP_Dropdown>();

        _leftOperand = new GameObject().AddComponent<TMP_InputField>();
        _rightOperand = new GameObject().AddComponent<TMP_InputField>();

        List<string> options = new List<string> { "==", "!=", "<", ">", "<=", ">=" };
        _operatorDropdown.AddOptions(options);

        _booleanExpression.Construct(_leftOperand, _operatorDropdown, _rightOperand);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_testObject);
        Object.Destroy(_leftOperand.gameObject);
        Object.Destroy(_rightOperand.gameObject);
    }

    [Test]
    public void BoolEquality_ShouldReturnOneIfEqualAndZeroIfNot(
        [Values("5", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        // Arrange
        _operatorDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        // Act
        bool result = _booleanExpression.EvaluateExpression();

        // Assert
        Assert.AreEqual(float.Parse(left) == float.Parse(right), result);
    }

    [Test]
    public void BoolInequality_ShouldReturnOneIfNotEqualAndZeroIfEqual(
        [Values("5", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        // Arrange
        _operatorDropdown.value = 1;
        _leftOperand.text = left;
        _rightOperand.text = right;

        // Act
        bool result = _booleanExpression.EvaluateExpression();

        // Assert
        Assert.AreEqual(float.Parse(left) != float.Parse(right), result);
    }

    [Test]
    public void BoolLessThan_ShouldReturnOneIfLessThanRightAndZeroIfGreaterOrEqual(
        [Values("1", "3", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        // Arrange
        _operatorDropdown.value = 2;
        _leftOperand.text = left;
        _rightOperand.text = right;

        // Act
        bool result = _booleanExpression.EvaluateExpression();

        // Assert
        Assert.AreEqual(float.Parse(left) < float.Parse(right), result);
    }

    [Test]
    public void BoolGreaterThan_ShouldReturnOneIfGreaterThanRightAndZeroIfLessOrEqual(
        [Values("1", "-6", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        // Arrange
        _operatorDropdown.value = 3;
        _leftOperand.text = left;
        _rightOperand.text = right;

        // Act
        bool result = _booleanExpression.EvaluateExpression();

        // Assert
        Assert.AreEqual(float.Parse(left) > float.Parse(right), result);
    }

    [Test]
    public void BoolLessEqual_ShouldReturnOneIfLessOrEqualToRightAndZeroIfGreater(
        [Values("1", "-6", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        // Arrange
        _operatorDropdown.value = 4;
        _leftOperand.text = left;
        _rightOperand.text = right;

        // Act
        bool result = _booleanExpression.EvaluateExpression();

        // Assert
        Assert.AreEqual(float.Parse(left) <= float.Parse(right), result);
    }

    [Test]
    public void BoolGreaterEqual_ShouldReturnOneIfGreaterOrEqualToRightAndZeroIfLess(
        [Values("1", "-6", "30")] string left,
        [Values("5", "-5", "30")] string right
    )
    {
        // Arrange
        _operatorDropdown.value = 5;
        _leftOperand.text = left;
        _rightOperand.text = right;

        // Act
        bool result = _booleanExpression.EvaluateExpression();

        // Assert
        Assert.AreEqual(float.Parse(left) >= float.Parse(right), result);
    }

    [Test]
    public void InvalidOperand_ShouldBeTreatedAsZero(
        [Values("2", "-5", "-20", "0")] string right
    )
    {
        // Arrange
        _operatorDropdown.value = 0;
        _leftOperand.text = "BAD INPUT";
        _rightOperand.text = right;

        // Act
        bool result = _booleanExpression.EvaluateExpression();

        // Assert
        Assert.AreEqual(0f == float.Parse(right), result);
    }

    [Test]
    public void BothInvalidOperands_ShouldBeTreatedAsZeroComparison(
        [Values("BAD", "123BAD", "!£$%^&123")] string left,
        [Values("BAD", "xyzABC")] string right
    )
    {
        // Arrange
        _operatorDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        // Act
        bool result = _booleanExpression.EvaluateExpression();

        // Assert
        Assert.AreEqual(true, result);
    }
}
