// Represents a human-controlled player
public class PlayerController : IPlayerController, IHumanInputReceiver
{
    public int PlayerId { get; set; }            // ID of the player (1 or 2)
    public bool IsHuman => true;                 // Always true for this controller

    private int selectedCol = -1;                // Column selected by the player

    // Called by the UI when a player clicks a column
    public void SetColumn(int col)
    {
        selectedCol = col;
        PlayTurn();                              // Immediately attempt move after input
    }

    // Attempts to make a move if a column has been selected
    public void PlayTurn()
    {
        if (selectedCol >= 0)
        {
            int moveCol = selectedCol;
            selectedCol = -1;                    // Reset for next turn
            GameManager.Instance.TryMakeMove(moveCol);
        }
    }
}
