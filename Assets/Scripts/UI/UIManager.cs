using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton instance for global access
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        // Destroy duplicate instances
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // UI panels to toggle
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject settingsMenu;

    private void OnEnable()
    {
        // Show main menu on enable (first load)
        ShowUI(UIState.MainMenu);
    }

    // Shows the desired UI state, hides others
    public void ShowUI(UIState state)
    {
        mainMenu.SetActive(state == UIState.MainMenu);
        gameUI.SetActive(state == UIState.Game);
        settingsMenu.SetActive(state == UIState.SettingsMenu);
    }
}
