using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Category("VPLTests")]
[Category("ExpressionTests")]
public class ArithmeticExpressionTests
{
    private GameObject _testObject;
    private ArithmeticExpression _arithmeticExpression;
    private TMP_InputField _leftOperand;
    private TMP_InputField _rightOperand;
    private TMP_Dropdown _operatorDropdown;

    [SetUp]
    public void SetUp()
    {
        _testObject = new GameObject();
        _testObject.AddComponent<Image>();

        _arithmeticExpression = _testObject.AddComponent<ArithmeticExpression>();
        _operatorDropdown = _testObject.AddComponent<TMP_Dropdown>();

        _leftOperand = new GameObject().AddComponent<TMP_InputField>();
        _rightOperand = new GameObject().AddComponent<TMP_InputField>();

        List<string> options = new List<string> { "+", "-", "*", "/", "%" };
        _operatorDropdown.AddOptions(options);

        _arithmeticExpression.Construct(_leftOperand, _rightOperand, _operatorDropdown);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_testObject);
        Object.Destroy(_leftOperand.gameObject);
        Object.Destroy(_rightOperand.gameObject);
    }

    [Test, Combinatorial]
    public void ArithmeticAddition_ShouldAddOperands(
        [Values("5", "30", "-10")] string left,
        [Values("2", "-5", "-20")] string right
    )
    {
        _operatorDropdown.value = 0;
        _leftOperand.text = left;
        _rightOperand.text = right;

        float result = _arithmeticExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) + float.Parse(right), result);
    }

    [Test, Combinatorial]
    public void ArithmeticSubtraction_ShouldSubtractOperands(
        [Values("5", "30", "-10")] string left,
        [Values("2", "-5", "-20")] string right
    )
    {
        _operatorDropdown.value = 1;
        _leftOperand.text = left;
        _rightOperand.text = right;

        float result = _arithmeticExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) - float.Parse(right), result);
    }

    [Test, Combinatorial]
    public void ArithmeticMultiplication_ShouldMultiplyOperands(
        [Values("5", "30", "-10")] string left,
        [Values("2", "-5", "-20")] string right
    )
    {
        _operatorDropdown.value = 2;
        _leftOperand.text = left;
        _rightOperand.text = right;

        float result = _arithmeticExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) * float.Parse(right), result);
    }

    [Test, Combinatorial]
    public void ArithmeticDivision_ShouldDivideOperands(
        [Values("5", "30", "-10")] string left,
        [Values("2", "-5", "-20")] string right
    )
    {
        _operatorDropdown.value = 3;
        _leftOperand.text = left;
        _rightOperand.text = right;

        float result = _arithmeticExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) / float.Parse(right), result);
    }

    [Test, Combinatorial]
    public void ArithmeticModulus_ShouldReturnDivisionRemainderOfOperands(
        [Values("5", "30", "-10")] string left,
        [Values("2", "-5", "-20")] string right
    )
    {
        _operatorDropdown.value = 4;
        _leftOperand.text = left;
        _rightOperand.text = right;

        float result = _arithmeticExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(left) % float.Parse(right), result);
    }

    [Test]
    public void InvalidOperand_ShouldIgnoreOperandValue(
        [Values("2", "-5", "-20")] string right
    )
    {
        _operatorDropdown.value = 0;
        _leftOperand.text = "BAD INPUT";
        _rightOperand.text = right;

        float result = _arithmeticExpression.EvaluateExpression();

        Assert.AreEqual(float.Parse(right), result);
    }

    [Test]
    public void BothInvalidOperands_ShouldReturnZero()
    {
        _operatorDropdown.value = 2;
        _leftOperand.text = "BAD INPUT";
        _rightOperand.text = "MORE BAD INPUT";

        float result = _arithmeticExpression.EvaluateExpression();

        Assert.AreEqual(0f, result);
    }
}
