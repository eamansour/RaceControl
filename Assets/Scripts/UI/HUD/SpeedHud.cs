public class SpeedHud : PlayerHudElement
{
    // Updates the player's speedometer HUD element
    private void FixedUpdate()
    {
        HudText.text = $"{Player.PlayerCar.GetSpeedInMPH()} MPH";
    }
}
