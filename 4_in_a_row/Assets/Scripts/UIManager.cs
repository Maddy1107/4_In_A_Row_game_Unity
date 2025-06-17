using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles board visuals and column click input
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        columnButtons = new List<Button>();
    }

    public GameObject cellPrefab;
    public RectTransform gridParent;
    public GameObject columnButtonPrefab;
    public Transform columnButtonParent;

    private List<Button> columnButtons;
    private GameObject[,] gridCells;

    public void GenerateGrid(int rows, int cols)
    {
        // Clear existing cells
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        gridCells = new GameObject[rows, cols];

        GridLayoutGroup layout = gridParent.GetComponent<GridLayoutGroup>();
        RectTransform parentRect = gridParent.GetComponent<RectTransform>();

        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        // Calculate square size
        float maxCellSize = Mathf.Min(parentWidth / cols, parentHeight / rows);

        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = cols;
        layout.cellSize = new Vector2(maxCellSize, maxCellSize);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GameObject cell = Instantiate(cellPrefab, gridParent);
                gridCells[r, c] = cell;
            }
        }
    }

    public void EnableColumnButtons(Action<int> onClick)
    {
        for (int i = 0; i < columnButtons.Count; i++)
        {
            int col = i;
            var btn = columnButtons[i];
            btn.interactable = true;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => onClick(col));
        }
    }

    public void DisableColumnButtons()
    {
        foreach (var btn in columnButtons)
        {
            btn.interactable = false;
            btn.onClick.RemoveAllListeners();
        }
    }

    public void SetCellColor(int row, int col, Color color)
    {
        gridCells[row, col].GetComponent<Image>().color = color;
    }
}
