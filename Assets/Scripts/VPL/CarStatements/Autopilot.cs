using System.Collections;

public class Autopilot : CarStatement
{
    // Hands control of the car to the car's AI
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());
        Player.CurrentControl = PlayerManager.ControlMethod.AI;
    }
}
