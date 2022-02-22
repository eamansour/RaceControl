public class BooleanExpression : Expression<bool>
{
    // Evaluate a given boolean expression
    public override bool EvaluateExpression()
    {
        bool result = false;
        float left = GetOperandValue(LeftOperandInput);
        float right = GetOperandValue(RightOperandInput);

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
