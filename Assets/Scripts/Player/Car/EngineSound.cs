using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
    private const string EngineSoundName = "Engine";

    [SerializeField]
    private List<float> _gearPitch = new List<float>() { 10f, 15f, 18f, 20f, 25f, 28f };
    
    [SerializeField]
    private List<int> _gearSpeed = new List<int>() { 5, 20, 35, 45, 55 };

    private Car _car;
    private AudioSource _source;
    private CameraFollow _cameraFollow;

    private void Start()
    {
        _car = GetComponent<Car>();
        _source = SoundManager.GetSource(EngineSoundName);
        SoundManager.PlaySound(EngineSoundName);

        _cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
    }

    private void Update()
    {
        if (_cameraFollow.Target != _car.gameObject) return;

        float pitchFraction = _gearPitch[0];
        for (int i = 1; i < _gearSpeed.Count; i++)
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
