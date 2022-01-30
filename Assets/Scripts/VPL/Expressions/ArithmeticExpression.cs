using System.Collections;
using UnityEngine;
using TMPro;

public class ArithmeticExpression : Expression<float>
{
    [SerializeField]
    private TMP_InputField _leftOperandInput;

    [SerializeField]
    private TMP_InputField _rightOperandInput;

    public void Construct(TMP_InputField leftOperandInput, TMP_Dropdown operatorDropdown, TMP_InputField rightOperandInput)
    {
        base.Construct(operatorDropdown);
        _leftOperandInput = leftOperandInput;
        _rightOperandInput = rightOperandInput;
    }

    // Evaluate a given arithmetic expression
    public override float EvaluateExpression()
    {
        float result = 0;
        float left = GetOperandValue(_leftOperandInput);
        float right = GetOperandValue(_rightOperandInput);

        // Perform appropriate arithmetic operation
        switch (GetSelectedDropdownText(DropdownInput))
        {
            case "+":
                result = left + right;
                break;
            case "-":
                result = left - right;
                break;
            case "*":
                result = left * right;
                break;
            case "/":
                result = left / right;
                break;
            case "%":
                result = left % right;
                break;
            default:
                break;
        }
        return result;
    }
}
