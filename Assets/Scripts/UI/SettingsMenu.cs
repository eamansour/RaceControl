using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System;

public class SettingsMenu : MonoBehaviour
{
    private static string s_volumePrefKey = "volume";
    private static string s_resolutionPrefKey = "resolution";
    private static string s_fullscreenPrefKey = "fullscreen";

    [SerializeField]
    private AudioMixer _mixer;

    [SerializeField]
    private TMP_Dropdown _resolutionDropdown;

    [SerializeField]
    private Toggle _fullscreenToggle;

    [SerializeField]
    private Slider _soundSlider;

    private Resolution[] _resolutions;

    // Constructor-like method to initialise serialized fields
    public void Construct(AudioMixer mixer, TMP_Dropdown resolutionDropdown, Toggle fullscreenToggle, Slider soundSlider)
    {
        _mixer = mixer;
        _resolutionDropdown = resolutionDropdown;
        _fullscreenToggle = fullscreenToggle;
        _soundSlider = soundSlider;
    }

    private void Start()
    {
        _resolutionDropdown.ClearOptions();
        _resolutions = Screen.resolutions;

        var options = new List<string>();
        int currentResolutionIndex = _resolutions.Length - 1;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            options.Add($"{_resolutions[i].width} x {_resolutions[i].height}");

            if (Mathf.Approximately(_resolutions[i].width, Screen.width)
                && Mathf.Approximately(_resolutions[i].height, Screen.height))
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        LoadSettings();
    }

    // Updates the game's sound volume
    public void SetVolume(float volume)
    {
        _mixer.SetFloat("volume", volume);
        _soundSlider.value = volume;
    }

    // Updates the game's display resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution newResolution = _resolutions[resolutionIndex];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
        _resolutionDropdown.value = resolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    // Updates the game's fullscreen/windowed setting
    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        _fullscreenToggle.isOn = fullscreen;
    }

    // Saves settings to a Player preferences file
    public void SaveSettings()
    {
        _mixer.GetFloat("volume", out float volume);
        PlayerPrefs.SetFloat(s_volumePrefKey, volume);
        PlayerPrefs.SetInt(s_resolutionPrefKey, _resolutionDropdown.value);
        PlayerPrefs.SetInt(s_fullscreenPrefKey, Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.Save();
    }

    // Loads settings from a Player preferences file, or default values
    private void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat(s_volumePrefKey, 0f);
        int resolutionIndex = PlayerPrefs.GetInt(s_resolutionPrefKey, 0);
        bool fullscreen = Convert.ToBoolean(PlayerPrefs.GetInt(s_fullscreenPrefKey, 1));

        SetVolume(volume);
        SetResolution(resolutionIndex);
        SetFullScreen(fullscreen);
    }
}