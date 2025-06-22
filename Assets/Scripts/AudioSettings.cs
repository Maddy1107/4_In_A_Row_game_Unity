using UnityEngine;

public class AudioSettings
{
    private const string MusicVolumeKey = "MusicVolume";
    private const string LastVolumeKey = "LastVolumeBeforeMute";
    private const string SFXEnabledKey = "SFXEnabled";

    public float CurrentVolume { get; private set; }
    public float LastVolumeBeforeMute { get; private set; }
    public bool IsMuted => Mathf.Approximately(CurrentVolume, 0f);
    public bool SFXEnabled { get; private set; }
    public bool MusicEnabled => !IsMuted;

    public void Load()
    {
        CurrentVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        LastVolumeBeforeMute = PlayerPrefs.GetFloat(LastVolumeKey, 1f);
        SFXEnabled = PlayerPrefs.GetInt(SFXEnabledKey, 1) == 1;

        if (!IsMuted)
            LastVolumeBeforeMute = CurrentVolume;

        Apply();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, CurrentVolume);
        PlayerPrefs.SetFloat(LastVolumeKey, LastVolumeBeforeMute);
        PlayerPrefs.SetInt(SFXEnabledKey, SFXEnabled ? 1 : 0);
    }

    public void SetVolume(float volume)
    {
        CurrentVolume = volume;
        if (!IsMuted)
            LastVolumeBeforeMute = volume;
        ApplyVolume();
    }

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
        ApplyVolume();
    }

    public void ToggleSFX(bool enabled)
    {
        SFXEnabled = enabled;
        ApplySFX();
    }

    private void Apply()
    {
        ApplyVolume();
        ApplySFX();
    }

    private void ApplyVolume()
    {
        AudioManager.Instance.SetMusicEnabled(MusicEnabled);
        AudioManager.Instance.SetMusicVolume(CurrentVolume);
    }

    private void ApplySFX()
    {
        AudioManager.Instance.SetSFXEnabled(SFXEnabled);
    }
}
