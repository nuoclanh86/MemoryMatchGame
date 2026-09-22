using TMPro;
using UnityEngine;

public class PlayerInfoOnBoard : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image avatarImage;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerScore;

    [SerializeField] private GameObject vfxPlayerTurn;

    private PlayerData _playerData = new();
    public PlayerData GetPlayerData() => _playerData;
    private string playerScoreFormat = "Score: {0}";

    public void Initialize(int playerNumber)
    {
        var list = GameManager.Instance.SelectedPlayerIndices;
        int index = list.FindIndex(x => x.selectedIndex == playerNumber);
        int playerIndex = index >= 0 ? list[index].playerIndex : -1;

        if (playerIndex != -1)
        {
            _playerData = GameManager.Instance.GetPlayerData(playerIndex);
            InitializePlayerData();
        }
        else
        {
            Debug.LogError($"[PlayerInfoOnBoard] Player with selectedIndex {playerNumber} not found in SelectedPlayerIndices.");
        }
    }

    private void InitializePlayerData()
    {
        avatarImage.sprite = _playerData.avatar;
        playerName.text = _playerData.name ?? "Null";
        SetPlayerScore(0);
        SetPlayerTurn(false);
    }

    public void SetPlayerScore(int numberScore)
    {
        playerScore.text = string.Format(playerScoreFormat, numberScore);
    }

    public void SetPlayerTurn(bool isPlayerTurn)
    {
        vfxPlayerTurn.SetActive(isPlayerTurn);
    }
}
