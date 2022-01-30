using System.Collections;

public class SetUpLinkedList : Statement
{
    public override IEnumerator Run()
    {
        Node<IPlayerManager> players = new Node<IPlayerManager>(GameManager.Instance.Players);
        Environment.Add("car", players);
        yield break;
    }
}