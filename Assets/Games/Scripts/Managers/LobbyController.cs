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
            GameManager.Instance.SelectedPlayerIndices.Add((item.PlayerIndex, 0));
            bool moveResult = MoveItemToPreview(itemTransform);
            Debug.Log($"[LobbyController] Move item {item.PlayerIndex} to preview: {moveResult} , player count: {_playerChoicePreview.GetPlayerCount()}");
        }
        else if (itemTransform.IsChildOf(_playerChoicePreview.transform))
        {
            GameManager.Instance.SelectedPlayerIndices.RemoveAll(x => x.playerIndex == item.PlayerIndex);
            MoveItemToScroll(itemTransform);
            _playerChoicePreview.ReArangePlayerPreviewItems();
        }
        _playerChoicePreview.UpdatePlayerSumupText();
        UpdatePlayerPreviewIndices();
    }

    public bool MoveItemToPreview(Transform item)
    {
        if (item == null)
            return false;

        Transform newParent = _playerChoicePreview.GetPlayerPreviewContentTransform();
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

    private void MoveItemToScroll(Transform item)
    {
        item.SetParent(_playerChoiceScroll.GetContentPanelTransform(), false);
    }

    private void UpdatePlayerPreviewIndices()
    {
        Transform parent = _playerChoicePreview.GetPlayerPreviewContentTransform();

        if (parent == null)
            return;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.childCount == 0)
                break;

            PlayerItem playerItem = child.GetComponentInChildren<PlayerItem>();

            if (playerItem != null)
            {
                UpdateSelectedPlayerIndex(playerItem.PlayerIndex, i + 1);
            }
        }
    }
    private void UpdateSelectedPlayerIndex(int playerIndex, int selectedIndex)
    {
        int index = GameManager.Instance.SelectedPlayerIndices.FindIndex(
            x => x.playerIndex == playerIndex);

        if (index >= 0)
        {
            GameManager.Instance.SelectedPlayerIndices[index] = (playerIndex, selectedIndex);
        }
    }

    private void OnStartGameButtonClicked()
    {
        GameManager.Instance.StartGame(_playerChoicePreview?.GetPlayerCount() ?? 0);
    }

}
