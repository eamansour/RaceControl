using System;
using System.Collections;
using UnityEngine;

public class WhileLoop : CompoundStatement
{
    private IExpression<bool> _expression;

    public void Construct(IExpression<bool> expression)
    {
        _expression = expression;
    }

    private void Start()
    {
        if (_expression == null || _expression.Equals(null))
        {
            _expression = GetComponentInChildren<IExpression<bool>>();
        }
    }

    public override IEnumerator Run()
    {
        bool expressionResult = _expression.EvaluateExpression();

        // Run the while loop as long as the expression evaluates to true
        while (expressionResult)
        {
            yield return StartCoroutine(RunBlock());
            expressionResult = _expression.EvaluateExpression();
        }
    }
}
