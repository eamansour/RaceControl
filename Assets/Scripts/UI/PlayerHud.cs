using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    private CameraController _cameraController;
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
    private TMP_Text _wrongWayText;

    [SerializeField]
    private Transform _checkpointParticles;

    [SerializeField]
    private bool _initialiseIndexText = false;

    private EventSystem _eventSystem;

    public void Construct(IPlayerManager player)
    {
        _player = player;
    }

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
        _player = GameManager.CurrentPlayer;

        _eventSystem = EventSystem.current;
        
        if (_initialiseIndexText)
        {
            List<IPlayerManager> players = GameManager.Players;
            for (int i = 0; i < players.Count; i++)
            {
                GameObject player = players[i].AttachedGameObject;
                player.GetComponentInChildren<TMP_Text>().text = $"{i}";
            }
        }
        
        // Subscribe to track any player updates
        GameManager.OnPlayerUpdated += Construct;
    }

    // Runs when the PlayerHud is destroyed, unsubscribe from any events
    private void OnDestroy()
    {
        GameManager.OnPlayerUpdated -= Construct;
    }

    // Runs on every frame, disables camera control when typing
    private void Update()
    {
        if (!GameManager.LevelStarted)
        {
            _cameraController.enabled = true;
            GameObject selected = _eventSystem.currentSelectedGameObject;
            if (!selected) return;

            TMP_InputField inputField = selected.GetComponent<TMP_InputField>();
            if (!inputField) return;

            if (inputField.isFocused)
            {
                _cameraController.enabled = false;
            }
        }
    }

    // Update the player's HUD on every physics system update
    private void FixedUpdate()
    {
        // Optional UI text components
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

        // Ensure the player is travelling in the correct direction
        if (_player.LastCheckpoint != _player.RecentCheckpoint && _player.RecentCheckpoint.name != "PitEntry")
        {
            _wrongWayText.gameObject.SetActive(true);
        }
        else
        {
            _wrongWayText.gameObject.SetActive(false);
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
