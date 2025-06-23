// Interface representing a player (human or AI) in the game
public interface IPlayerController
{
    int PlayerId { get; set; }      // Player's unique ID (1 or 2)
    bool IsHuman { get; }           // Indicates if the player is human

    void PlayTurn();                // Called when it's this player's turn
}
