public class FloatLiteral : LiteralExpression<float>
{
    public override float EvaluateExpression()
    {
        float result = base.EvaluateExpression();
        if (result != default(float)) return result;

        return GetOperandValue(LeftOperandInput);
    }
}
