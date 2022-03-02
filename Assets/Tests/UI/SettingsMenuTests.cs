using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

[Category("UITests")]
public class SettingsMenuTests
{
    private SettingsMenu _settingsMenu;
    private AudioMixer _mixer;
    private Slider _soundSlider;
    private TMP_Dropdown _resolutionDropdown;
    private Toggle _fullscreenToggle;

    [SetUp]
    public void SetUp()
    {
        _settingsMenu = new GameObject().AddComponent<SettingsMenu>();
        _mixer = Resources.Load<AudioMixer>("Sound/MainMixer");
        _soundSlider = new GameObject().AddComponent<Slider>();
        _resolutionDropdown = new GameObject().AddComponent<TMP_Dropdown>();
        _fullscreenToggle = new GameObject().AddComponent<Toggle>();
        _settingsMenu.Construct(_mixer, _resolutionDropdown, _fullscreenToggle, _soundSlider);
    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SetActive(true);
            Object.Destroy(go);
        }
    }

    [Test]
    public void SetVolume_ShouldChangeMixerVolume()
    {
        _settingsMenu.SetVolume(20f);

        _mixer.GetFloat("volume", out float volume);
        Assert.AreEqual(20f, volume);
    }

    [Test]
    public void SetVolume_ShouldChangeSoundSliderValue()
    {
        _settingsMenu.SetVolume(0.5f);
        Assert.AreEqual(0.5f, _soundSlider.value);
    }

    [Test]
    public void SetFullscreenTrue_ShouldEnableFullscreenToggle()
    {
        _settingsMenu.SetFullScreen(true);
        Assert.IsTrue(_fullscreenToggle.isOn);
    }

    [Test]
    public void SetFullscreenFalse_ShouldDisableFullscreenToggle()
    {
        _settingsMenu.SetFullScreen(false);
        Assert.IsFalse(_fullscreenToggle.isOn);
    }

    [Test]
    public void ClearData_EmptiesPlayerPreferences()
    {
        PlayerPrefs.SetFloat("volume", 1f);
        PlayerPrefs.SetInt("resolution", 1);
        PlayerPrefs.SetInt("fullscreen", 1);
        PlayerPrefs.SetInt("unlockedScene", 1);

        _settingsMenu.ClearData();

        Assert.IsFalse(PlayerPrefs.HasKey("volume"));
        Assert.IsFalse(PlayerPrefs.HasKey("resolution"));
        Assert.IsFalse(PlayerPrefs.HasKey("fullscreen"));
        Assert.IsFalse(PlayerPrefs.HasKey("unlockedScene"));
    }

    [UnityTest]
    public IEnumerator SetResolution_ShouldUpdateSelectedResolution()
    {
        yield return null;
        _settingsMenu.SetResolution(1);
        Assert.AreEqual(1, _resolutionDropdown.value);
    }
}
