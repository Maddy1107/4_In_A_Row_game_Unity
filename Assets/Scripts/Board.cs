using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform boardParent;

    private int rows, cols;
    public int[,] CellStateGrid { get; private set; }
    private Cell[,] cells;

    public void GenerateBoard(int rows, int cols)
    {
        ClearBoard();

        this.rows = rows;
        this.cols = cols;
        CellStateGrid = new int[rows, cols];
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

    private void ClearBoard()
    {
        foreach (Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }
    }

    public Cell GetPlaceableCell(int col)
    {
        for (int row = rows - 1; row >= 0; row--)
        {
            if (CellStateGrid[row, col] == 0)
                return cells[row, col];
        }
        return null;
    }

    public void PlacePiece(int row, int col, int playerId)
    {
        CellStateGrid[row, col] = playerId;
        cells[row, col].SetPiece(playerId);
    }

    public bool IsFull()
    {
        for (int col = 0; col < cols; col++)
        {
            if (CellStateGrid[0, col] == 0)
                return false;
        }
        return true;
    }
}
