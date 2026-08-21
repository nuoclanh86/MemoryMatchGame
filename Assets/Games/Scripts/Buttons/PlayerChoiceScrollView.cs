using UnityEngine;

public class PlayerChoiceScrollView : MonoBehaviour
{
    [SerializeField] private GameObject playerTogglePrefab;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private ListCardScriptableObject listCard;

    public void CreatePlayerChoices(LobbyController lobbyController)
    {
        if (listCard == null || listCard.sprites == null || listCard.sprites.Count == 0)
        {
            Debug.LogError("ListCardScriptableObject is not assigned or contains no sprites.");
            return;
        }
        for (int i = 0; i < listCard.sprites.Count; i++)
        {
            GameObject playerItem = Instantiate(playerTogglePrefab, contentPanel);
            playerItem.name = $"PlayerItem_{i}";
            var playerItemComponent = playerItem.GetComponent<PlayerItem>();
            playerItemComponent.Initialize(i, listCard.sprites[i], lobbyController);
        }
    }

    public Transform GetContentPanelTransform()
    {
        return contentPanel;
    }
}
