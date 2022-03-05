using UnityEngine;

public class BooleanExpressionWrapper : Expression<bool>
{
    [SerializeField]
    private BooleanExpression _boolExpression;

    // Evaluate the wrapped Boolean expression
    public override bool EvaluateExpression()
    {
        return _boolExpression.EvaluateExpression();
    }
}
