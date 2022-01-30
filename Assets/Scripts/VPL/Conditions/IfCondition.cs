using System.Collections;

public class IfCondition : Condition
{
    public override IEnumerator Run()
    {
        // Evaluate the if condition
        yield return StartCoroutine(EvaluateCondition());
    }
}
