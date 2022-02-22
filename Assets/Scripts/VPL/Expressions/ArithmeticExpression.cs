public class ArithmeticExpression : Expression<float>
{
    // Evaluate a given arithmetic expression
    public override float EvaluateExpression()
    {
        float result = 0;
        float left = GetOperandValue(LeftOperandInput);
        float right = GetOperandValue(RightOperandInput);

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
