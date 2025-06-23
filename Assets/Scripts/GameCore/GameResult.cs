using System.Collections.Generic;
using UnityEngine;

// Handles win, draw, and pattern detection logic for Connect 4
public class GameResult
{
    private readonly int[,] grid;
    private readonly int rows;
    private readonly int cols;

    private List<Vector2Int> winningCells = new List<Vector2Int>(); // Stores the winning cell positions
    public List<Vector2Int> WinningCells => winningCells;

    // Directions to check: vertical, horizontal, and two diagonals
    private static readonly Vector2Int[] directions = new[]
    {
        new Vector2Int(1, 0),   // Vertical
        new Vector2Int(0, 1),   // Horizontal
        new Vector2Int(1, 1),   // Diagonal down-right
        new Vector2Int(1, -1),  // Diagonal down-left
    };

    public GameResult(int[,] grid)
    {
        this.grid = grid;
        rows = grid.GetLength(0);
        cols = grid.GetLength(1);
    }

    // Checks for win or draw condition after a move
    public Result CheckResult(int row, int col, int playerId)
    {
        winningCells.Clear();

        if (CheckWin(row, col, playerId))
            return Result.Win;

        if (IsFull())
            return Result.Draw;

        return Result.Ongoing;
    }

    // Checks if 4 or more connected pieces exist from a given cell
    private bool CheckWin(int row, int col, int playerId)
    {
        foreach (var dir in directions)
        {
            List<Vector2Int> current = new List<Vector2Int> { new Vector2Int(row, col) };

            // Check both directions from the point
            current.AddRange(CollectDirection(row, col, dir.x, dir.y, playerId));
            current.AddRange(CollectDirection(row, col, -dir.x, -dir.y, playerId));

            if (current.Count >= 4)
            {
                winningCells.Clear();
                winningCells.AddRange(current);
                return true;
            }
        }

        return false;
    }

    // Collects same-player pieces in one direction
    private List<Vector2Int> CollectDirection(int row, int col, int rowDir, int colDir, int playerId)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        int r = row + rowDir;
        int c = col + colDir;

        while (IsValid(r, c) && grid[r, c] == playerId)
        {
            result.Add(new Vector2Int(r, c));
            r += rowDir;
            c += colDir;
        }

        return result;
    }

    // Checks if given position is inside the board bounds
    private bool IsValid(int row, int col)
    {
        return row >= 0 && row < rows && col >= 0 && col < cols;
    }

    // Checks if the board is completely filled
    public bool IsFull()
    {
        if (grid == null || cols == 0 || rows == 0)
            return false;

        for (int col = 0; col < cols; col++)
        {
            if (grid[0, col] == 0) // Top row is still empty
                return false;
        }
        return true;
    }

    // Counts open-ended 2-in-a-rows and 3-in-a-rows for a player
    public (int twos, int threes) GetPatternCounts(int playerId)
    {
        int twos = 0;
        int threes = 0;

        foreach (var dir in directions)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (grid[r, c] != playerId) continue;

                    int count = 1;
                    int nr = r + dir.x;
                    int nc = c + dir.y;

                    // Count how many in-a-row in this direction
                    while (IsValid(nr, nc) && grid[nr, nc] == playerId)
                    {
                        count++;
                        nr += dir.x;
                        nc += dir.y;
                    }

                    // Check for open ends (to make it a valid potential pattern)
                    bool openStart = IsValid(r - dir.x, c - dir.y) && grid[r - dir.x, c - dir.y] == 0;
                    bool openEnd = IsValid(nr, nc) && grid[nr, nc] == 0;

                    if (openStart && openEnd)
                    {
                        if (count == 2) twos++;
                        else if (count == 3) threes++;
                    }
                }
            }
        }

        return (twos, threes);
    }
}
