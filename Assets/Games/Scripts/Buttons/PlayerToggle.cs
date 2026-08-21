using UnityEngine;
using UnityEngine.UI;

public class PlayerToggle : MonoBehaviour
{
    private Image image;

    private bool isSelected;

    private void Awake()
    {
        image = this.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found on PlayerToggle GameObject.");
        }
    }

    public void Toggle()
    {
        isSelected = !isSelected;
        image.color = isSelected ? Color.blue : Color.white;
    }

    public void SetSprite(Sprite newSprite)
    {
        if (image != null)
        {
            image.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Image component not found on PlayerToggle GameObject.");
        }
    }
}
