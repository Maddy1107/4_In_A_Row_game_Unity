using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    public Slider volumeSlider;                  // Slider to control music volume
    public Toggle volumeToggle;                  // Toggle to mute/unmute music
    public Sprite volumeOnIcon;                  // Icon for unmuted state
    public Sprite volumeOffIcon;                 // Icon for muted state

    [Header("UI Navigation")]
    public Button backButton;                    // Button to return to main menu

    [Header("Toggles")]
    public List<RadioOption> options;            // Radio options for difficulty & SFX

    private AudioSettings audioSettings = new AudioSettings(); // Persistent audio config

    // Called when this menu is opened
    private void OnEnable()
    {
        audioSettings.Load();                    // Load saved audio settings
        SetupListeners();                        // Attach event handlers
        ApplySavedSettingsToUI();                // Reflect current settings in UI
        UpdateIcons();                           // Update toggle icon based on volume
    }

    // Called when this menu is closed
    private void OnDisable()
    {
        CleanupListeners();                      // Remove event handlers
        audioSettings.Save();                    // Save current settings to PlayerPrefs
    }

    // Add all necessary listeners
    private void SetupListeners()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);        // Volume slider
        volumeToggle.onValueChanged.AddListener(ToggleMute);            // Mute toggle
        backButton.onClick.AddListener(() => UIManager.Instance.ShowUI(UIState.MainMenu)); // Back button

        foreach (var option in options)                                  // Radio options
            option.Initialize(OnOptionSelected);
    }

    // Remove listeners to avoid memory leaks
    private void CleanupListeners()
    {
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeToggle.onValueChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        foreach (var option in options)
            option.Cleanup();
    }

    // Set UI values based on stored preferences
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

    // Volume slider changed
    private void OnVolumeChanged(float value)
    {
        audioSettings.SetVolume(value);          // Update audio settings
        UpdateIcons();                           // Update toggle icon
    }

    // Mute toggle switched
    private void ToggleMute(bool isOn)
    {
        audioSettings.ToggleMute(isOn);          // Mute or unmute music
        volumeSlider.value = audioSettings.CurrentVolume;
        UpdateIcons();
    }

    // A radio option (Difficulty or SFX) was selected
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

    // Update the toggle sprite depending on mute state
    private void UpdateIcons()
    {
        volumeToggle.GetComponent<Image>().sprite =
            audioSettings.MusicEnabled ? volumeOnIcon : volumeOffIcon;
    }
}
