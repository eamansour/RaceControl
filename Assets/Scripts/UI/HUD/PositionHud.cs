using System.Text;

public class PositionHud : PlayerHudElement
{
    // Updates the player's race position HUD element
    private void FixedUpdate()
    {
        HudText.text = $"{GetOrdinalPosition()}";
    }

    /// <summary>
    /// Calculates and returns the player's position in the race as an ordinal number string.
    /// </summary>
    private string GetOrdinalPosition()
    {
        StringBuilder sb = new StringBuilder();
        int position = Player.GetRacePosition();
        sb.Append(position);

        switch (position % 10)
        {
            case 1 when (position % 100) != 11:
                sb.Append("ST");
                break;
            case 2 when (position % 100) != 12:
                sb.Append("ND");
                break;
            case 3 when (position % 100) != 13:
                sb.Append("RD");
                break;
            default:
                sb.Append("TH");
                break;
        }
        return sb.ToString();
    }
}
