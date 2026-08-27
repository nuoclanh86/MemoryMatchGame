using UnityEngine;

public class PopupPlayerWon : MonoBehaviour
{

    public void Initialize()
    {
        // Initialize the popup if needed
    }

    public void OnPlayAgainButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

}
