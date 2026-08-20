using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    int rows = 4;
    int columns = 4;
    int size = 150;

    void Start()
    {
        CreateGameBoard();
    }

    private void CreateGameBoard()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject cell = Instantiate(cellPrefab, transform);
                cell.name = $"Cell_{row}_{col}";
                // Position the cell based on its row and column
                cell.transform.localPosition = new Vector3(col * size, -row * size, 0);
            }
        }
    }
}
