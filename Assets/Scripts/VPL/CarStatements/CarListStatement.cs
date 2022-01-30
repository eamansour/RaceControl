using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarListStatement : CarStatement
{
    [SerializeField]
    private TMP_Dropdown _indexDropdown;

    [SerializeField]
    private CarStatement _carStatement;

    public override IEnumerator Run()
    {
        List<IPlayerManager> players = GameManager.Instance.Players;
        IPlayerManager player = players[0];

        string selected = GetSelectedDropdownText(_indexDropdown);

        if (selected == "i")
        {
            // Using pre-defined index (i)
            if (Environment.ContainsKey("i"))
            {
                // Default to the first player if the index is out of bounds
                int index = (int)Environment["i"];
                if (index > 0 && index < players.Count)
                {
                    player = players[index];
                }
                else
                {
                    player = players[0];
                }
            }
        }
        else
        {
            // Convert the selected index to an integer to retrieve the corresponding player
            int selectedIndex = (int)GetSelectedToFloat(_indexDropdown);
            player = players[selectedIndex];
        }

        if (player != null)
        {
            // Focus on the indexed player
            SetPlayer(player);

            if (_carStatement)
            {
                _carStatement.Construct(player.PlayerCar, player, TimerDropdown);
                yield return StartCoroutine(_carStatement.Run());                
            }
        }
    }
}
