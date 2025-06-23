using System.Collections.Generic;

// Chooses AI moves based on the selected difficulty level
public class AIMoveSelector
{
    private readonly Difficulty difficulty;

    // Constructor stores difficulty level
    public AIMoveSelector(Difficulty difficulty)
    {
        this.difficulty = difficulty;
    }

    // Selects a column to play based on difficulty
    public int SelectColumn(int[,] board, int playerId)
    {
        List<int> available = AIMoveUtility.GetAvailableColumns(board); // Get all non-full columns
        if (available.Count == 0) return 0; // No valid moves, return 0 as fallback

        return difficulty switch
        {
            Difficulty.Easy => AIMoveUtility.RandomColumn(available), // Pick randomly
            Difficulty.Medium => FindBestColumn(board, available, playerId, blockOnly: true), // Block opponent if needed
            Difficulty.Hard => FindBestColumn(board, available, playerId, blockOnly: false), // Try to win or block
            Difficulty.Impossible => new MinimaxAI(board, playerId, 4).GetBestMove(), // Use minimax for best move
            _ => AIMoveUtility.RandomColumn(available),
        };
    }

    // Finds the best column to block or win depending on difficulty
    private int FindBestColumn(int[,] board, List<int> options, int playerId, bool blockOnly)
    {
        if (!blockOnly)
        {
            // Try to win first
            foreach (int col in options)
                if (AIMoveUtility.WouldWin(board, col, playerId))
                    return col;
        }

        // Then try to block opponent
        foreach (int col in options)
            if (AIMoveUtility.WouldWin(board, col, 3 - playerId))
                return col;

        // Otherwise, just pick a random move
        return AIMoveUtility.RandomColumn(options);
    }
}
