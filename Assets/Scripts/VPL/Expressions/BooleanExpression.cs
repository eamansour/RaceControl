using TMPro;
using UnityEngine;

public class BooleanExpression : Expression<bool>
{
    [SerializeField]
    private TMP_Dropdown _logicalDropdown;

    private BooleanExpression _spawnedExpression;

    public void Construct(TMP_Dropdown logicalDropdown)
    {
        _logicalDropdown = logicalDropdown;
    }

    private void Start()
    {
        _logicalDropdown.onValueChanged.AddListener(OnValueChanged);
    }

    // Evaluate a given Boolean expression
    public override bool EvaluateExpression()
    {
        bool result = false;
        float left = GetOperandValue(LeftOperandInput);
        float right = GetOperandValue(RightOperandInput);

        // Perform appropriate Boolean comparison
        switch (GetSelectedToString(DropdownInput))
        {
            case "==":
                result = left == right;
                break;
            case "!=":
                result = left != right;
                break;
            case "<":
                result = left < right;
                break;
            case "<=":
                result = left <= right;
                break;
            case ">":
                result = left > right;
                break;
            case ">=":
                result = left >= right;
                break;
            default:
                break;
        }

        // Evaluation with a logical operator (i.e. and, or)
        if (_spawnedExpression)
        {
            switch (GetSelectedToString(_logicalDropdown))
            {
                case "and":
                    result = result && _spawnedExpression.EvaluateExpression();
                    break;
                case "or":
                    result = result || _spawnedExpression.EvaluateExpression();
                    break;
                default:
                    break;
            }
        }
        return result;
    }

    /// <summary>
    /// Event method to handle logical operator dropdown value changes.
    /// </summary>
    public void OnValueChanged(int option)
    {
        // Value changed to no logical operator
        if (option == 0)
        {
            DeleteSpawnedExpressions();
            return;
        }
        
        // Value changed to a logical operator
        if (!_spawnedExpression)
        {
            SpawnExpression();
            Droppable.UpdateLayout();
        }
    }

    /// <summary>
    /// Deletes all sibling expressions that follow this expression in the object hierarchy.
    /// </summary>
    private void DeleteSpawnedExpressions()
    {
        Transform parent = transform.parent;
        for (int i = transform.GetSiblingIndex() + 1; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
        _spawnedExpression = null;
    }

    /// <summary>
    /// Spawns a new Boolean expression.
    /// </summary>
    private void SpawnExpression()
    {
        _logicalDropdown.enabled = false;
        string leftOperand = LeftOperandInput.text;
        string rightOperand = RightOperandInput.text;

        LeftOperandInput.text = "";
        RightOperandInput.text = "";

        GameObject newExpression = GameObject.Instantiate(gameObject);
        newExpression.transform.SetParent(transform.parent, false);

        _logicalDropdown.enabled = true;
        LeftOperandInput.text = leftOperand;
        RightOperandInput.text = rightOperand;

        foreach (TMP_Dropdown dropdown in newExpression.GetComponentsInChildren<TMP_Dropdown>())
        {
            dropdown.value = 0;
            dropdown.enabled = true;
        }
        _spawnedExpression = newExpression.GetComponent<BooleanExpression>();
    }
}
