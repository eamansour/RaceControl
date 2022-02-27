using System.Collections;
using UnityEngine;

public class Wait : CarStatement
{
    public override IEnumerator Run()
    {
        float selectedTime = GetSelectedToFloat(DropdownInput);
        yield return new WaitForSeconds(selectedTime);
    }
}
