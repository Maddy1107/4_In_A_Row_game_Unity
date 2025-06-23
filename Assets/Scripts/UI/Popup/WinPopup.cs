using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
    private PopupAnimator animator;

    [SerializeField] private TMP_Text winText;           // Text to display win/draw message
    [SerializeField] private Image resutlImage;          // Image to show win/draw sprite

    [Header("Buttons")]
    [SerializeField] private Button restartButton;       // Button to restart the game
    [SerializeField] private Button mainMenuButton;      // Button to return to main menu

    [Header("Sprites")]
    [SerializeField] private Sprite winSprite;           // Sprite shown on win
    [SerializeField] private Sprite drawSprite;          // Sprite shown on draw

    void OnEnable()
    {
        // Set up button click listeners
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowUI(UIState.MainMenu);
        });

        // Subscribe to game over event
        GameManager.Instance.OnGameOver += Show;
    }

    private void RestartGame()
    {
        Hide();
        GameManager.Instance.RestartGame(); // Restart game on button press
    }

    void OnDisable()
    {
        // Clean up listeners
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();

        GameManager.Instance.OnGameOver -= Show;
    }

    private void Awake()
    {
        animator = GetComponent<PopupAnimator>(); // Get animator component
    }

    // Show win/draw popup with appropriate text and image
    public void Show(IPlayerController player, Result result)
    {
        if (player == null || result == Result.Draw)
        {
            AudioManager.Instance?.PlaySFX(UISFX.Lose);
            winText.text = "It's a Draw!";
            resutlImage.sprite = drawSprite;
        }
        else
        {
            winText.text = player.IsHuman ? $"Player {player.PlayerId}\nWins!" : "AI\nWins!";
            resutlImage.sprite = winSprite;
        }

        animator.AnimateIn(); // Animate popup in
    }

    public void Hide()
    {
        animator.AnimateOut(); // Animate popup out
    }
}
