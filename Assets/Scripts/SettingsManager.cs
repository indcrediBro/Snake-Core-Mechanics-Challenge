using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    [Header("Audio Settings")]
    public AudioMixer audioMixer;            // Reference to the AudioMixer
    public string musicVolumeParameter = "MusicVolume";
    public string sfxVolumeParameter = "SFXVolume";

    private float musicVolume;
    private float sfxVolume;

    [Header("UI Settings")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public Sprite switchIcon, switchIconFlipped;
    public Image cameraShakeImageRenderer, screenFlashImageRenderer;
    public bool cameraShakeEnabled, screenFlashEnabled;

    private void Start()
    {
        InitializeUI();
        LoadSettings();
    }

    private void InitializeUI()
    {
        // Initialize sliders with current settings
        musicVolumeSlider.value = GetMusicVolume();
        sfxVolumeSlider.value = GetSFXVolume();
        cameraShakeEnabled = GetCameraShakeActiveness() != 0;
        screenFlashEnabled = GetScreenFlashActiveness() != 0;

        // Add listeners to sliders
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        if (cameraShakeEnabled)
        {
            cameraShakeImageRenderer.sprite = switchIcon;
        }
        else
        {
            cameraShakeImageRenderer.sprite = switchIconFlipped;
        }

        if (screenFlashEnabled)
        {
            screenFlashImageRenderer.sprite = switchIcon;
        }
        else
        {
            screenFlashImageRenderer.sprite = switchIconFlipped;
        }
    }

    private void OnMusicVolumeChanged(float value)
    {
        SetMusicVolume(value);
        ApplySettings();
    }

    private void OnSFXVolumeChanged(float value)
    {
        SetSFXVolume(value);
        ApplySettings();
    }

    private void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(musicVolumeParameter, musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    private void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(sfxVolumeParameter, sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }

    public int GetCameraShakeActiveness()
    {
        return PlayerPrefs.GetInt("CameraShake", 1);
    }
    public int GetScreenFlashActiveness()
    {
        return PlayerPrefs.GetInt("ScreenFlash", 1);
    }
    public void SetCameraShakeActiveness()
    {
        if (cameraShakeEnabled)
        {
            PlayerPrefs.SetInt("CameraShake", 0);
            cameraShakeEnabled = false;
            cameraShakeImageRenderer.sprite = switchIconFlipped;
        }
        else
        {
            PlayerPrefs.SetInt("CameraShake", 1);
            cameraShakeEnabled = true;
            cameraShakeImageRenderer.sprite = switchIcon;
        }
    }
    public void SetScreenFlashActiveness()
    {
        if (screenFlashEnabled)
        {
            PlayerPrefs.SetInt("ScreenFlash", 0);
            screenFlashEnabled = false;
            screenFlashImageRenderer.sprite = switchIconFlipped;
        }
        else
        {
            PlayerPrefs.SetInt("ScreenFlash", 1);
            screenFlashEnabled = true;
            screenFlashImageRenderer.sprite = switchIcon;
        }
    }

    private void LoadSettings()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));
    }

    private void ApplySettings()
    {
        audioMixer.SetFloat(musicVolumeParameter, Mathf.Log10(GetMusicVolume()) * 20);
        audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(GetSFXVolume()) * 20);
    }
}
