public abstract class LiteralExpression<T> : Expression<T>
{

    // Evaluates a generic literal expression by searching the environment for
    // a selected option's corresponding value, defaulting otherwise
    public override T EvaluateExpression()
    {
        string literal = GetSelectedDropdownText(DropdownInput);
        if (Environment.ContainsKey(literal) && Environment[literal] is T)
        {
            return Environment.Get<T>(literal);
        }
        return default(T);
    }
}
