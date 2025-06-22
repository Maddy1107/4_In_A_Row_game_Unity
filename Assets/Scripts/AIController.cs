using System.Collections;
using UnityEngine;

public class AIController : IPlayerController
{
    public int PlayerId { get; set; }
    public bool IsHuman => false;

    private readonly AIMoveSelector moveSelector;

    public AIController(Difficulty difficulty)
    {
        moveSelector = new AIMoveSelector(difficulty);
        Debug.Log($"AI initialized with difficulty: {difficulty}");
    }

    public void PlayTurn()
    {
        GameManager.Instance.StartCoroutine(DelayedAIMove());
    }

    private IEnumerator DelayedAIMove()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        int col = moveSelector.SelectColumn(GameManager.Instance.board.CellStateGrid, PlayerId);
        GameManager.Instance.TryMakeMove(col);
    }
}
