using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    private CameraController _cameraController;
    private IPlayerManager _player;

    [SerializeField]
    private Transform _checkpointParticles;

    [SerializeField]
    private bool _initialiseIndexText = false;

    private EventSystem _eventSystem;

    public void SetPlayer(IPlayerManager player)
    {
        _player = player;
        PlayerHudElement.SetPlayer(player);
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
        GameManager.OnPlayerUpdated += SetPlayer;
    }

    // Runs when the PlayerHud is destroyed, unsubscribe from any events
    private void OnDestroy()
    {
        GameManager.OnPlayerUpdated -= SetPlayer;
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
        UpdateCheckpointParticles();
    }

    /// <summary>
    /// Updates the position and rotation of the checkpoint particle system using the player's
    /// target checkpoint.
    /// </summary>
    private void UpdateCheckpointParticles()
    {
        Checkpoint targetCheckpoint = _player.TargetCheckpoint;
        _checkpointParticles.position = targetCheckpoint.GetPosition();
        _checkpointParticles.localEulerAngles = new Vector3(0, -targetCheckpoint.GetRotation().z, 0);
    }
}
