using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    private CameraFollow _mainCamera;
    private IPlayerManager _player;

    [SerializeField]
    private TMP_Text _mphText;

    [SerializeField]
    private TMP_Text _fuelText;

    [SerializeField]
    private TMP_Text _positionText;

    [SerializeField]
    private TMP_Text _lapText;

    [SerializeField]
    private Transform _checkpointParticles;

    [SerializeField]
    private bool _initialiseIndexText = false;

    public void Construct(IPlayerManager player)
    {
        _player = player;
    }

    private void Start()
    {
        _mainCamera = GameObject.FindObjectOfType<CameraFollow>();
        _player = _mainCamera.Target.GetComponent<IPlayerManager>();
        
        if (_initialiseIndexText)
        {
            List<IPlayerManager> players = GameManager.Instance.Players;
            for (int i = 0; i < players.Count; i++)
            {
                GameObject player = players[i].AttachedGameObject;
                player.GetComponentInChildren<TMP_Text>().text = $"{i}";
            }
        }
    }

    // Update the player's HUD on every physics system update
    private void FixedUpdate()
    {
        if (!(_player as Object) || _mainCamera.Target != _player.AttachedGameObject)
        {
            _player = _mainCamera.Target.GetComponent<IPlayerManager>();
        }

        if (_mphText)
        {
            _mphText.text = $"{_player.PlayerCar.GetSpeedInMPH()} MPH";
        }

        if (_fuelText)
        {
            _fuelText.text = $"{_player.PlayerCar.GetFuelInt()}%";
        }

        if (_positionText)
        {
            _positionText.text = $"{GetOrdinalPosition()}";
        }

        if (_lapText)
        {
            _lapText.text = $"{_player.CurrentLap}";
        }

        // Update the checkpoint particles' position/rotation as the player moves
        Checkpoint targetCheckpoint = _player.TargetCheckpoint;
        _checkpointParticles.position = targetCheckpoint.GetPosition();
        _checkpointParticles.localEulerAngles = new Vector3(0, -targetCheckpoint.GetRotation().z, 0);
    }

    // Returns the player's position in the race as an ordinal number string
    private string GetOrdinalPosition()
    {
        StringBuilder sb = new StringBuilder();
        int position = _player.GetRacePosition();
        sb.Append(position);

        switch (position % 10)
        {
            case 1 when (position % 100) != 11:
                sb.Append("ST");
                break;
            case 2 when (position % 100) != 12:
                sb.Append("ND");
                break;
            case 3 when (position % 100) != 13:
                sb.Append("RD");
                break;
            default:
                sb.Append("TH");
                break;
        }
        return sb.ToString();
    }
}
