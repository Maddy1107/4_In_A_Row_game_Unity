using UnityEngine;
using UnityEngine.UI;

// Handles main menu interactions and button navigation
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button localGameButton;    // Button to start local multiplayer
    [SerializeField] private Button aiGameButton;       // Button to start vs AI
    [SerializeField] private Button settingsButton;     // Button to open settings menu

    void OnEnable()
    {
        // Add button listeners when menu becomes active
        localGameButton.onClick.AddListener(() => StartGame(GameMode.Local));
        aiGameButton.onClick.AddListener(() => StartGame(GameMode.AI));
        settingsButton.onClick.AddListener(OpenSettings);
    }

    void OnDisable()
    {
        // Clean up listeners to avoid duplicate calls or memory leaks
        localGameButton.onClick.RemoveAllListeners();
        aiGameButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
    }

    // Starts the selected game mode and switches to game UI
    private void StartGame(GameMode mode)
    {
        UIManager.Instance.ShowUI(UIState.Game);         // Switch to game screen
        GameManager.Instance.SelectMode(mode);           // Initialize game mode in GameManager
    }

    // Opens the settings menu UI
    private void OpenSettings()
    {
        UIManager.Instance.ShowUI(UIState.SettingsMenu);
    }
}
