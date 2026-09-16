using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameBoard : MonoBehaviour
{
    #region GameBoard Configuration
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private PopupPlayerWon popupPlayerWon;
    [SerializeField] private List<GameObject> listPlayerPositions;

    private int _padding = 5;
    private int _cellSize = -1;
    private float _cellSizeScaleRate = 2.5f;
    #endregion

    #region GameBoard Lifecycle

    private const int NONE_CELL_ID = -1;
    private bool lockChoiceCells = false;
    private int lastChosenCellID = NONE_CELL_ID;
    List<GameCell> gameCells = new List<GameCell>();

    public bool LockChoiceCells => lockChoiceCells;

    #endregion

    void Start()
    {
        _cellSize = (int)(cellPrefab.GetComponent<RectTransform>().rect.width * _cellSizeScaleRate);

        InitGameBoard(2, 2);
    }

    private void InitGameBoard(int rows, int columns)
    {
        Debug.Log($"[GameBoard] Creating game board with {rows} rows and {columns} columns.");
        CreateGameBoard(rows, columns, _cellSize + _padding);

        popupPlayerWon.gameObject.SetActive(false);
        if (listPlayerPositions.Count > 0)
        {
            foreach (var playerPos in listPlayerPositions)
            {
                playerPos.SetActive(false);
            }
        }
    }

    private void CreateGameBoard(int rows, int columns, float gameCellSize)
    {
        int totalCells = rows * columns;
        int pairCount = totalCells / 2;

        if (GameManager.Instance.ListCard.sprites.Count < pairCount)
        {
            Debug.LogError(
                $"Not enough sprites. Required: {pairCount}, " +
                $"Available: {GameManager.Instance.ListCard.sprites.Count}");
            return;
        }

        float offsetX = (columns - 1) * gameCellSize / 2f;
        float offsetY = (rows - 1) * gameCellSize / 2f;

        // --------------------------------------------------
        // 1. Create all positions
        // --------------------------------------------------

        List<Vector3> positions = new List<Vector3>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                positions.Add(new Vector3(
                    col * gameCellSize - offsetX,
                    -row * gameCellSize + offsetY,
                    0
                ));
            }
        }

        // Randomize positions
        Shuffle(positions);

        // --------------------------------------------------
        // 2. Create ID pairs
        // --------------------------------------------------

        List<int> ids = new List<int>();

        for (int id = 0; id < pairCount; id++)
        {
            ids.Add(id);
            ids.Add(id);
        }

        // Randomize IDs
        Shuffle(ids);

        // --------------------------------------------------
        // 3. Randomize sprites
        //    Pick different sprite for each ID
        // --------------------------------------------------

        List<Sprite> sprites = new List<Sprite>(GameManager.Instance.ListCard.sprites);

        // Shuffle sprites
        Shuffle(sprites);

        // --------------------------------------------------
        // 4. Create cells
        // --------------------------------------------------

        for (int i = 0; i < totalCells; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = $"Cell_{i}";
            cell.transform.localPosition = positions[i];
            cell.transform.localScale = Vector3.one * _cellSizeScaleRate;
            int cellID = ids[i];
            Sprite sprite = sprites[cellID];
            GameCell gameCell = cell.GetComponent<GameCell>();
            gameCell.InitializeCell(cellID, sprite, OnCellSelected);
            gameCells.Add(gameCell);
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void OnCellSelected(int cellID)
    {
        if (lastChosenCellID == NONE_CELL_ID)
        {
            lastChosenCellID = cellID;
            Debug.Log($"[GameBoard] First cell selected: {cellID}");
        }
        else
        {
            Debug.Log($"[GameBoard] Second cell selected: {cellID}");
            if (lastChosenCellID == cellID)
            {
                Debug.Log("[GameBoard] Match found!");
                // Delete or disable matched cells
                RemoveCells(lastChosenCellID);

                // Check if all cells are matched
                if (gameCells.Count == 0)
                {
                    Debug.Log("[GameBoard] All cells matched. Player won!");
                    // Show player won popup
                    if (popupPlayerWon != null)
                    {
                        popupPlayerWon.gameObject.SetActive(true);
                        popupPlayerWon.Initialize();
                    }
                }
            }
            else
            {
                Debug.Log("[GameBoard] No match.");
                ResetWrongChosenCells(lastChosenCellID, cellID);
            }
            lastChosenCellID = NONE_CELL_ID; // Reset for next selection
        }
    }

    private void RemoveCells(int cellID)
    {
        int count = 2; // only have 2 cells with the same ID, so we can remove them both
        for (int i = gameCells.Count - 1; i >= 0; i--)
        {
            if (gameCells[i].GameCell_ID == cellID)
            {
                GameCell cell = gameCells[i];
                gameCells.RemoveAt(i);
                Destroy(cell.gameObject);
                count--;
            }
            if (count <= 0) break;
        }
    }

    private async void ResetWrongChosenCells(int cellID_01, int cellID_02)
    {
        lockChoiceCells = true;
        await UniTask.Delay(1000);
        ResetWrongChosenCell(cellID_01);
        ResetWrongChosenCell(cellID_02);
        lockChoiceCells = false;
    }

    private void ResetWrongChosenCell(int cellID)
    {
        int count = 2; // only have 2 cells with the same ID

        for (int i = 0; i < gameCells.Count; i++)
        {
            if (gameCells[i].GameCell_ID == cellID)
            {
                gameCells[i].ResetState();
                count--;
            }

            if (count <= 0)
                break;
        }
    }
}
