using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [field: SerializeField]
    public string Name { get; private set; }
    
    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    private AudioMixerGroup _mixerGroup;

    [SerializeField]
    private bool _loop = false;

    private AudioSource _source;

    // Set an audio source
    public void SetSource(AudioSource audioSource)
    {
        _source = audioSource;
        _source.outputAudioMixerGroup = _mixerGroup;
        _source.clip = _clip;
        _source.loop = _loop;
    }

    // Play the current sound from its audio source
    public void Play()
    {
        _source.Play();
    }

    // Stops the current sound
    public void Stop()
    {
        _source.Stop();
    }
}
