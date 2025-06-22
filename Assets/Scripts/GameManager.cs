using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Board Settings")]
    public int rows = 6;
    public int cols = 7;
    public Board board;

    [Header("Player Settings")]
    private IPlayerController player1;
    private IPlayerController player2;
    private IPlayerController currentPlayer;

    public IPlayerController CurrentPlayer => currentPlayer;

    private GameResult resultChecker;

    public TMP_Text turnText;

    // Game Events
    public event Action<IPlayerController, Result> OnGameOver;
    public event Action OnBack;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void StartGame()
    {
        board.GenerateBoard(rows, cols);
        resultChecker = new GameResult(board.CellStateGrid);

        currentPlayer = player1;
        ChangeTurnText();

        currentPlayer.PlayTurn();
    }

    public void SelectMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Local:
                CreatePlayer(PlayerType.Human, PlayerType.Human);
                break;
            case GameMode.AI:
                CreatePlayer(PlayerType.Human, PlayerType.AI);
                break;
            default:
                Debug.LogError("Unsupported game mode selected.");
                return;
        }
    }

    public void CreatePlayer(PlayerType p1Type, PlayerType p2Type)
    {
        player1 = PlayerFactory.CreatePlayer(p1Type, 1);
        player2 = PlayerFactory.CreatePlayer(p2Type, 2);
        StartGame();
    }

    public void OnCellClicked(int col)
    {
        if (currentPlayer is IHumanInputReceiver human)
        {
            human.SetColumn(col);
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

        Result result = resultChecker.CheckResult(cell.row, col, currentPlayer.PlayerId);

        if (result != Result.Ongoing)
        {
            if (result == Result.Draw)
            {
                OnGameOver?.Invoke(null, result);
            }

            var sfx = currentPlayer.IsHuman ? UISFX.Win : UISFX.Lose;
            AudioManager.Instance?.PlaySFX(sfx);

            List<Vector2Int> winCells = resultChecker.WinningCells;
            board.GlowWinningCells(winCells);

            StartCoroutine(ResultDelay(currentPlayer, result));
        }
        else
        {
            ChangeTurn();
        }
    }
    private IEnumerator ResultDelay(IPlayerController winner, Result result)
    {
        yield return new WaitForSeconds(1.5f);

        OnGameOver?.Invoke(winner, result);
    }

    private void ChangeTurn()
    {
        currentPlayer = (currentPlayer == player1) ? player2 : player1;
        ChangeTurnText();

        Debug.Log($"Switched to Player {currentPlayer.PlayerId}");
        currentPlayer.PlayTurn();
    }

    public void ChangeTurnText()
    {
        if (turnText == null)
        {
            Debug.LogWarning("Turn text is not assigned.");
            return;
        }

        turnText.text = currentPlayer.IsHuman
            ? $"Player {currentPlayer.PlayerId}'s Turn"
            : $"AI is Thinking...";
    }

    public void RestartGame()
    {
        board.GenerateBoard(rows, cols);
        resultChecker = new GameResult(board.CellStateGrid);
        StartGame();
    }

    public void OnBackPressed()
    {
        OnBack?.Invoke();
    }
}
