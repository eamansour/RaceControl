using System.Collections;

public class ElifCondition : Condition
{
    public override IEnumerator Run()
    {
        // Only evaluate the elif condition if the last conditional was false
        // and it was the previous statement
        if (!LastConditionResult && LastConditionIndex == transform.GetSiblingIndex() - 1)
        {
            yield return StartCoroutine(EvaluateCondition());
        }
    }
}
