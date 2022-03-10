using System.Collections;
using TMPro;
using UnityEngine;

public class Wait : Statement
{
    [SerializeField]
    private TMP_Dropdown _timeDropdown;

    public void Construct(TMP_Dropdown timeDropdown)
    {
        _timeDropdown = timeDropdown;
    }

    // Wait statements pause program execution for a given period of time
    public override IEnumerator Run()
    {
        float selectedTime = GetSelectedToFloat(_timeDropdown);
        yield return new WaitForSeconds(selectedTime);
    }
}
