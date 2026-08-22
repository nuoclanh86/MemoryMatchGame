using TMPro;
using UnityEngine;

public class PlayerChoicePreview : MonoBehaviour
{
    [SerializeField] private Transform playerPreviewContent;
    [SerializeField] private TextMeshProUGUI playerSumupText;

    private readonly string formattedSumupText = "Số Người Chơi : {0}";
    public Transform GetPlayerPreviewContentTransform()
    {
        return playerPreviewContent;
    }

    public void UpdatePlayerSumupText()
    {
        int playerCount = playerPreviewContent.childCount;
        string formattedText = FormatPlayerSumupText(playerCount);
        if (playerSumupText != null)
            playerSumupText.text = formattedText;
        else
            Debug.LogError("[PlayerChoicePreview] playerSumupText is not assigned.");
    }

    private string FormatPlayerSumupText(int playerCount)
    {
        return string.Format(formattedSumupText, playerCount);
    }

    public int GetPlayerCount()
    {
        return playerPreviewContent.childCount;
    }
}

