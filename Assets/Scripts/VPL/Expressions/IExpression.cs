using System.Collections;
using TMPro;

public interface IExpression<T>
{
    void Construct(TMP_InputField leftOperandInput = null, TMP_InputField rightOperandInput = null, TMP_Dropdown dropdownInput = null);

    /// <summary>
    /// Evaluates the expression, returning the result of the evaluation.
    /// </summary>
    T EvaluateExpression();
}
