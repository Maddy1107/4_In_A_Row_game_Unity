using System.Collections.Generic;
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
        var boardRect = boardParent.GetComponent<RectTransform>();

        int padding = 30;
        layout.padding = new RectOffset(padding, padding, padding, padding);

        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = cols;

        float availableWidth = boardRect.rect.width - (padding * 2);
        float availableHeight = boardRect.rect.height - (padding * 2);

        float maxCellWidth = availableWidth / (cols + 0.1f * (cols - 1));
        float maxCellHeight = availableHeight / (rows + 0.1f * (rows - 1));
        float cellSize = Mathf.Min(maxCellWidth, maxCellHeight);

        float totalCellWidth = cellSize * cols;
        float spacingX = (availableWidth - totalCellWidth) / (cols - 1);

        float totalCellHeight = cellSize * rows;
        float spacingY = (availableHeight - totalCellHeight) / (rows - 1);

        layout.cellSize = new Vector2(cellSize, cellSize);
        layout.spacing = new Vector2(spacingX, spacingY);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var cellObj = Instantiate(cellPrefab, boardParent);
                var cell = cellObj.GetComponent<Cell>();
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
    public void GlowWinningCells(List<Vector2Int> winCells)
    {
        foreach (var pos in winCells)
        {
            cells[pos.x, pos.y].StartGlow();
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
        AudioManager.Instance?.PlaySFX(UISFX.Piecefall);
    }
}
