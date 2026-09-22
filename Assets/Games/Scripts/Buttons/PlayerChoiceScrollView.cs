using UnityEngine;

public class PlayerChoiceScrollView : MonoBehaviour
{
    [SerializeField] private GameObject playerTogglePrefab;
    [SerializeField] private Transform contentPanel;

    public void CreatePlayerChoices(LobbyController lobbyController)
    {
        for (int i = 0; i < GameManager.Instance.GetTotalPlayers(); i++)
        {
            GameObject playerItem = Instantiate(playerTogglePrefab, contentPanel);
            playerItem.name = $"PlayerItem_{i}";
            var playerItemComponent = playerItem.GetComponent<PlayerItem>();
            playerItemComponent.Initialize(i, GameManager.Instance.GetPlayerData(i)?.avatar, lobbyController);
        }
    }

    public Transform GetContentPanelTransform()
    {
        return contentPanel;
    }
}
