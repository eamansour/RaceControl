using System.Collections;
using UnityEngine;

public class AssignNode : AssignStatement<Node<IPlayerManager>>
{
    [SerializeField]
    private bool _updateNextNode = false;

    public override IEnumerator Run()
    {
        // If the assignment is of the form "node.next = ...", update the linked list
        string selected = GetSelectedDropdownText(_variableDropdown);
        if (_updateNextNode)
        {
            if (Environment.ContainsKey(selected) && Environment[selected] is Node<IPlayerManager>)
            {
                Environment.Get<Node<IPlayerManager>>(selected).Next = _expression.EvaluateExpression();
                Debug.Log($"{selected}.next = {Environment.Get<Node<IPlayerManager>>(selected).Next}");
                yield break;
            }
        }
        yield return StartCoroutine(base.Run());
        Debug.Log($"Key: {selected}, Value: {Environment.Get<Node<IPlayerManager>>(selected)}");
    }   
}
