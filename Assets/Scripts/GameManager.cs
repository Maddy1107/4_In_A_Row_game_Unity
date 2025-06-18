using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Board Settings")]
    public int rows = 6;
    public int cols = 7;

    public Board board;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        board.GenerateBoard(rows, cols);
    }

    public void OnCellClicked(int col)
    {
        //if (!currentPlayer.IsHuman) return;
        TryMakeMove(col);
    }

    public void OnCellHover(int col, bool isHovering)
    {
        var cell = board.GetPlaceableCell(col);
        if (cell == null) return;

        if (isHovering) cell.SetHighLightPiece(1);
        else cell.ResetCell();
    }

    private void TryMakeMove(int col)
    {
        var cell = board.GetPlaceableCell(col);
        if (cell == null) return;

        board.PlacePiece(cell.row, col, 1);

        // if (board.CheckWin(cell.row, col, currentPlayerId))
        // {
        //     Debug.Log($"Player {currentPlayerId} Wins!");
        //     return;
        // }

        // if (board.IsFull())
        // {
        //     Debug.Log("Draw!");
        //     return;
        // }

        // currentPlayerId = 3 - currentPlayerId; // Fast toggle between 1 and 2
    }
}
