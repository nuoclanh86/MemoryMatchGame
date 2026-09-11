using System;
using UnityEngine;
using UnityEngine.UI;

public class GameCell : MonoBehaviour
{
    public int GameCell_ID { get; private set; }

    private Image image;
    private bool isSelected;
    private GameBoard gameBoard;

    public Action<int> onCellSelected;

    private void Start()
    {
        gameBoard = GetComponentInParent<GameBoard>();
        if (gameBoard == null)
        {
            Debug.LogError("[GameCell] GameBoard not found in the scene.");
        }
    }

    public void Toggle()
    {
        if (gameBoard.LockChoiceCells)
        {
            Debug.Log("[GameCell] Cell selection is locked. Ignoring selection.");
            return;
        }

        isSelected = !isSelected;
        image.color = isSelected ? Color.green : Color.white;

        onCellSelected?.Invoke(GameCell_ID);
    }

    public void InitializeCell(int id, Sprite sprite, Action<int> onCellSelected)
    {
        GameCell_ID = id;
        this.onCellSelected = onCellSelected;

        if (image == null)
            image = this.GetComponent<Image>();

        if (image != null)
            image.sprite = sprite;
        else
            Debug.LogError("[GameCell] Image component not found on GameCell : " + this.name);
    }

    public void ResetState()
    {
        isSelected = false;
        image.color = Color.white;
    }
}