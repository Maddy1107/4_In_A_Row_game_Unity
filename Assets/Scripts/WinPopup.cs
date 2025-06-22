using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
    private PopupAnimator animator;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private Image resutlImage;

    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Sprites")]
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite drawSprite;

    void OnEnable()
    {
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowUI(UIState.MainMenu);
        });

        GameManager.Instance.OnGameOver += Show;
    }

    private void RestartGame()
    {
        Hide();
        GameManager.Instance.RestartGame();
    }

    void OnDisable()
    {
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();

        GameManager.Instance.OnGameOver -= Show;
    }

    private void Awake()
    {
        animator = GetComponent<PopupAnimator>();
    }

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
        animator.AnimateIn();
    }

    public void Hide()
    {
        animator.AnimateOut();
    }
}
