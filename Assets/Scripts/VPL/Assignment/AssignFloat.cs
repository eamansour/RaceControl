using System.Collections;
public class AssignFloat : AssignStatement<float>
{
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());
    }
}
