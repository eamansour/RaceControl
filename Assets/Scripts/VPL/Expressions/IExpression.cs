using System.Collections;
using TMPro;

public interface IExpression<T>
{
    void Construct(TMP_Dropdown operatorDropdown);
    T EvaluateExpression();
    IEnumerator Run();
}
