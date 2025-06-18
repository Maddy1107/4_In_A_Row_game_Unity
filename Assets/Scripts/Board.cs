using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform boardParent;
    private int rows, cols;

    public int[,] cellStateGrid { get; private set; }
    private Cell[,] cells;


    public void GenerateBoard(int rows, int cols)
    {
        // Clear previous board
        foreach (Transform child in boardParent)
            Destroy(child.gameObject);

        this.rows = rows;
        this.cols = cols;
        cellStateGrid = new int[rows, cols];
        cells = new Cell[rows, cols];

        var layout = boardParent.GetComponent<GridLayoutGroup>();
        var parentRect = boardParent.GetComponent<RectTransform>();

        float cellSize = Mathf.Min(parentRect.rect.width / cols, parentRect.rect.height / rows);

        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = cols;
        layout.cellSize = new Vector2(cellSize, cellSize);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var newCell = Instantiate(cellPrefab, boardParent);
                var cell = newCell.GetComponent<Cell>();
                cell.Init(r, c);
                cells[r, c] = cell;
            }
        }
    }

    public Vector2Int? PlacePiece(int col, int playerId)
    {
        for (int row = rows - 1; row >= 0; row--)
        {
            if (cellStateGrid[row, col] == 0)
            {
                cellStateGrid[row, col] = playerId;
                cells[row, col].SetPiece(playerId);
                return new Vector2Int(row, col);
            }
        }
        return null;
    }

    public bool IsFull()
    {
        for (int col = 0; col < cols; col++)
            if (cellStateGrid[0, col] == 0)
                return false;
        return true;
    }
}
