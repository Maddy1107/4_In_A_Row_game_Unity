using System.Collections.Generic;
using UnityEngine;

public class AIController : IPlayerController
{
    public int PlayerId { get; set; }
    public bool IsHuman => false;

    public void PlayTurn()
    {
        var board = GameManager.Instance.board.CellStateGrid;
        int cols = board.GetLength(1);
        List<int> availableCols = new List<int>();

        for (int i = 0; i < cols; i++)
        {
            if (board[0, i] == 0)
                availableCols.Add(i);
        }

        if (availableCols.Count > 0)
        {
            int col = availableCols[Random.Range(0, availableCols.Count)];
            GameManager.Instance.TryMakeMove(col);
        }
    }
}
