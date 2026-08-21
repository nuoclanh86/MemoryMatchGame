using UnityEngine;

public class PlayerChoiceScrollView : MonoBehaviour
{
    [SerializeField] private GameObject playerTogglePrefab;
    [SerializeField] private GameObject ContentPanel;
    [SerializeField] private ListCardScriptableObject listCard;

    void Start()
    {
        // CreatePlayerChoices();
    }

    private void CreatePlayerChoices()
    {
        if (listCard == null || listCard.sprites == null || listCard.sprites.Count == 0)
        {
            Debug.LogError("ListCardScriptableObject is not assigned or contains no sprites.");
            return;
        }
        for (int i = 0; i < listCard.sprites.Count; i++)
        {
            GameObject playerToggle = Instantiate(playerTogglePrefab, ContentPanel.transform);
            playerToggle.name = $"PlayerToggle_{i}";
            playerToggle.GetComponent<PlayerToggle>().SetSprite(listCard.sprites[i]);
        }
    }
}
