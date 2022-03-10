using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
    private const string EngineSoundName = "Engine";

    private List<float> _gearPitch = new List<float>() { 10f, 15f, 18f, 20f, 25f, 28f };
    private List<int> _gearSpeed = new List<int>() { 5, 20, 35, 45, 55 };

    private Car _car;
    private AudioSource _source;

    private void Start()
    {
        _car = GetComponent<Car>();
        _source = SoundManager.GetSource(EngineSoundName);
    }

    private void OnDestroy()
    {
        if (_source)
        {
            SoundManager.StopSound(EngineSoundName);
        }
    }

    private void Update()
    {
        // In case of multiple players, only change the sound of the player being followed
        if (Time.timeScale == 0f || GameManager.CurrentPlayer.AttachedGameObject != _car.gameObject) return;

        if (!_source.isPlaying && Time.timeScale == 1f)
        {
            SoundManager.PlaySound(EngineSoundName);
        }

        float pitchFraction = _gearPitch[0];
        for (int i = 0; i < _gearSpeed.Count; i++)
        {
            // Simulate the sound of changing gears when above a speed threshold
            if (_car.GetSpeedInMPH() > _gearSpeed[i])
            {
                pitchFraction = _gearPitch[i + 1];
            }
        }

        _source.pitch = _car.GetSpeedInMPH() / pitchFraction;

        // Ensure there exists an idle engine sound
        if (_source.pitch == 0)
        {
            _source.pitch += 0.1f;
        }
    }
}
