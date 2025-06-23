using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;
using System.Collections;

// Manages overall game flow and logic
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Board Settings")]
    public int rows = 6;
    public int cols = 7;
    public Board board; // Reference to the board component

    [Header("Player Settings")]
    private IPlayerController player1;           // Player 1 controller
    private IPlayerController player2;           // Player 2 controller
    private IPlayerController currentPlayer;     // Whose turn is it now

    public IPlayerController CurrentPlayer => currentPlayer; // Read-only accessor

    private GameResult resultChecker; // Result checker instance

    public TMP_Text turnText;         // UI text for turn display
    public ParticleSystem Particles;  // Win celebration effect

    // Game Events
    public event Action<IPlayerController, Result> OnGameOver; // Fires on game end
    public event Action OnBack;                                 // Fires when back is pressed

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; // Singleton assignment
    }

    // Starts the game after players are created
    private void StartGame()
    {
        board.GenerateBoard(rows, cols); // Build the board
        resultChecker = new GameResult(board.CellStateGrid); // Initialize result checker

        currentPlayer = player1;
        ChangeTurnText(); // Update UI

        currentPlayer.PlayTurn(); // Let first player take turn
    }

    // Called by UI to select a game mode
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

    // Instantiates both players and starts the game
    public void CreatePlayer(PlayerType p1Type, PlayerType p2Type)
    {
        player1 = PlayerFactory.CreatePlayer(p1Type, 1);
        player2 = PlayerFactory.CreatePlayer(p2Type, 2);
        StartGame();
    }

    // Called when a cell is clicked (by human)
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

    // Handles highlighting when hovering over a column
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

    // Attempts to place a piece in the specified column
    public void TryMakeMove(int col)
    {
        var cell = board.GetPlaceableCell(col);
        if (cell == null) return;

        board.PlacePiece(cell.row, col, currentPlayer.PlayerId);

        if (!HasGameEnded(cell.row, col))
            ChangeTurn(); // Continue game if not ended
    }

    // Checks if the current move ended the game
    public bool HasGameEnded(int row, int col)
    {
        Result result = resultChecker.CheckResult(row, col, currentPlayer.PlayerId);

        if (result != Result.Ongoing)
        {
            if (result == Result.Draw)
            {
                OnGameOver?.Invoke(null, result);
            }

            if (currentPlayer.IsHuman)
                PlayParticles(); // Trigger win particles if human

            var sfx = currentPlayer.IsHuman ? UISFX.Win : UISFX.Lose;
            AudioManager.Instance?.PlaySFX(sfx); // Play end-game sound

            List<Vector2Int> winCells = resultChecker.WinningCells;
            board.GlowWinningCells(winCells); // Highlight winning cells

            StartCoroutine(ResultDelay(currentPlayer, result)); // Delay before game over callback

            return true;
        }
        return false;
    }

    // Waits briefly before triggering game over event
    private IEnumerator ResultDelay(IPlayerController winner, Result result)
    {
        yield return new WaitForSeconds(1.5f);
        OnGameOver?.Invoke(winner, result);
    }

    // Plays the win particle effect
    private void PlayParticles()
    {
        if (Particles != null)
        {
            Particles.Play();
        }
    }

    // Switches turn to the other player
    private void ChangeTurn()
    {
        currentPlayer = (currentPlayer == player1) ? player2 : player1;
        ChangeTurnText();

        Debug.Log($"Switched to Player {currentPlayer.PlayerId}");
        currentPlayer.PlayTurn(); // Let the new player act
    }

    // Updates the turn UI text based on current player
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

    // Restarts the game with same players/settings
    public void RestartGame()
    {
        board.GenerateBoard(rows, cols);
        resultChecker = new GameResult(board.CellStateGrid);
        StartGame();
    }

    // Invoked when back is pressed in UI
    public void OnBackPressed()
    {
        OnBack?.Invoke();
    }
}
