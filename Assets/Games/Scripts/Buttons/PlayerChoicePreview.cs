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
        return playerPreviewContent.GetComponentsInChildren<PlayerItem>(true).Length;
    }

    public void ReArangePlayerPreviewItems()
    {
        Transform[] slots = new Transform[4];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = playerPreviewContent.transform.GetChild(i);
        }

        int targetSlotIndex = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            Transform sourceSlot = slots[i];

            if (sourceSlot.childCount == 0)
                continue;

            Transform playerItem = sourceSlot.GetChild(0);

            if (sourceSlot == slots[targetSlotIndex])
            {
                targetSlotIndex++;
                continue;
            }

            playerItem.SetParent(slots[targetSlotIndex], false);

            RectTransform rect = playerItem.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero;
            }

            targetSlotIndex++;
        }
    }
}

