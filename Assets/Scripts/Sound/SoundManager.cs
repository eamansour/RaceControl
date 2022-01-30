using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private Sound[] _sounds;

    private void Awake()
    {
        // Initialise and persist single instance (singleton) for use throughout the game
        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        // Create a hierarchy of GameObjects with audio sources attached
        for (int i = 0; i < _sounds.Length; i++)
        {
            GameObject go = new GameObject($"Sound{i}_{_sounds[i].Name}");
            go.transform.SetParent(transform);
            _sounds[i].SetSource(go.AddComponent<AudioSource>());
        }
    }

    // Start background music
    private void Start()
    {
        PlaySound("Music");
    }

    // Play a sound, given by its name
    public void PlaySound(string name)
    {
        for (int i = 0; i < _sounds.Length; i++)
        {
            if (_sounds[i].Name == name)
            {
                _sounds[i].Play();
                return;
            }
        }
    }

    // Stop a sound, given by its name
    public void StopSound(string name)
    {
        for (int i = 0; i < _sounds.Length; i++)
        {
            if (_sounds[i].Name == name)
            {
                _sounds[i].Stop();
                return;
            }
        }
    }

}