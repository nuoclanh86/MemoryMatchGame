using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerItem : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    private int _playerIndex;
    private LobbyController _lobbyController;

    private bool isSelected;

    public int PlayerIndex
    {
        get { return _playerIndex; }
        set { _playerIndex = value; }
    }

    private void Awake()
    {
        image = this.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found on PlayerToggle GameObject.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        image.color = isSelected ? Color.blue : Color.white;

        if (_lobbyController != null)
        {
            _lobbyController.OnPlayerChoiceItemClicked(this);
        }
    }

    private void SetSprite(Sprite newSprite)
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

    public void Initialize(int playerIndex, Sprite sprite, LobbyController lobbyController)
    {
        PlayerIndex = playerIndex;
        SetSprite(sprite);
        _lobbyController = lobbyController;
    }
}
