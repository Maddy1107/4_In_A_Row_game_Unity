using UnityEngine;

// Manages game flow, turns, and win/draw detection.
public class GameManager : MonoBehaviour
{
    public int rows = 6;
    public int columns = 7;

    private Board board;
    //private IPlayerController[] players;
    private int currentPlayerIndex = 0;

    private void Start()
    {
        board = new Board(rows, columns);
        UIManager.Instance.GenerateGrid(rows, columns);

        //Initialize players: 0 = Human, 1 = AI
        // players = new IPlayerController[]
        // {
        //     new PlayerController(UIManager.Instance), // Player 1 (Human)
        //     new AIController()                   // Player 2 (AI)
        // };

        //StartTurn();
    }

    // private void StartTurn()
    // {
    //     players[currentPlayerIndex].MakeMove(board, OnMoveChosen);
    // }

    // private void OnMoveChosen(int column)
    // {
    //     if (!board.PlacePiece(column, currentPlayerIndex + 1, out int row))
    //         return; // Invalid move (column full)

    //     // Update UI
    //     Color color = currentPlayerIndex == 0 ? Color.red : Color.yellow;
    //     UIManager.Instance.SetCellColor(row, column, color);

    //     // Check for win
    //     if (board.CheckWin(row, column, currentPlayerIndex + 1))
    //     {
    //         Debug.Log($"Player {currentPlayerIndex + 1} wins!");
    //         UIManager.Instance.DisableColumnButtons();
    //         return;
    //     }

    //     // Check for draw
    //     if (board.IsFull())
    //     {
    //         Debug.Log("Draw!");
    //         UIManager.Instance.DisableColumnButtons();
    //         return;
    //     }

    //     // Next turn
    //     currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
    //     StartTurn();
    // }
}
