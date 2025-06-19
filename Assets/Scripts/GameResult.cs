using UnityEngine;

public class GameResult
{
    private readonly int[,] grid;
    private readonly int rows;
    private readonly int cols;

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
        if (CheckWin(row, col, playerId))
            return Result.Win;

        return Result.Ongoing;
    }

    private bool CheckWin(int row, int col, int playerId)
    {
        foreach (var dir in directions)
        {
            int count = 1;
            count += CountDirection(row, col, dir.x, dir.y, playerId);
            count += CountDirection(row, col, -dir.x, -dir.y, playerId);

            if (count >= 4)
                return true;
        }

        return false;
    }

    private int CountDirection(int row, int col, int rowDir, int colDir, int playerId)
    {
        int count = 0;
        int r = row + rowDir;
        int c = col + colDir;

        while (IsValid(r, c) && grid[r, c] == playerId)
        {
            count++;
            r += rowDir;
            c += colDir;
        }

        return count;
    }

    private bool IsValid(int row, int col)
    {
        return row >= 0 && row < rows && col >= 0 && col < cols;
    }
}
