using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Represents an individual cell on the board
public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image pieceImage;      // Shows placed piece
    [SerializeField] private Image highlightImage;  // Shows hover piece preview
    [SerializeField] private Image glowImage;       // Shows glowing win effect

    private Button button;
    public int row, col;

    [SerializeField] private Sprite player1Sprite;  // Sprite for player 1
    [SerializeField] private Sprite player2Sprite;  // Sprite for player 2

    // Initialize cell with row and column values
    public void Init(int r, int c)
    {
        row = r;
        col = c;

        if (button == null)
            button = GetComponent<Button>();

        ResetCell(); // Reset visuals

        glowImage.enabled = false;

        button.onClick.RemoveAllListeners(); // Clear old listeners
        button.onClick.AddListener(() => GameManager.Instance.OnCellClicked(col)); // Hook click to column
    }

    // Triggered when mouse pointer enters cell
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.OnCellHover(col, true);
    }

    // Triggered when mouse pointer exits cell
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.OnCellHover(col, false);
    }

    // Starts glowing effect for win animation
    public void StartGlow()
    {
        if (glowImage == null) return;

        glowImage.enabled = true;
        StartCoroutine(FadeInGlow());
    }

    // Fades in the glow with pulsing animation
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

    // Sets a semi-transparent highlight for hover preview
    public void SetHighLightPiece(int playerId)
    {
        highlightImage.enabled = true;
        highlightImage.sprite = playerId == 1 ? player1Sprite : player2Sprite;
        highlightImage.color = new Color(highlightImage.color.r, highlightImage.color.g, highlightImage.color.b, 0.5f);
    }

    // Triggers drop animation and sets the placed piece sprite
    public void SetPiece(int playerId)
    {
        StartCoroutine(DropPiece(playerId));
    }

    // Animates the falling piece and bounce effect
    private IEnumerator DropPiece(int playerId)
    {
        pieceImage.enabled = true;
        pieceImage.sprite = playerId == 1 ? player1Sprite : player2Sprite;
        button.interactable = false;

        var rect = pieceImage.rectTransform;
        var start = new Vector2(rect.anchoredPosition.x, 800f); // Drop from top
        var end = rect.anchoredPosition;
        float t = 0, duration = 0.1f;

        rect.anchoredPosition = start;
        while (t < duration)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(start, end, t / duration);
            yield return null;
        }

        // Add a bounce effect after drop
        float bounceHeight = 25f;
        float bounceDuration = 0.3f;
        Vector2 peak = end + Vector2.up * bounceHeight;

        for (float bounceT = 0; bounceT < bounceDuration; bounceT += Time.deltaTime)
        {
            float p = bounceT / bounceDuration;
            rect.anchoredPosition = Vector2.Lerp(end, peak, 4 * p * (1 - p)); // Ease bounce
            yield return null;
        }

        rect.anchoredPosition = end;
    }

    // Resets the cell to its default state
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
