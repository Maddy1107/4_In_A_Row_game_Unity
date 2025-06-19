using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject settingsMenu;

    void OnEnable()
    {
        ShowUI(UIState.MainMenu);
    }

    public void ShowUI(UIState state)
    {
        mainMenu.SetActive(state == UIState.MainMenu);
        gameUI.SetActive(state == UIState.Game);
        settingsMenu.SetActive(state == UIState.SettingsMenu);
    }
}
