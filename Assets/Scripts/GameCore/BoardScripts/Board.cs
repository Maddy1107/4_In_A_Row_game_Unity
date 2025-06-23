using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages the game board UI and cell state logic
public class Board : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private GameObject cellPrefab;      // Prefab for a single cell
    [SerializeField] private Transform boardParent;      // Parent container for grid layout

    private int rows, cols;
    public int[,] CellStateGrid { get; private set; }     // Logical state of the board (0 = empty, 1/2 = players)
    private Cell[,] cells;                                // References to all cell components

    // Generates a new board with given dimensions
    public void GenerateBoard(int rows, int cols)
    {
        ClearBoard(); // Remove any existing cells

        this.rows = rows;
        this.cols = cols;
        CellStateGrid = new int[rows, cols]; // Initialize logical grid
        cells = new Cell[rows, cols];        // Initialize visual grid

        var layout = boardParent.GetComponent<GridLayoutGroup>();
        var boardRect = boardParent.GetComponent<RectTransform>();

        // Set padding and column constraint
        int padding = 30;
        layout.padding = new RectOffset(padding, padding, padding, padding);
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = cols;

        // Calculate best-fitting cell size and spacing
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

        // Instantiate all cells and initialize them
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var cellObj = Instantiate(cellPrefab, boardParent);
                var cell = cellObj.GetComponent<Cell>();
                cell.Init(r, c); // Assign row and column
                cells[r, c] = cell;
            }
        }
    }

    // Clears all cell objects from the board
    private void ClearBoard()
    {
        foreach (Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }
    }

    // Highlights winning cells with a glow effect
    public void GlowWinningCells(List<Vector2Int> winCells)
    {
        foreach (var pos in winCells)
        {
            cells[pos.x, pos.y].StartGlow();
        }
    }

    // Returns the lowest empty cell in the given column
    public Cell GetPlaceableCell(int col)
    {
        for (int row = rows - 1; row >= 0; row--)
        {
            if (CellStateGrid[row, col] == 0)
                return cells[row, col];
        }
        return null; // Column is full
    }

    // Updates both logical and visual board with a placed piece
    public void PlacePiece(int row, int col, int playerId)
    {
        CellStateGrid[row, col] = playerId;          // Update logic
        cells[row, col].SetPiece(playerId);          // Update visuals
        AudioManager.Instance?.PlaySFX(UISFX.Piecefall); // Play piece drop sound
    }
}
