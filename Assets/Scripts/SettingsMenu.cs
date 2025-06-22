using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    public Slider volumeSlider;
    public Toggle volumeToggle;
    public Sprite volumeOnIcon;
    public Sprite volumeOffIcon;

    [Header("UI Navigation")]
    public Button backButton;

    [Header("Toggles")]
    public List<RadioOption> options;

    private AudioSettings audioSettings = new AudioSettings();

    private void OnEnable()
    {
        audioSettings.Load();

        SetupListeners();
        ApplySavedSettingsToUI();
        UpdateIcons();
    }

    private void OnDisable()
    {
        CleanupListeners();
        audioSettings.Save();
    }

    private void SetupListeners()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        volumeToggle.onValueChanged.AddListener(ToggleMute);
        backButton.onClick.AddListener(() => UIManager.Instance.ShowUI(UIState.MainMenu));

        foreach (var option in options)
            option.Initialize(OnOptionSelected);
    }

    private void CleanupListeners()
    {
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeToggle.onValueChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        foreach (var option in options)
            option.Initialize(OnOptionSelected);
    }

    private void ApplySavedSettingsToUI()
    {
        volumeSlider.value = audioSettings.CurrentVolume;
        volumeToggle.isOn = !audioSettings.MusicEnabled;

        string savedDiff = PlayerPrefs.GetString("GameDifficulty", "Easy");
        bool sfxOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        foreach (var option in options)
        {
            if (option.type == RadioOptionType.Difficulty)
            {
                option.toggle.isOn = option.difficultyValue.ToString() == savedDiff;
            }
            else if (option.type == RadioOptionType.SFX)
            {
                option.toggle.isOn = option.sfxEnabled == sfxOn;
            }
        }

    }

    private void OnVolumeChanged(float value)
    {
        audioSettings.SetVolume(value);
        UpdateIcons();
    }

    private void ToggleMute(bool isOn)
    {
        audioSettings.ToggleMute(isOn);
        volumeSlider.value = audioSettings.CurrentVolume;
        UpdateIcons();
    }

    private void OnOptionSelected(RadioOption selected)
    {
        if (selected.type == RadioOptionType.Difficulty)
        {
            PlayerPrefs.SetString("GameDifficulty", selected.difficultyValue.ToString());
            Debug.Log("Difficulty set to: " + selected.difficultyValue);
        }
        else if (selected.type == RadioOptionType.SFX)
        {
            audioSettings.ToggleSFX(selected.sfxEnabled);
            Debug.Log("SFX enabled: " + selected.sfxEnabled);
        }
    }

    private void UpdateIcons()
    {
        volumeToggle.GetComponent<Image>().sprite = audioSettings.MusicEnabled ? volumeOnIcon : volumeOffIcon;
    }
}
