using System.Collections.Generic;
using UnityEngine;

// Provides helper methods for AI move decisions
public static class AIMoveUtility
{
    // Returns list of columns that are not full
    public static List<int> GetAvailableColumns(int[,] board)
    {
        List<int> available = new List<int>();
        for (int c = 0; c < board.GetLength(1); c++)
            if (board[0, c] == 0) // If top cell is empty, column is available
                available.Add(c);
        return available;
    }

    // Gets the lowest available row in a column
    public static int GetRow(int[,] board, int col)
    {
        for (int r = board.GetLength(0) - 1; r >= 0; r--)
            if (board[r, col] == 0) // Look for empty cell from bottom up
                return r;
        return -1; // Column is full
    }

    // Checks if placing a piece in a column would result in a win
    public static bool WouldWin(int[,] board, int col, int player)
    {
        int row = GetRow(board, col);
        if (row < 0) return false; // Can't play here

        var clone = (int[,])board.Clone(); // Clone board to simulate move
        clone[row, col] = player; // Simulate placing the piece

        return new GameResult(clone).CheckResult(row, col, player) == Result.Win; // Check if it's a win
    }

    // Picks a random column from available options
    public static int RandomColumn(List<int> options)
    {
        return options[Random.Range(0, options.Count)];
    }
}
