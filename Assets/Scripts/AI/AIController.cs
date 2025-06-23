using System.Collections;
using UnityEngine;

// Controls AI player behavior
public class AIController : IPlayerController
{
    // ID of the AI player
    public int PlayerId { get; set; }

    // AI is not a human player
    public bool IsHuman => false;

    // Selects AI moves based on difficulty
    private readonly AIMoveSelector moveSelector;

    // Constructor initializes AI with difficulty
    public AIController(Difficulty difficulty)
    {
        moveSelector = new AIMoveSelector(difficulty);
        Debug.Log($"AI initialized with difficulty: {difficulty}");
    }

    // Called when it's the AI's turn to play
    public void PlayTurn()
    {
        GameManager.Instance.StartCoroutine(DelayedAIMove());
    }

    // Waits a moment and then makes the AI move
    private IEnumerator DelayedAIMove()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f)); // Add delay for realism
        int col = moveSelector.SelectColumn(GameManager.Instance.board.CellStateGrid, PlayerId); // Pick a column
        GameManager.Instance.TryMakeMove(col); // Attempt move in selected column
    }
}
