using System.Collections;

public class ManualControl : CarStatement
{
    // Hands control of the car to the user
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());
        Player.CurrentControl = PlayerManager.ControlMethod.Human;
    }
}
