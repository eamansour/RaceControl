using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarListStatement : CarStatement
{
    [SerializeField]
    private CarStatement _carStatement;

    public void Construct(CarStatement carStatement)
    {
        _carStatement = carStatement;
    }

    public override IEnumerator Run()
    {
        List<IPlayerManager> players = GameManager.Players;
        IPlayerManager player = players[0];

        string selected = GetSelectedToString(DropdownInput);

        if (Environment.ContainsKey(selected))
        {
            // Default to the first player if the index is out of bounds
            int index = (int)Environment[selected];
            if (index > 0 && index < players.Count)
            {
                player = players[index];
            }
            else
            {
                player = players[0];
            }
        }
        else
        {
            // Convert the selected index to an integer to retrieve the corresponding player
            int selectedIndex = (int)GetSelectedToFloat(DropdownInput);
            player = players[selectedIndex];
        }

        if (player != null)
        {
            // Focus on the indexed player
            SetPlayer(player);

            if (_carStatement)
            {
                _carStatement.Construct(player.PlayerCar, player, DropdownInput);
                yield return StartCoroutine(_carStatement.Run());
            }
        }
    }
}
