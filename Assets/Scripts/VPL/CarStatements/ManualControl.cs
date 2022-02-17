using System.Collections;

public class ManualControl : CarStatement
{
    // Gives control of the car to the user
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());
        Player.CurrentControl = ControlMode.Human;
    }
}
