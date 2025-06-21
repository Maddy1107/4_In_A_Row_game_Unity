using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Volume")]
    public Slider volumeSlider;
    public Toggle volumeToggle;

    [Header("Navigation")]
    public Button backButton;

    [Header("Icons")]
    public Sprite volumeOnIcon;
    public Sprite volumeOffIcon;

    [Header("Radio Options")]
    public List<RadioOption> options;

    private float volume = 1f;
    private bool musicEnabled = true;
    private bool sfxEnabled = true;
    private Difficulty difficulty = Difficulty.Easy;

    #region Unity Methods

    void OnEnable()
    {
        LoadSettings();
        SetupUIListeners();
        ApplySettingsToUI();
    }

    void OnDisable()
    {
        SaveSettings();
        CleanupListeners();
    }

    #endregion

    #region Setup

    void SetupUIListeners()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        volumeToggle.onValueChanged.AddListener(ToggleMute);
        backButton.onClick.AddListener(() => UIManager.Instance.ShowUI(UIState.MainMenu));

        foreach (var option in options)
        {
            option.Initialize(OnOptionSelected);
        }
    }

    void CleanupListeners()
    {
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeToggle.onValueChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    #endregion

    #region Settings State

    void LoadSettings()
    {
        volume = PlayerPrefs.GetFloat("Volume", 1f);
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        string savedDiff = PlayerPrefs.GetString("GameDifficulty", "Easy");
        if (!System.Enum.TryParse(savedDiff, out difficulty))
        {
            difficulty = Difficulty.Easy;
        }
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.SetInt("SFXEnabled", sfxEnabled ? 1 : 0);
        PlayerPrefs.SetString("GameDifficulty", difficulty.ToString());
        PlayerPrefs.Save();
    }

    #endregion

    #region UI → State Handlers

    void OnVolumeChanged(float value)
    {
        volume = value;
        musicEnabled = value > 0.01f;
        AudioListener.volume = musicEnabled ? volume : 0f;
        UpdateVolumeIcon();
    }

    void ToggleMute(bool isMuted)
    {
        musicEnabled = !isMuted;
        volume = musicEnabled ? 1f : 0f;
        volumeSlider.value = volume;
        AudioListener.volume = volume;
        UpdateVolumeIcon();
    }

    void OnOptionSelected(RadioOption selected)
    {
        switch (selected.type)
        {
            case RadioOptionType.Difficulty:
                difficulty = selected.difficultyValue;
                Debug.Log("Difficulty set to: " + difficulty);
                break;

            case RadioOptionType.SFX:
                sfxEnabled = selected.sfxEnabled;
                Debug.Log("SFX enabled: " + sfxEnabled);
                break;
        }
    }

    #endregion

    #region UI Update

    void ApplySettingsToUI()
    {
        volumeSlider.value = volume;
        volumeToggle.isOn = !musicEnabled;

        foreach (var option in options)
        {
            if (option.type == RadioOptionType.Difficulty && option.difficultyValue == difficulty)
                option.toggle.isOn = true;

            else if (option.type == RadioOptionType.SFX && option.sfxEnabled == sfxEnabled)
                option.toggle.isOn = true;
        }

        UpdateVolumeIcon();
    }

    void UpdateVolumeIcon()
    {
        if (volumeToggle != null)
            volumeToggle.GetComponent<Image>().sprite = musicEnabled ? volumeOnIcon : volumeOffIcon;
    }

    #endregion
}
