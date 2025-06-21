using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image pieceImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Image glowImage;

    private Button button;
    public int row, col;

    [SerializeField] private Sprite player1Sprite;
    [SerializeField] private Sprite player2Sprite;

    public void Init(int r, int c)
    {
        row = r;
        col = c;

        if (button == null)
            button = GetComponent<Button>();

        ResetCell();

        glowImage.enabled = false;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => GameManager.Instance.OnCellClicked(col));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.OnCellHover(col, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.OnCellHover(col, false);
    }

    public void StartGlow()
    {
        if (glowImage == null) return;

        glowImage.enabled = true;

        StartCoroutine(FadeInGlow());
    }

    private IEnumerator FadeInGlow(float duration = 2f, float pulseSpeed = 2f)
    {
        Color baseColor = glowImage.color;
        baseColor.a = 0f;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Abs(Mathf.Sin(t * pulseSpeed));
            Color c = baseColor;
            c.a = alpha;
            glowImage.color = c;

            yield return null;
        }
    }


    public void SetHighLightPiece(int playerId)
    {
        highlightImage.enabled = true;
        highlightImage.sprite = playerId == 1 ? player1Sprite : player2Sprite;
        highlightImage.color = new Color(highlightImage.color.r, highlightImage.color.g, highlightImage.color.b, 0.5f);
    }

    public void SetPiece(int playerId)
    {
        StartCoroutine(DropPiece(playerId));
    }

    private IEnumerator DropPiece(int playerId)
    {
        pieceImage.enabled = true;
        pieceImage.sprite = playerId == 1 ? player1Sprite : player2Sprite;
        button.interactable = false;

        var rect = pieceImage.rectTransform;
        var start = new Vector2(rect.anchoredPosition.x, 800f);
        var end = rect.anchoredPosition;
        float t = 0, duration = 0.1f;

        rect.anchoredPosition = start;
        while (t < duration)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(start, end, t / duration);
            yield return null;
        }

        // 🔁 Bounce
        float bounceHeight = 25f;
        float bounceDuration = 0.3f;
        Vector2 peak = end + Vector2.up * bounceHeight;

        for (float bounceT = 0; bounceT < bounceDuration; bounceT += Time.deltaTime)
        {
            float p = bounceT / bounceDuration;
            rect.anchoredPosition = Vector2.Lerp(end, peak, 4 * p * (1 - p));
            yield return null;
        }

        rect.anchoredPosition = end;
    }

    public void ResetCell()
    {
        pieceImage.enabled = false;
        pieceImage.color = Color.white;

        highlightImage.enabled = false;
        highlightImage.color = Color.white;

        if (button == null)
            button = GetComponent<Button>();
        button.interactable = true;
    }
}
