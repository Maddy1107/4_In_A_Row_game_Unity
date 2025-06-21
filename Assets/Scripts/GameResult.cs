using System.Collections.Generic;
using UnityEngine;

public class GameResult
{
    private readonly int[,] grid;
    private readonly int rows;
    private readonly int cols;

    private List<Vector2Int> winningCells = new List<Vector2Int>();
    public List<Vector2Int> WinningCells => winningCells;

    private static readonly Vector2Int[] directions = new[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
    };

    public GameResult(int[,] grid)
    {
        this.grid = grid;
        rows = grid.GetLength(0);
        cols = grid.GetLength(1);
    }

    public Result CheckResult(int row, int col, int playerId)
    {
        winningCells.Clear();

        if (CheckWin(row, col, playerId))
            return Result.Win;

        if (IsFull())
            return Result.Draw;

        return Result.Ongoing;
    }

    private bool CheckWin(int row, int col, int playerId)
    {
        foreach (var dir in directions)
        {
            List<Vector2Int> current = new List<Vector2Int> { new Vector2Int(row, col) };

            current.AddRange(CollectDirection(row, col, dir.x, dir.y, playerId));
            current.AddRange(CollectDirection(row, col, -dir.x, -dir.y, playerId));

            if (current.Count >= 4)
            {
                winningCells = current;
                return true;
            }
        }

        return false;
    }

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

    private bool IsValid(int row, int col)
    {
        return row >= 0 && row < rows && col >= 0 && col < cols;
    }

    public bool IsFull()
    {
        if (grid == null || cols == 0 || rows == 0)
            return false;

        for (int col = 0; col < cols; col++)
        {
            if (grid[0, col] == 0)
                return false;
        }
        return true;
    }
}
