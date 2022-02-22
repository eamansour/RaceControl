using UnityEngine;
using TMPro;

public class NodeLiteral : LiteralExpression<Node<IPlayerManager>>
{
    [SerializeField]
    private bool _getNextNode = false;

    public void Construct(TMP_Dropdown dropdown, bool getNextNode)
    {
        base.Construct(dropdownInput: dropdown);
        _getNextNode = getNextNode;
    }

    public override Node<IPlayerManager> EvaluateExpression()
    {
        // If the literal is of the form "node.next", return the next node in the linked list
        string literal = GetSelectedDropdownText(DropdownInput);
        if (_getNextNode)
        {
            if (Environment.ContainsKey(literal) && Environment[literal] is Node<IPlayerManager>)
            {
                return Environment.Get<Node<IPlayerManager>>(literal).Next;
            }
        }
        return base.EvaluateExpression();
    }
}