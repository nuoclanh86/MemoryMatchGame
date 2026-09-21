using TMPro;
using UnityEngine;

public class PlayerInfoOnBoard : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image avatarImage;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerScore;

    [SerializeField] private GameObject vfxPlayerTurn;

    private string playerScoreFormat = "Score: {0}";

    public void Initialize(int playerNumber)
    {
        var player = GameManager.Instance.SelectedPlayerIndices.Find(x => x.selectedIndex == playerNumber);
        int playerIndex = player == default ? -1 : player.playerIndex;
        if (playerIndex != -1)
        {
            LoadAvatar(playerIndex);
            LoadPlayerName(playerIndex);
            SetPlayerScore(0);
            SetPlayerTurn(false);
        }
        else
        {
            Debug.LogError($"[PlayerInfoOnBoard] Player with selectedIndex {playerNumber} not found in SelectedPlayerIndices.");
        }
    }

    private void LoadAvatar(int playerIndex)
    {
        avatarImage.sprite = GameManager.Instance.ListPlayer.players[playerIndex].avatar;
    }
    private void LoadPlayerName(int playerIndex)
    {
        playerName.text = GameManager.Instance.ListPlayer.players[playerIndex].name;
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
