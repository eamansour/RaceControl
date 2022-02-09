using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager s_instance;

    private static Sound[] s_sounds;

    [SerializeField]
    private Sound[] _sounds;

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
        for (int i = 0; i < s_sounds.Length; i++)
        {
            GameObject go = new GameObject($"Sound{i}_{s_sounds[i].Name}");
            go.transform.SetParent(transform);
            s_sounds[i].SetSource(go.AddComponent<AudioSource>());
        }
    }

    // Start background music
    private void Start()
    {
        PlaySound("Music");
    }

    // Play a sound, given by its name
    public static void PlaySound(string name)
    {
        for (int i = 0; i < s_sounds.Length; i++)
        {
            if (s_sounds[i].Name == name)
            {
                s_sounds[i].Play();
                return;
            }
        }
    }

    // Stop a sound, given by its name
    public static void StopSound(string name)
    {
        for (int i = 0; i < s_sounds.Length; i++)
        {
            if (s_sounds[i].Name == name)
            {
                s_sounds[i].Stop();
                return;
            }
        }
    }
}