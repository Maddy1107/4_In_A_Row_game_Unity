using System.Collections.Generic;
using UnityEngine;

public static class AIMoveUtility
{
    public static List<int> GetAvailableColumns(int[,] board)
    {
        List<int> available = new List<int>();
        for (int c = 0; c < board.GetLength(1); c++)
            if (board[0, c] == 0)
                available.Add(c);
        return available;
    }

    public static int GetRow(int[,] board, int col)
    {
        for (int r = board.GetLength(0) - 1; r >= 0; r--)
            if (board[r, col] == 0)
                return r;
        return -1;
    }

    public static bool WouldWin(int[,] board, int col, int player)
    {
        int row = GetRow(board, col);
        if (row < 0) return false;

        var clone = (int[,])board.Clone();
        clone[row, col] = player;

        return new GameResult(clone).CheckResult(row, col, player) == Result.Win;
    }

    public static int RandomColumn(List<int> options)
    {
        return options[Random.Range(0, options.Count)];
    }
}
