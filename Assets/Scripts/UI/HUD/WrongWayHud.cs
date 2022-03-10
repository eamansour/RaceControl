public class WrongWayHud : PlayerHudElement
{
    // Controls the "Wrong Way!" HUD text to ensure the player is travelling in the correct direction
    private void FixedUpdate()
    {
        if (Player.LastCheckpoint != Player.RecentCheckpoint && Player.RecentCheckpoint.name != "PitEntry")
        {
            HudText.gameObject.SetActive(true);
        }
        else
        {
            HudText.gameObject.SetActive(false);
        }
    }
}
