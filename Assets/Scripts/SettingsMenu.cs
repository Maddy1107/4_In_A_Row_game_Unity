using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle volumeButton;
    public Toggle sfxToggle;
    public Button backButton;
    [Header("Icons")]
    public Sprite volumeOnIcon;
    public Sprite volumeOffIcon;
    public Sprite sfxOnIcon;
    public Sprite sfxOffIcon;

    private AudioSettings audioSettings = new AudioSettings();

    void OnEnable()
    {
        audioSettings.Load();
        volumeSlider.value = audioSettings.CurrentVolume;
        sfxToggle.isOn = audioSettings.SFXEnabled;
        volumeButton.isOn = !audioSettings.MusicEnabled;

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        volumeButton.onValueChanged.AddListener(ToggleMute);
        sfxToggle.onValueChanged.AddListener(ToggleSFX);
        backButton.onClick.AddListener(() => UIManager.Instance.ShowUI(UIState.MainMenu));

        UpdateIcons();
    }

    void OnDisable()
    {
        audioSettings.Save();
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        volumeButton.onValueChanged.RemoveListener(ToggleMute);
        sfxToggle.onValueChanged.RemoveListener(ToggleSFX);
        backButton.onClick.RemoveAllListeners();
    }

    void Start()
    {
        sfxToggle.isOn = audioSettings.SFXEnabled;
        volumeButton.isOn = !audioSettings.MusicEnabled;
    }

    void OnVolumeChanged(float value)
    {
        audioSettings.SetVolume(value);
        UpdateIcons();
    }

    void ToggleSFX(bool isOn)
    {
        audioSettings.ToggleSFX(isOn);
        UpdateIcons();
    }

    void ToggleMute(bool isOn)
    {
        audioSettings.ToggleMute(isOn);
        volumeSlider.value = audioSettings.CurrentVolume;
        UpdateIcons();
    }

    void UpdateIcons()
    {
        volumeButton.GetComponent<Image>().sprite = audioSettings.MusicEnabled ? volumeOnIcon : volumeOffIcon;
        sfxToggle.GetComponent<Image>().sprite = audioSettings.SFXEnabled ? sfxOnIcon : sfxOffIcon;
    }
}
