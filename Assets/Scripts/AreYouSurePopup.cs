using UnityEngine;
using UnityEngine.UI;

public class AreYouSurePopup : MonoBehaviour
{
    private PopupAnimator animator;

    [Header("Buttons")]
    [SerializeField] private Button stayButton;
    [SerializeField] private Button leaveButton;

    private void OnEnable()
    {
        stayButton.onClick.AddListener(Hide);
        leaveButton.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowUI(UIState.MainMenu);
        });

        GameManager.Instance.OnBack += Show;
    }

    private void OnDisable()
    {
        stayButton.onClick.RemoveAllListeners();
        leaveButton.onClick.RemoveAllListeners();

        GameManager.Instance.OnBack -= Show;
    }

    private void Awake()
    {
        animator = GetComponent<PopupAnimator>();
    }

    public void Show()
    {
        if (animator != null)
        {
            animator.AnimateIn();
        }
    }

    public void Hide()
    {
        if (animator != null)
        {
            animator.AnimateOut();
        }
    }
}
