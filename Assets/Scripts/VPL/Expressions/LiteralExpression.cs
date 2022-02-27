public abstract class LiteralExpression<T> : Expression<T>
{
    /// <summary>
    /// Evaluates a literal by searching the environment for
    /// a selected option's corresponding value, or using the 
    /// type's default value otherwise.
    /// </summary>
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
