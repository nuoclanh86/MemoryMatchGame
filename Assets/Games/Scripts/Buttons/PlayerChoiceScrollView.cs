using UnityEngine;

public class PlayerChoiceScrollView : MonoBehaviour
{
    [SerializeField] private GameObject playerTogglePrefab;
    [SerializeField] private Transform contentPanel;

    private ListCardScriptableObject _listCard;

    public void CreatePlayerChoices(LobbyController lobbyController)
    {
        if (_listCard == null)
        {
            _listCard = GameManager.Instance.ListCard;
        }
        
        if (_listCard == null || _listCard.sprites == null || _listCard.sprites.Count == 0)
        {
            Debug.LogError("ListCardScriptableObject is not assigned or contains no sprites.");
            return;
        }
        for (int i = 0; i < _listCard.sprites.Count; i++)
        {
            GameObject playerItem = Instantiate(playerTogglePrefab, contentPanel);
            playerItem.name = $"PlayerItem_{i}";
            var playerItemComponent = playerItem.GetComponent<PlayerItem>();
            playerItemComponent.Initialize(i, _listCard.sprites[i], lobbyController);
        }
    }

    public Transform GetContentPanelTransform()
    {
        return contentPanel;
    }
}
