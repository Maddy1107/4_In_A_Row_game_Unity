using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle musicToggle;

    void OnEnable()
    {
        volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        musicToggle.onValueChanged.AddListener(AudioManager.Instance.SetMusicEnabled);
    }
    void OnDisable()
    {
        volumeSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetMusicVolume);
        musicToggle.onValueChanged.RemoveListener(AudioManager.Instance.SetMusicEnabled);
    }

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
    }
}
