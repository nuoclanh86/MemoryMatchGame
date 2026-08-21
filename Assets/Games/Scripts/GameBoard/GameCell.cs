using UnityEngine;
using UnityEngine.UI;

public class GameCell : MonoBehaviour
{
    private Image image;
    private bool isSelected;

    void Start()
    {
        image = this.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found on PlayerToggle GameObject.");
        }
    }

    void Update()
    {

    }

    public void Toggle()
    {
        isSelected = !isSelected;
        image.color = isSelected ? Color.green : Color.white;
    }
}
