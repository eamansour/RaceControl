using System.Collections;
using TMPro;

public abstract class CarStatement : Statement
{
    protected static ICar PlayerCar { get; private set; }
    protected static IPlayerManager Player { get; private set; }

    protected TMP_Dropdown DropdownInput { get; private set; }

    public override IEnumerator Run()
    {
        if (Environment.ContainsKey("car") && Environment["car"] is IDataStructure<IPlayerManager>)
        {
            IPlayerManager containedPlayer = Environment.Get<IDataStructure<IPlayerManager>>("car").GetContainedValue();
            if (containedPlayer != Player)
            {
                SetPlayer(Environment.Get<IDataStructure<IPlayerManager>>("car").GetContainedValue());
            }
            yield return null;
        }
    }

    public void Construct(ICar playerCar, IPlayerManager player, TMP_Dropdown dropdownInput = null)
    {
        PlayerCar = playerCar;
        Player = player;
        DropdownInput = dropdownInput;
    }

    protected virtual void Start()
    {
        if (Player == null || Player.Equals(null))
        {
            Player = GameManager.CurrentPlayer;
        }

        if (PlayerCar == null || PlayerCar.Equals(null))
        {
            PlayerCar = Player.PlayerCar;
        }

        DropdownInput ??= GetComponentInChildren<TMP_Dropdown>();
    }

    // Update player-related environment variables
    protected virtual void Update()
    {
        if (Environment.ContainsKey("fuel") && Environment.Get<float>("fuel") != PlayerCar.Fuel)
        {
            Environment["fuel"] = PlayerCar.Fuel;
        }

        if (Environment.ContainsKey("lap") && Environment.Get<int>("lap") != Player.CurrentLap)
        {
            Environment["lap"] = Player.CurrentLap;
        }
    }

    /// <summary>
    /// Updates the currently-controlled player in the level.
    /// </summary>
    protected void SetPlayer(IPlayerManager newPlayer)
    {
        Player = newPlayer;
        PlayerCar = newPlayer.PlayerCar;
        GameManager.SetPlayer(newPlayer);
    }
}
