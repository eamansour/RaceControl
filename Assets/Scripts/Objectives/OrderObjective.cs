using System.Collections.Generic;
using UnityEngine;

public class OrderObjective : LapObjective
{
    [SerializeField]
    private List<int> _requiredIndexOrder = new List<int>();
    private List<IPlayerManager> _players = new List<IPlayerManager>();

    public void Construct(List<int> requiredIndexOrder, List<IPlayerManager> players)
    {
        _requiredIndexOrder = requiredIndexOrder;
        _players = players;
    }

    protected override void Start()
    {
        base.Start();
        _players = GameManager.Instance.Players;
    }

    // The objective is complete when all players have completed the required number of laps
    // in a required order, given by their player indexes
    public override bool IsComplete()
    {
        // If an earlier car has completed fewer laps than a later car, fail the level
        for (int i = 1; i < _requiredIndexOrder.Count; i++)
        {
            int prevIndex = _requiredIndexOrder[i - 1];
            int currentIndex = _requiredIndexOrder[i];

            if (_players[prevIndex].CurrentLap < _players[currentIndex].CurrentLap)
            {
                Failed = true;
                return false;
            }
        }

        // Remove the first player index once the player has met the required number of laps before other players
        bool removed = false;
        do
        {
            if (_requiredIndexOrder.Count > 0 && _players[_requiredIndexOrder[0]].CurrentLap >= RequiredLapCounter)
            {
                _requiredIndexOrder.RemoveAt(0);
                removed = true;
            }
            else
            {
                removed = false;
            }

        } while (removed);

        return (_requiredIndexOrder.Count == 0);
    }
}
