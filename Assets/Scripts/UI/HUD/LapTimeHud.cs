using UnityEngine;
using TMPro;
using System;

public class LapTimeHud : PlayerHudElement
{
    [SerializeField]
    private TMP_Text _bestLapTimeText;

    // Updates the player's current and best lap time HUD elements
    private void FixedUpdate()
    {
        TimeSpan currentLapTime = TimeSpan.FromSeconds(Player.CurrentLapTime);

        HudText.text = $"Time: {currentLapTime.ToString("mm':'ss':'fff")}";

        if (Player.BestLapTime != Mathf.Infinity)
        {
            TimeSpan bestLapTime = TimeSpan.FromSeconds(Player.BestLapTime);
            _bestLapTimeText.text = $"Best: {bestLapTime.ToString("mm':'ss':'fff")}";
        }
        else if (_bestLapTimeText.text != "")
        {
            _bestLapTimeText.text = "";
        }
    }
}
