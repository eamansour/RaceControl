using System.Collections;
using UnityEngine;

public class Wait : CarStatement
{
    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());

        float selectedTime = GetSelectedToFloat(DropdownInput);
        yield return new WaitForSeconds(selectedTime);
    }
}
