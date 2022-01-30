using System.Collections;
using TMPro;
using UnityEngine;

public class BooleanExpression : Expression<bool>
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

    // Evaluate a given boolean expression
    public override bool EvaluateExpression()
    {
        bool result = false;
        float left = GetOperandValue(_leftOperandInput);
        float right = GetOperandValue(_rightOperandInput);

        // Perform appropriate boolean comparison
        switch (GetSelectedDropdownText(DropdownInput))
        {
            case "==":
                result = left == right;
                break;
            case "!=":
                result = left != right;
                break;
            case "<":
                result = left < right;
                break;
            case "<=":
                result = left <= right;
                break;
            case ">":
                result = left > right;
                break;
            case ">=":
                result = left >= right;
                break;
            default:
                break;
        }

        return result;
    }
}
