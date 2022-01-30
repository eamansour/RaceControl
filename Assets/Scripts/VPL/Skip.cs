using System.Collections;

public class Skip : Statement
{
    // Skip statements do nothing
    public override IEnumerator Run()
    {
        yield break;
    }
}
