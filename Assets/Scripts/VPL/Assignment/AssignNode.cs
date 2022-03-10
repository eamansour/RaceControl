using System.Collections;
using UnityEngine;

public class AssignNode : AssignStatement<Node<IPlayerManager>>
{
    [SerializeField]
    private bool _updateNextNode = false;

    public void Construct(bool updateNextNode)
    {
        _updateNextNode = updateNextNode;
    }

    public override IEnumerator Run()
    {
        // If the assignment is of the form "node.next = ...", update the linked list
        string selected = GetSelectedToString(_variableDropdown);
        if (_updateNextNode)
        {
            if (Environment.ContainsKey(selected) && Environment[selected] is Node<IPlayerManager>)
            {
                Environment.Get<Node<IPlayerManager>>(selected).Next = _expression.EvaluateExpression();
                yield break;
            }
        }
        yield return StartCoroutine(base.Run());
    }
}
