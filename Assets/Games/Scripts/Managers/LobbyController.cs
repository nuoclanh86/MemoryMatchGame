using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] private PlayerChoiceScrollView _playerChoiceScroll;
    [SerializeField] private PlayerChoicePreview _playerChoicePreview;
    [SerializeField] Button playButton;

    private readonly int maxPlayers = 4;

    void Start()
    {
        if (playButton != null && GameManager.Instance != null)
            playButton.onClick.AddListener(OnStartGameButtonClicked);
        else
            Debug.LogError("[LobbyController] Can not find playButton or GameManager instance.");

        _playerChoiceScroll.CreatePlayerChoices(this);
        _playerChoicePreview.UpdatePlayerSumupText();
    }

    public void OnPlayerChoiceItemClicked(PlayerItem item)
    {

        Transform itemTransform = item.transform;

        if (_playerChoicePreview?.GetPlayerCount() < maxPlayers &&
                itemTransform.IsChildOf(_playerChoiceScroll.transform))
        {
            bool moveResult = MoveItemToPreview(itemTransform, _playerChoicePreview.GetPlayerPreviewContentTransform());
            Debug.Log($"[LobbyController] Move item {item.PlayerIndex} to preview: {moveResult} , player count: {_playerChoicePreview.GetPlayerCount()}");
            GameManager.Instance.SelectedPlayerIndices.Add(item.PlayerIndex);
        }
        else if (itemTransform.IsChildOf(_playerChoicePreview.transform))
        {
            MoveItemToScroll(itemTransform, _playerChoiceScroll.GetContentPanelTransform());
            GameManager.Instance.SelectedPlayerIndices.Remove(item.PlayerIndex);
            _playerChoicePreview.ReArangePlayerPreviewItems();
        }
        _playerChoicePreview.UpdatePlayerSumupText();
    }

    private bool MoveItemToPreview(Transform item, Transform newParent)
    {
        if (item == null || newParent == null)
            return false;

        Transform targetParent = null;

        // Tìm child đầu tiên không có child
        foreach (Transform child in newParent)
        {
            if (child.childCount == 0)
            {
                targetParent = child;
                break;
            }
        }

        // Không tìm được slot trống
        if (targetParent == null)
            return false;

        // Set parent
        item.SetParent(targetParent, false);

        // Set position về center
        RectTransform rectItem = item.GetComponent<RectTransform>();

        if (rectItem != null)
        {
            rectItem.anchorMin = new Vector2(0.5f, 0.5f);
            rectItem.anchorMax = new Vector2(0.5f, 0.5f);
            rectItem.pivot = new Vector2(0.5f, 0.5f);
            rectItem.anchoredPosition = Vector2.zero;
        }

        return true;
    }

    private void MoveItemToScroll(Transform item, Transform newParent)
    {
        item.SetParent(newParent, false);
    }

    private void OnStartGameButtonClicked()
    {
        GameManager.Instance.StartGame(_playerChoicePreview?.GetPlayerCount() ?? 0);
    }
}
