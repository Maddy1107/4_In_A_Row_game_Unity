using UnityEngine;
using System.Collections.Generic;

// Handles all music and sound effect playback in the game
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Settings")]
    public AudioSource musicSource; // Source for background music

    [Header("SFX Settings")]
    public AudioSource sfxSource; // Source for sound effects
    public List<SFXEntry> sfxEntries; // List of SFX key-clip pairs

    private Dictionary<UISFX, AudioClip> sfxMap = new Dictionary<UISFX, AudioClip>(); // Quick lookup for SFX clips

    // PlayerPrefs keys
    private const string VolumeKey = "MusicVolume";
    private const string MusicEnabledKey = "MusicEnabled";
    private const string SFXEnabledKey = "SFXEnabled";

    [System.Serializable]
    public class SFXEntry
    {
        public UISFX key;        // Identifier for the SFX
        public AudioClip clip;   // Associated audio clip
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes

        // Populate SFX dictionary
        foreach (var entry in sfxEntries)
        {
            if (!sfxMap.ContainsKey(entry.key))
                sfxMap[entry.key] = entry.clip;
        }

        LoadMusicSettings(); // Apply saved volume/mute state
    }

    // ========== MUSIC CONTROL ==========

    // Set and save music volume
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource.mute = volume <= 0.01f;
        PlayerPrefs.SetFloat(VolumeKey, volume);
    }

    // Get saved music volume
    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(VolumeKey, 1f);
    }

    // Enable or disable music globally
    public void SetMusicEnabled(bool isOn)
    {
        PlayerPrefs.SetInt(MusicEnabledKey, isOn ? 1 : 0);
    }

    // Check if music is enabled
    public bool IsMusicEnabled()
    {
        return PlayerPrefs.GetInt(MusicEnabledKey, 1) == 1;
    }

    // ========== SFX CONTROL ==========

    // Play a specific UI sound effect
    public void PlaySFX(UISFX key)
    {
        if (!IsSFXEnabled() || !sfxMap.ContainsKey(key)) return;

        AudioClip clip = sfxMap[key];
        sfxSource.PlayOneShot(clip);
    }

    // Enable or disable sound effects globally
    public void SetSFXEnabled(bool isOn)
    {
        PlayerPrefs.SetInt(SFXEnabledKey, isOn ? 1 : 0);
    }

    // Check if SFX are enabled
    public bool IsSFXEnabled()
    {
        return PlayerPrefs.GetInt(SFXEnabledKey, 1) == 1;
    }

    // ========== LOAD SETTINGS ==========

    // Load and apply saved music settings
    private void LoadMusicSettings()
    {
        float volume = GetMusicVolume();
        bool musicEnabled = IsMusicEnabled();

        musicSource.volume = volume;
        musicSource.mute = !musicEnabled || volume <= 0.01f;
    }
}
