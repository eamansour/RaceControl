using UnityEngine;
using TMPro;

public abstract class PlayerHudElement : MonoBehaviour
{
    protected static IPlayerManager Player { get; private set; }

    [SerializeField]
    protected TMP_Text HudText;

    // Updates the currently-tracked player to update all existing HUD elements
    public static void SetPlayer(IPlayerManager player)
    {
        Player = player;
    }

    private void Start()
    {
        if (Player == null || Player.Equals(null))
        {
            Player = GameManager.CurrentPlayer;
        }
    }
}
