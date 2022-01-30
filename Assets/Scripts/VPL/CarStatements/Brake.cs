using System.Collections;

public class Brake : CarStatement
{
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());

        float selectedTime = GetSelectedToFloat(TimerDropdown);
        yield return StartCoroutine(PlayerCar.Brake(selectedTime));
    }
}
