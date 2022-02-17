using System.Collections;
using UnityEngine;
using TMPro;

public class Turn : CarStatement
{
    [SerializeField]
    private TMP_Dropdown _directionDropdown;

    public override IEnumerator Run()
    {
        yield return StartCoroutine(base.Run());

        // Retrieve and translate selected direction to integer
        string selectedDirection = GetSelectedDropdownText(_directionDropdown);
        float direction = selectedDirection == "LEFT" ? -1f : 1f;

        // Retrieve selected dropdown option as a float
        float selectedTime = GetSelectedToFloat(DropdownInput);

        yield return StartCoroutine(PlayerCar.Turn(direction, selectedTime));
    }

    public void Construct(TMP_Dropdown directionDropdown)
    {
        _directionDropdown = directionDropdown;
    }
}
