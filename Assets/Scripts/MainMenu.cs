using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button localGameButton;
    [SerializeField] private Button aiGameButton;
    [SerializeField] private Button settingsButton;

    void OnEnable()
    {
        localGameButton.onClick.AddListener(() => StartGame(GameMode.Local));
        aiGameButton.onClick.AddListener(() => StartGame(GameMode.AI));
        settingsButton.onClick.AddListener(OpenSettings);
    }

    void OnDisable()
    {
        localGameButton.onClick.RemoveAllListeners();
        aiGameButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
    }

    private void StartGame(GameMode mode)
    {
        UIManager.Instance.ShowUI(UIState.Game);
        GameManager.Instance.SelectMode(mode);
    }

    private void OpenSettings()
    {
        UIManager.Instance.ShowUI(UIState.SettingsMenu);
    }
}
