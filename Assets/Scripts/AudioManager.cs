using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetMusicEnabled(bool isOn)
    {
        musicSource.mute = !isOn;
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
    }

    private void LoadSettings()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
        SetMusicEnabled(PlayerPrefs.GetInt("MusicEnabled", 1) == 1);
    }
}
