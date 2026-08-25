using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    private int _padding = 5;
    private int _cellSize = -1;

    void Start()
    {
        int _rows = GameManager.Instance.PlayerCount * 3;
        int _columns = GameManager.Instance.PlayerCount * 4;

        _cellSize = (int)cellPrefab.GetComponent<RectTransform>().rect.width;
        CreateGameBoard(_rows, _columns, _cellSize + _padding);
    }

    private void CreateGameBoard(int rows, int columns, float gameCellSize)
    {
        int totalCells = rows * columns;

        if (totalCells % 2 != 0)
        {
            Debug.LogError("Total number of cells must be even.");
            return;
        }

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

        List<Sprite> sprites = new List<Sprite>(
            GameManager.Instance.ListCard.sprites
        );

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

            int cellID = ids[i];

            Sprite sprite = sprites[cellID];

            GameCell gameCell = cell.GetComponent<GameCell>();

            gameCell.InitializeCell(cellID, sprite);
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
}
