using System.Collections.Generic;
using UnityEngine;

public class AIMoveSelector
{
    private readonly Difficulty difficulty;

    public AIMoveSelector(Difficulty difficulty)
    {
        this.difficulty = difficulty;
    }

    public int SelectColumn(int[,] board, int playerId)
    {
        List<int> available = AIMoveUtility.GetAvailableColumns(board);
        if (available.Count == 0) return 0;

        return difficulty switch
        {
            Difficulty.Easy => AIMoveUtility.RandomColumn(available),
            Difficulty.Medium => FindBestColumn(board, available, playerId, blockOnly: true),
            Difficulty.Hard => FindBestColumn(board, available, playerId, blockOnly: false),
            Difficulty.Impossible => AIMoveUtility.RandomColumn(available), // Hook in Minimax later
            _ => AIMoveUtility.RandomColumn(available),
        };
    }

    private int FindBestColumn(int[,] board, List<int> options, int playerId, bool blockOnly)
    {
        if (!blockOnly)
        {
            foreach (int col in options)
                if (AIMoveUtility.WouldWin(board, col, playerId))
                    return col;
        }

        foreach (int col in options)
            if (AIMoveUtility.WouldWin(board, col, 3 - playerId))
                return col;

        return AIMoveUtility.RandomColumn(options);
    }
}
