using System;
using UnityEngine;
using UnityEngine.UI;

public class GameCell : MonoBehaviour
{
    public int GameCell_ID { get; private set; }

    private Image image;
    private bool isSelected;

    public Action<int> onCellSelected;

    public void Toggle()
    {
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
}