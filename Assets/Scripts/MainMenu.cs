using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button localGameButton;
    [SerializeField] private Button aiGameButton;
    [SerializeField] private Button settingsButton;

    void OnEnable()
    {
        localGameButton.onClick.AddListener(() => StartGame(PlayerType.Human, PlayerType.Human));
        aiGameButton.onClick.AddListener(() => StartGame(PlayerType.Human, PlayerType.AI));
        settingsButton.onClick.AddListener(OpenSettings);
    }

    void OnDisable()
    {
        localGameButton.onClick.RemoveAllListeners();
        aiGameButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
    }

    private void StartGame(PlayerType p1, PlayerType p2)
    {
        GameManager.Instance.CreatePlayer(p1, p2);
        UIManager.Instance.ShowUI(UIState.Game);
    }

    private void OpenSettings()
    {
        UIManager.Instance.ShowUI(UIState.SettingsMenu);
    }
}
