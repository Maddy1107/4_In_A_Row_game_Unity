using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class UIElementSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("SFX Keys")]
    public UISFX hoverSFX;
    public UISFX clickSFX;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSFX == UISFX.None) return;
        AudioManager.Instance?.PlaySFX(hoverSFX);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSFX == UISFX.None) return;
        AudioManager.Instance?.PlaySFX(clickSFX);
    }
}
