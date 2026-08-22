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
            MoveItem(itemTransform, _playerChoicePreview.GetPlayerPreviewContentTransform());
        }
        else if (itemTransform.IsChildOf(_playerChoicePreview.transform))
        {
            MoveItem(itemTransform, _playerChoiceScroll.GetContentPanelTransform());
        }
        _playerChoicePreview.UpdatePlayerSumupText();
    }

    private void MoveItem(Transform item, Transform newParent)
    {
        item.SetParent(newParent, false);
    }

    private void OnStartGameButtonClicked()
    {
        GameManager.Instance.StartGame(_playerChoicePreview?.GetPlayerCount() ?? 0);
    }
}
