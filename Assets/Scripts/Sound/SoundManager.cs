using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    private static SoundManager s_instance;

    private static List<Sound> s_sounds = new List<Sound>();

    [SerializeField]
    private List<Sound> _sounds = new List<Sound>();

    private void Awake()
    {
        // Persist a single instance (singleton) throughout the game
        if (s_instance != null)
        {
            if (s_instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            s_instance = this;
            DontDestroyOnLoad(this);
        }

        // Create a hierarchy of GameObjects with audio sources attached
        s_sounds = _sounds;
        for (int i = 0; i < s_sounds.Count; i++)
        {
            GameObject go = new GameObject($"Sound{i}_{s_sounds[i].Name}");
            go.transform.SetParent(transform);
            s_sounds[i].Source = go.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Starts the game's background music.
    /// </summary>
    private void Start()
    {
        PlaySound("Music");
    }

    /// <summary>
    /// Plays a sound, given by its name.
    /// </summary>
    public static void PlaySound(string name)
    {
        for (int i = 0; i < s_sounds.Count; i++)
        {
            if (s_sounds[i].Name == name)
            {
                s_sounds[i].Play();
                return;
            }
        }
    }

    /// <summary>
    /// Stop a sound, given by its name.
    /// </summary>
    public static void StopSound(string name)
    {
        for (int i = 0; i < s_sounds.Count; i++)
        {
            if (s_sounds[i].Name == name)
            {
                s_sounds[i].Stop();
                return;
            }
        }
    }

    /// <summary>
    /// Gets a given sound's audio source if the sound exists, null otherwise.
    /// </summary>
    public static AudioSource GetSource(string name)
    {
        for (int i = 0; i < s_sounds.Count; i++)
        {
            if (s_sounds[i].Name == name)
            {
                return s_sounds[i].Source;
            }
        }
        return null;
    }
}