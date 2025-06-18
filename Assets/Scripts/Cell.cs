using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image pieceImage;
    [SerializeField] private Image highlightImage;

    private Button button;
    public int row, col;

    private static readonly Color Player1Color = new Color(1f, 0f, 0f, 1f);
    private static readonly Color Player2Color = new Color(1f, 1f, 0f, 1f);
    private static readonly Color Player1Highlight = new Color(1f, 0f, 0f, 0.3f);
    private static readonly Color Player2Highlight = new Color(1f, 1f, 0f, 0.3f);

    public void Init(int r, int c)
    {
        row = r;
        col = c;

        if (button == null)
            button = GetComponent<Button>();

        ResetCell();

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

    public void SetHighLightPiece(int playerId)
    {
        highlightImage.enabled = true;
        highlightImage.color = playerId == 1 ? Player1Highlight : Player2Highlight;
    }

    public void SetPiece(int playerId)
    {
        pieceImage.enabled = true;
        pieceImage.color = playerId == 1 ? Player1Color : Player2Color;
        button.interactable = false;
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
