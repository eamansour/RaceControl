using System.Collections;

public class LinkedListDefinition : Statement
{
    public override IEnumerator Run()
    {
        Node<IPlayerManager> players = new Node<IPlayerManager>(GameManager.Players);
        Environment.Add("car", players);
        yield break;
    }
}
