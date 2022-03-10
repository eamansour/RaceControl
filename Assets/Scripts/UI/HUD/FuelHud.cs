public class FuelHud : PlayerHudElement
{
    // Updates the player's fuel level HUD element
    private void FixedUpdate()
    {
        HudText.text = $"{Player.PlayerCar.GetFuelInt()}%";
    }
}
