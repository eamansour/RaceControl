public class LapHud : PlayerHudElement
{
    // Updates the player's current lap HUD element
    private void FixedUpdate()
    {
        HudText.text = $"{Player.CurrentLap}";
    }
}
