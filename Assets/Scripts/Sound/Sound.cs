using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [field: SerializeField]
    public string Name { get; private set; }

    public AudioSource Source
    {
        get => _source;
        set
        {
            _source = value;
            _source.outputAudioMixerGroup = _mixerGroup;
            _source.clip = _clip;
            _source.loop = _loop;            
        }
    }
    
    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private AudioMixerGroup _mixerGroup;

    [SerializeField]
    private bool _loop = false;

    private AudioSource _source;

    // Play the current sound from its audio source
    public void Play()
    {
        Source.Play();
    }

    // Stops the current sound
    public void Stop()
    {
        Source.Stop();
    }
}
