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
        float offsetX = (columns - 1) * gameCellSize / 2f;
        float offsetY = (rows - 1) * gameCellSize / 2f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject cell = Instantiate(cellPrefab, transform);

                cell.name = $"Cell_{row}_{col}";

                cell.transform.localPosition = new Vector3(
                    col * gameCellSize - offsetX,
                    -row * gameCellSize + offsetY,
                    0
                );

                cell.GetComponent<GameCell>().InitializeCell(GameManager.Instance.ListCard.sprites[Random.Range(0, GameManager.Instance.ListCard.sprites.Count)]);
            }
        }
    }
}
