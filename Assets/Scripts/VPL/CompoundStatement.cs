using System.Collections;

public abstract class CompoundStatement : Statement
{
    public abstract override IEnumerator Run();

    /// <summary>
    /// Runs a compound statement's contained block of statements.
    /// </summary>
    protected IEnumerator RunBlock()
    {
        // The first child of compound statements is a label, skip this
        for (int i = 1; i <= (transform.childCount - 1); i++)
        {
            while (Console.Paused)
            {
                yield return null;
            }

            Statement statement = transform.GetChild(i).GetComponent<Statement>();
            yield return StartCoroutine(statement.Run());
        }
    }
}
