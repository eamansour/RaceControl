using UnityEngine;

public class LapObjective : Objective
{
    [field: SerializeField]
    protected int RequiredLapCounter { get; private set; } = 0;

    [SerializeField]
    private bool _mustWin = false;

    [SerializeField]
    private bool _mustChangePlayer = false;

    private IPlayerManager _initialPlayer;

    protected override void Start()
    {
        base.Start();

        if (_mustChangePlayer)
        {
            _initialPlayer = Player;
        }
    }

    public void Construct(int requiredLapCounter, bool mustWin, bool mustChangePlayer, IPlayerManager player)
    {
        RequiredLapCounter = requiredLapCounter;
        _mustWin = mustWin;
        _mustChangePlayer = mustChangePlayer;
        _initialPlayer = player;
    }

    public override void UpdateCompletion()
    {
        int currentLap = Player.CurrentLap;
        bool complete = (currentLap >= RequiredLapCounter);

        if (complete)
        {
            if ((_mustWin && Player.GetRacePosition() != 1) || 
                (_mustChangePlayer && Player.AttachedGameObject == _initialPlayer.AttachedGameObject))
            {
                Failed = true;
                return;
            }
        }
        Passed = complete;
    }
}
