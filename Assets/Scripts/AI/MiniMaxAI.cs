using System.Collections.Generic;
using UnityEngine;

// Minimax AI logic for selecting the best move in Connect 4
public class MinimaxAI
{
    private readonly int[,] board;
    private readonly int playerId;
    private readonly int opponentId;
    private readonly int maxDepth;

    public MinimaxAI(int[,] board, int playerId, int maxDepth)
    {
        this.board = board;
        this.playerId = playerId;
        this.opponentId = 3 - playerId;
        this.maxDepth = maxDepth;
    }

    // Entry point to get the best column to play using minimax
    public int GetBestMove()
    {
        List<int> availableCols = AIMoveUtility.GetAvailableColumns(board);
        int bestScore = int.MinValue;
        int bestCol = availableCols[0];

        foreach (int col in availableCols)
        {
            int row = AIMoveUtility.GetRow(board, col);
            if (row < 0) continue;

            int[,] clone = (int[,])board.Clone();
            clone[row, col] = playerId;

            int score = Minimax(clone, maxDepth - 1, false, row, col, playerId);

            if (score > bestScore)
            {
                bestScore = score;
                bestCol = col;
            }
        }

        return bestCol;
    }

    // Recursive minimax algorithm to evaluate best move
    private int Minimax(int[,] state, int depth, bool isMaximizing, int lastRow, int lastCol, int lastPlayer)
    {
        if (new GameResult(state).CheckResult(lastRow, lastCol, lastPlayer) == Result.Win)
        {
            return lastPlayer == playerId ? 1000 : -1000; // Win = high score
        }

        if (depth == 0 || AIMoveUtility.GetAvailableColumns(state).Count == 0)
        {
            return EvaluateBoard(state); // Terminal node
        }

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        int currentPlayer = isMaximizing ? playerId : opponentId;

        foreach (int col in AIMoveUtility.GetAvailableColumns(state))
        {
            int row = AIMoveUtility.GetRow(state, col);
            if (row < 0) continue;

            int[,] clone = (int[,])state.Clone();
            clone[row, col] = currentPlayer;

            int score = Minimax(clone, depth - 1, !isMaximizing, row, col, currentPlayer);

            if (isMaximizing)
                bestScore = Mathf.Max(bestScore, score);
            else
                bestScore = Mathf.Min(bestScore, score);
        }

        return bestScore;
    }

    // Board evaluation for non-terminal nodes using patterns and center control
    private int EvaluateBoard(int[,] state)
    {
        var result = new GameResult(state);

        var (myTwos, myThrees) = result.GetPatternCounts(playerId);
        var (opTwos, opThrees) = result.GetPatternCounts(opponentId);

        int myScore = myTwos * 10 + myThrees * 50;
        int opScore = opTwos * 10 + opThrees * 50;

        // Center preference scoring (more weight to center columns)
        int centerCol = state.GetLength(1) / 2;
        int centerScore = 0;

        for (int r = 0; r < state.GetLength(0); r++)
        {
            for (int c = 0; c < state.GetLength(1); c++)
            {
                if (state[r, c] == playerId)
                {
                    int dist = Mathf.Abs(centerCol - c);
                    centerScore += state.GetLength(1) - dist;
                }
                else if (state[r, c] == opponentId)
                {
                    int dist = Mathf.Abs(centerCol - c);
                    centerScore -= (state.GetLength(1) - dist) / 2;
                }
            }
        }

        return myScore - opScore + centerScore;
    }
}
