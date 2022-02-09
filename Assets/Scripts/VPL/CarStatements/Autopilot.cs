using System.Collections;

public class Autopilot : CarStatement
{
    // Gives control of the car to the car's AI
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());
        Player.CurrentControl = PlayerManager.ControlMethod.AI;
    }
}
