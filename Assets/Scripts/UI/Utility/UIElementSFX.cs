using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Ensures this script is only used on UI elements like Button, Toggle, etc.
[RequireComponent(typeof(Selectable))]
public class UIElementSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("SFX Keys")]
    [SerializeField] private UISFX hoverSFX = UISFX.None;  // Sound to play on hover
    [SerializeField] private UISFX clickSFX = UISFX.None;  // Sound to play on click

    // Triggered when the mouse enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        // If hover SFX is set and AudioManager is available, play it
        if (hoverSFX != UISFX.None && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(hoverSFX);
    }

    // Triggered when the UI element is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        // If click SFX is set and AudioManager is available, play it
        if (clickSFX != UISFX.None && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(clickSFX);
    }
}
