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

        // Retrieve available resolutions as strings
        var options = new List<string>();
        int currentResolutionIndex = _resolutions.Length - 1;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            options.Add($"{_resolutions[i].width} x {_resolutions[i].height}@{_resolutions[i].refreshRate}hz");

            if (Mathf.Approximately(_resolutions[i].width, Screen.width)
                && Mathf.Approximately(_resolutions[i].height, Screen.height))
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        LoadSettings();
    }

    /// <summary>
    /// Updates the game's sound volume.
    /// </summary>
    public void SetVolume(float volume)
    {
        _mixer.SetFloat("volume", volume);
        _soundSlider.value = volume;
    }

    /// <summary>
    /// Updates the game's display resolution.
    /// </summary>
    public void SetResolution(int resolutionIndex)
    {
        Resolution newResolution = _resolutions[resolutionIndex];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
        _resolutionDropdown.value = resolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Updates the game's fullscreen/windowed setting.
    /// </summary>
    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        _fullscreenToggle.isOn = fullscreen;
    }

    /// <summary>
    /// Saves settings to player preferences.
    /// </summary>
    public void SaveSettings()
    {
        _mixer.GetFloat("volume", out float volume);
        PlayerPrefs.SetFloat(s_volumePrefKey, volume);
        PlayerPrefs.SetInt(s_resolutionPrefKey, _resolutionDropdown.value);
        PlayerPrefs.SetInt(s_fullscreenPrefKey, Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Clears all saved data, including game progression
    /// </summary>
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// Loads settings from a player preferences, or loads default values.
    /// </summary>
    private void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat(s_volumePrefKey, 0f);
        int resolutionIndex = PlayerPrefs.GetInt(s_resolutionPrefKey, _resolutions.Length - 1);
        bool fullscreen = Convert.ToBoolean(PlayerPrefs.GetInt(s_fullscreenPrefKey, 1));

        SetVolume(volume);
        SetResolution(resolutionIndex);
        SetFullScreen(fullscreen);
    }
}
