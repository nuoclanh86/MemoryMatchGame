using UnityEngine;

public class PlayerChoiceScrollView : MonoBehaviour
{
    [SerializeField] private GameObject playerTogglePrefab;
    [SerializeField] private Transform contentPanel;

    private ListPlayerSO _listPlayer;

    public void CreatePlayerChoices(LobbyController lobbyController)
    {
        if (_listPlayer == null)
        {
            _listPlayer = GameManager.Instance.ListPlayer;
        }

        if (_listPlayer == null || _listPlayer.players == null || _listPlayer.players.Count == 0)
        {
            Debug.LogError("ListPlayerSO is not assigned or contains no player data.");
            return;
        }
        for (int i = 0; i < _listPlayer.players.Count; i++)
        {
            GameObject playerItem = Instantiate(playerTogglePrefab, contentPanel);
            playerItem.name = $"PlayerItem_{i}";
            var playerItemComponent = playerItem.GetComponent<PlayerItem>();
            playerItemComponent.Initialize(i, _listPlayer.players[i].avatar, lobbyController);
        }
    }

    public Transform GetContentPanelTransform()
    {
        return contentPanel;
    }
}
