using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class PopupAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform panel; // UI panel to animate

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>(); // Needed for fade in/out
        if (panel == null)
            panel = GetComponentInChildren<RectTransform>(); // Auto-assign if not set

        canvasGroup.alpha = 0f;           // Start fully invisible
        panel.localScale = Vector3.zero;  // Start fully shrunk
    }

    // Animate popup in with scale + fade
    public void AnimateIn(float duration = 0.4f)
    {
        StopAllCoroutines();
        gameObject.SetActive(true); // Ensure object is visible
        StartCoroutine(Fade(Vector3.zero, Vector3.one, 0f, 1f, duration));
    }

    // Animate popup out with reverse scale + fade
    public void AnimateOut(float duration = 0.25f)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(panel.localScale, Vector3.zero, 1f, 0f, duration, true));
    }

    // Handles both scale and alpha fading over time
    private IEnumerator Fade(Vector3 fromScale, Vector3 toScale, float fromAlpha, float toAlpha, float duration, bool deactivate = false)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / duration);
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, progress);
            panel.localScale = Vector3.Lerp(fromScale, toScale, Mathf.SmoothStep(0, 1, progress));
            yield return null;
        }

        canvasGroup.alpha = toAlpha;
        canvasGroup.interactable = toAlpha > 0f;     // Enable interaction if visible
        canvasGroup.blocksRaycasts = toAlpha > 0f;   // Block clicks only if visible
        panel.localScale = toScale;
    }
}
