using UnityEngine;
using TMPro;

public abstract class Objective : MonoBehaviour
{
    public bool Passed { get; set; } = false;
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
    }

    private void Update()
    {
        // The player in camera focus is the main player, so objectives should refer to it
        if (!(Player as Object) || GameManager.CurrentPlayer.AttachedGameObject != Player.AttachedGameObject)
        {
            Player = GameManager.CurrentPlayer;
        }
    }

    public abstract void UpdateCompletion();

    public void UpdateUI(bool complete)
    {
        ObjectiveText.color = complete ? Color.green : Color.red;
    }

    public void Reset()
    {
        ObjectiveText.color = Color.white;
        Passed = false;
        Failed = false;
    }
}
