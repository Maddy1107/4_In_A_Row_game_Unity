using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image pieceImage;
    public Button button;

    private int row, col;

    public void Init(int r, int c)
    {
        row = r;
        col = c;
        //button.onClick.AddListener(() => GameManager.Instance.OnCellClicked(col));
    }

    public void SetPiece(int playerId)
    {
        pieceImage.color = (playerId == 1) ? Color.red : Color.yellow;
        button.interactable = false;
    }
}
