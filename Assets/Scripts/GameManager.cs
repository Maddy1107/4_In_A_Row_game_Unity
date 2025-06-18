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
        StartGame();
    }

    void StartGame()
    {
        board.GenerateBoard(rows, cols);
    }
}
