using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Board Settings")]
    public int rows = 6;
    public int cols = 7;
    public Board board;

    [Header("Player Settings")]
    public PlayerType player1Type = PlayerType.Human;
    public PlayerType player2Type = PlayerType.AI;

    private IPlayerController player1;
    private IPlayerController player2;
    private IPlayerController currentPlayer;

    public IPlayerController CurrentPlayer => currentPlayer;

    private GameResult resultChecker;

    // Game Events
    public event Action<int> OnWin;
    public event Action OnDraw;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        board.GenerateBoard(rows, cols);
        resultChecker = new GameResult(board.CellStateGrid);

        player1 = PlayerFactory.CreatePlayer(player1Type, 1);
        player2 = PlayerFactory.CreatePlayer(player2Type, 2);

        currentPlayer = player1;
        currentPlayer.PlayTurn(); // Let first player (AI/human) start
    }

    public void OnCellClicked(int col)
    {
        if (currentPlayer is IHumanInputReceiver human)
        {
            human.SetColumn(col); // Human decides input
        }
        else
        {
            Debug.LogWarning("Current player is AI. Ignoring click.");
        }
    }

    public void OnCellHover(int col, bool isHovering)
    {
        if (currentPlayer is IHumanInputReceiver)
        {
            var cell = board.GetPlaceableCell(col);
            if (cell == null) return;

            if (isHovering)
                cell.SetHighLightPiece(currentPlayer.PlayerId);
            else
                cell.ResetCell();
        }
    }

    public void TryMakeMove(int col)
    {
        var cell = board.GetPlaceableCell(col);
        if (cell == null) return;

        board.PlacePiece(cell.row, col, currentPlayer.PlayerId);

        var result = resultChecker.CheckResult(cell.row, col, currentPlayer.PlayerId);

        if (result == Result.Win)
        {
            Debug.Log($"🎉 Player {currentPlayer.PlayerId} Wins!");
            OnWin?.Invoke(currentPlayer.PlayerId);
            return;
        }

        if (board.IsFull())
        {
            Debug.Log("🤝 It's a Draw!");
            OnDraw?.Invoke();
            return;
        }

        ChangeTurn();
    }

    private void ChangeTurn()
    {
        currentPlayer = (currentPlayer == player1) ? player2 : player1;
        Debug.Log($"🔄 Switched to Player {currentPlayer.PlayerId}");
        currentPlayer.PlayTurn(); // Let AI or wait for human
    }
}
