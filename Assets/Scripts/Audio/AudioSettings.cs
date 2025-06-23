using UnityEngine;

// Handles saving, loading, and applying audio settings
public class AudioSettings
{
    // PlayerPrefs keys
    private const string MusicVolumeKey = "MusicVolume";
    private const string LastVolumeKey = "LastVolumeBeforeMute";
    private const string SFXEnabledKey = "SFXEnabled";

    // Current music volume
    public float CurrentVolume { get; private set; }

    // Last volume before muting (for restoring after unmute)
    public float LastVolumeBeforeMute { get; private set; }

    // True if muted
    public bool IsMuted => Mathf.Approximately(CurrentVolume, 0f);

    // True if SFX is enabled
    public bool SFXEnabled { get; private set; }

    // True if music is enabled (i.e., not muted)
    public bool MusicEnabled => !IsMuted;

    // Load settings from PlayerPrefs
    public void Load()
    {
        CurrentVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        LastVolumeBeforeMute = PlayerPrefs.GetFloat(LastVolumeKey, 1f);
        SFXEnabled = PlayerPrefs.GetInt(SFXEnabledKey, 1) == 1;

        if (!IsMuted)
            LastVolumeBeforeMute = CurrentVolume;

        Apply(); // Apply loaded settings to the AudioManager
    }

    // Save current settings to PlayerPrefs
    public void Save()
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, CurrentVolume);
        PlayerPrefs.SetFloat(LastVolumeKey, LastVolumeBeforeMute);
        PlayerPrefs.SetInt(SFXEnabledKey, SFXEnabled ? 1 : 0);
    }

    // Set volume and update last unmuted volume
    public void SetVolume(float volume)
    {
        CurrentVolume = volume;
        if (!IsMuted)
            LastVolumeBeforeMute = volume;
        ApplyVolume(); // Push changes to AudioManager
    }

    // Mute or unmute the music
    public void ToggleMute(bool mute)
    {
        if (mute)
        {
            LastVolumeBeforeMute = CurrentVolume;
            CurrentVolume = 0f;
        }
        else
        {
            CurrentVolume = LastVolumeBeforeMute > 0.001f ? LastVolumeBeforeMute : 1f;
        }
        ApplyVolume(); // Reflect mute state in AudioManager
    }

    // Enable or disable SFX
    public void ToggleSFX(bool enabled)
    {
        SFXEnabled = enabled;
        ApplySFX(); // Update SFX state in AudioManager
    }

    // Apply all settings to AudioManager
    private void Apply()
    {
        ApplyVolume();
        ApplySFX();
    }

    // Apply volume and mute state
    private void ApplyVolume()
    {
        AudioManager.Instance.SetMusicEnabled(MusicEnabled);
        AudioManager.Instance.SetMusicVolume(CurrentVolume);
    }

    // Apply SFX state
    private void ApplySFX()
    {
        AudioManager.Instance.SetSFXEnabled(SFXEnabled);
    }
}
