using System.Collections;

public class Brake : CarStatement
{
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());

        float selectedTime = GetSelectedToFloat(DropdownInput);
        yield return StartCoroutine(PlayerCar.Brake(selectedTime));
    }
}
