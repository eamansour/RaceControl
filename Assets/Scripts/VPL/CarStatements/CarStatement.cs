using System.Collections;
using UnityEngine;
using TMPro;

public abstract class CarStatement : Statement
{
    protected static ICar PlayerCar;
    protected static IPlayerManager Player;

    protected TMP_Dropdown TimerDropdown { get; private set; }

    private static CameraFollow s_cameraFollow;

    public override IEnumerator Run()
    {
        if (Environment.ContainsKey("car") && Environment["car"] is IDataStructure<IPlayerManager>)
        {
            SetPlayer(Environment.Get<IDataStructure<IPlayerManager>>("car").GetContainedValue());
            yield return null;
        }
    }

    public void Construct(ICar playerCar, IPlayerManager player)
    {
        PlayerCar = playerCar;
        Player = player;
    }
    
    public void Construct(ICar playerCar, IPlayerManager player, TMP_Dropdown timerDropdown)
    {
        PlayerCar = playerCar;
        Player = player;
        TimerDropdown = timerDropdown;
    }

    protected virtual void Start()
    {
        if (Player == null || Player.Equals(null))
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayerManager>();
        }

        if (PlayerCar == null || PlayerCar.Equals(null))
        {
            PlayerCar = Player.PlayerCar;
        }
        
        if (s_cameraFollow == null || s_cameraFollow.Equals(null))
        {
            s_cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
        }
        
        TimerDropdown ??= GetComponentInChildren<TMP_Dropdown>();
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

    // Retrieves a selected dropdown option and converts it to a float
    protected float GetSelectedToFloat(TMP_Dropdown dropdown)
    {
        return float.TryParse(GetSelectedDropdownText(dropdown), out float value) ? value : 0f;
    }

    protected void SetPlayer(IPlayerManager newPlayer)
    {
        Player = newPlayer;
        PlayerCar = newPlayer.PlayerCar;
        s_cameraFollow.Target = newPlayer.AttachedGameObject;
    }
}
