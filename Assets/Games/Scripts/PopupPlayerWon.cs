using UnityEngine;

public class PopupPlayerWon : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI playerWonText;

    public void Initialize(string playerName, int playerScore)
    {
        playerWonText.text = $"Player {playerName} won the game with a score of {playerScore}!";
    }

    public void OnPlayAgainButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

}
