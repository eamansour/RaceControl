using UnityEngine;
using TMPro;

public abstract class Objective : MonoBehaviour, IObjective
{
    public bool Passed { get; protected set; } = false;
    public bool Failed { get; protected set; } = false;

    [field: SerializeField]
    protected TMP_Text ObjectiveText { get; private set; }

    protected IPlayerManager Player { get; private set; }

    public void Construct(IPlayerManager player)
    {
        Player = player;
    }

    protected virtual void Start()
    {
        if (Player == null || Player.Equals(null))
        {
            Player = GameManager.CurrentPlayer;
        }

        // Subscribe to track any player updates
        GameManager.OnPlayerUpdated += Construct;
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerUpdated -= Construct;
    }

    /// <inheritdoc />
    public abstract void UpdateCompletion();

    /// <inheritdoc />
    public void UpdateUI(bool complete)
    {
        ObjectiveText.color = complete ? Color.green : Color.red;
    }

    /// <inheritdoc />
    public void Reset()
    {
        ObjectiveText.color = Color.white;
        Passed = false;
        Failed = false;
    }
}
