using System.Collections;

public class Retire : CarStatement
{
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());

        Player.IsRetiring = true;
        Player.CurrentControl = ControlMode.AI;

        // Prevent early termination of the retire statement
        while (!PlayerCar.InPit)
        {
            yield return null;
        }

        Player.RetirePlayer();
    }
}
