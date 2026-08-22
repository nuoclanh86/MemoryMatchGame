using UnityEngine;
using UnityEngine.UI;

public class GameCell : MonoBehaviour
{
    private Image image;
    private bool isSelected;

    public void Toggle()
    {
        isSelected = !isSelected;
        image.color = isSelected ? Color.green : Color.white;
    }

    public void InitializeCell(Sprite sprite)
    {
        if (image == null)
            image = this.GetComponent<Image>();

        if (image != null)
            image.sprite = sprite;
        else
            Debug.LogError("[GameCell] Image component not found on GameCell : " + this.name);
    }
}
