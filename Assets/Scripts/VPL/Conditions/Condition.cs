using System.Collections;

public abstract class Condition : CompoundStatement
{
    protected static bool LastConditionResult { get; private set; } = true;
    protected static int LastConditionIndex { get; private set; } = -1;

    private IExpression<bool> _expression;

    public void Construct(IExpression<bool> expression, bool lastConditionResult, int lastConditionIndex)
    {
        _expression = expression;
        LastConditionResult = lastConditionResult;
        LastConditionIndex = lastConditionIndex;
    }

    private void Start()
    {
        _expression = GetComponentInChildren<IExpression<bool>>();
    }

    public abstract override IEnumerator Run();

    /// <summary>
    /// Evaluates a conditional statement block.
    /// </summary>
    protected IEnumerator EvaluateCondition()
    {
        bool expressionResult = _expression.EvaluateExpression();

        // Only run the contained statements if the condition evaluates to true
        if (expressionResult)
        {
            yield return StartCoroutine(RunBlock());
            LastConditionResult = true;
        }
        else
        {
            LastConditionResult = false;
        }
        LastConditionIndex = transform.GetSiblingIndex();
    }
}
