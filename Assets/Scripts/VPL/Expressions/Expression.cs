using System.Collections;
using UnityEngine;
using TMPro;
using System;

public abstract class Expression<T> : Statement, IExpression<T>
{
    [field: SerializeField]
    protected TMP_InputField LeftOperandInput { get; private set; }

    [field: SerializeField]
    protected TMP_InputField RightOperandInput { get; private set; }

    [field: SerializeField]
    protected TMP_Dropdown DropdownInput { get; private set; }

    // Assigns a result of an expression to a given variable
    public override IEnumerator Run()
    {
        yield break;
    }

    // Evaluates an expression
    public abstract T EvaluateExpression();

    public void Construct(
        TMP_InputField leftOperandInput = null,
        TMP_InputField rightOperandInput = null,
        TMP_Dropdown dropdownInput = null
    )
    {
        LeftOperandInput = leftOperandInput;
        RightOperandInput = rightOperandInput;
        DropdownInput = dropdownInput;
    }

    // Retrieves an inputted operand from an inputfield and converts it to a float value
    protected virtual float GetOperandValue(TMP_InputField operandInput)
    {
        float result = 0;
        string operandText = operandInput.text;

        if (Environment.ContainsKey(operandText))
        {
            // Avoid casting stored player types
            if (Environment[operandText] is IDataStructure<IPlayerManager>) return 0f;

            result = Convert.ToSingle(Environment[operandText]);
        }
        else
        {
            result = float.TryParse(operandText, out float value) ? value : 0f;
        }
        return result;
    }
}
