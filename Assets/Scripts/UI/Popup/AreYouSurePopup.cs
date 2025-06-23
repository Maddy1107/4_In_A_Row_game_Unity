using UnityEngine;
using UnityEngine.UI;

// Handles the "Are You Sure?" confirmation popup UI
public class AreYouSurePopup : MonoBehaviour
{
    private PopupAnimator animator;

    [Header("Buttons")]
    [SerializeField] private Button stayButton;  // Button to stay in the game
    [SerializeField] private Button leaveButton; // Button to go back to main menu

    // Called when the popup becomes active
    private void OnEnable()
    {
        stayButton.onClick.AddListener(Hide); // Hide on "Stay"
        leaveButton.onClick.AddListener(() =>
        {
            Hide(); // Hide popup
            UIManager.Instance.ShowUI(UIState.MainMenu); // Go to main menu
        });

        GameManager.Instance.OnBack += Show; // Listen for back input
    }

    // Called when the popup is disabled
    private void OnDisable()
    {
        stayButton.onClick.RemoveAllListeners(); // Clean up listeners
        leaveButton.onClick.RemoveAllListeners();

        GameManager.Instance.OnBack -= Show; // Stop listening for back input
    }

    // Called when the object is created
    private void Awake()
    {
        animator = GetComponent<PopupAnimator>(); // Get the animator component
    }

    // Show the popup with animation
    public void Show()
    {
        if (animator != null)
        {
            animator.AnimateIn();
        }
    }

    // Hide the popup with animation
    public void Hide()
    {
        if (animator != null)
        {
            animator.AnimateOut();
        }
    }
}
