using System.Collections;
using UnityEngine;

public class Wait : CarStatement
{
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());

        float selectedTime = GetSelectedToFloat(TimerDropdown);
        yield return new WaitForSeconds(selectedTime);
    }
}
