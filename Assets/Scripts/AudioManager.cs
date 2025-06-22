using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Settings")]
    public AudioSource musicSource;

    [Header("SFX Settings")]
    public AudioSource sfxSource;
    public List<SFXEntry> sfxEntries;

    private Dictionary<UISFX, AudioClip> sfxMap = new Dictionary<UISFX, AudioClip>();

    private const string VolumeKey = "MusicVolume";
    private const string MusicEnabledKey = "MusicEnabled";
    private const string SFXEnabledKey = "SFXEnabled";

    [System.Serializable]
    public class SFXEntry
    {
        public UISFX key;
        public AudioClip clip;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var entry in sfxEntries)
        {
            if (!sfxMap.ContainsKey(entry.key))
                sfxMap[entry.key] = entry.clip;
        }


        LoadMusicSettings();
    }

    // MUSIC CONTROL

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource.mute = volume <= 0.01f;
        PlayerPrefs.SetFloat(VolumeKey, volume);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(VolumeKey, 1f);
    }
    public void SetMusicEnabled(bool isOn)
    {
        PlayerPrefs.SetInt(MusicEnabledKey, isOn ? 1 : 0);
    }
    public bool IsMusicEnabled()
    {
        return PlayerPrefs.GetInt(MusicEnabledKey, 1) == 1;
    }

    // SFX CONTROL

    public void PlaySFX(UISFX key)
    {
        if (!IsSFXEnabled() || !sfxMap.ContainsKey(key)) return;

        AudioClip clip = sfxMap[key];
        sfxSource.PlayOneShot(clip);
    }

    public void SetSFXEnabled(bool isOn)
    {
        PlayerPrefs.SetInt(SFXEnabledKey, isOn ? 1 : 0);
    }

    public bool IsSFXEnabled()
    {
        return PlayerPrefs.GetInt(SFXEnabledKey, 1) == 1;
    }

    // LOAD CONTROLS
    private void LoadMusicSettings()
    {
        float volume = GetMusicVolume();
        bool musicEnabled = IsMusicEnabled();

        musicSource.volume = volume;
        musicSource.mute = !musicEnabled || volume <= 0.01f;
    }
}
