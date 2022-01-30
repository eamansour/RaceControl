public class FloatLiteral : LiteralExpression<float>
{
    public override float EvaluateExpression()
    {
        float result = base.EvaluateExpression();
        if (result != default(float)) return result;
        
        string literal = GetSelectedDropdownText(DropdownInput);
        return float.TryParse(literal, out float value) ? value : 0f;
    }
}
