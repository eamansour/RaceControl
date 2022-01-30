using System.Collections;

public class ElseCondition : Condition
{
    public override IEnumerator Run()
    {
        // Only run the else block if the last conditional was false
        // and it was the previous statement
        if (!LastConditionResult && LastConditionIndex == transform.GetSiblingIndex() - 1)
        {
            yield return StartCoroutine(RunBlock());
        }
    }
}
