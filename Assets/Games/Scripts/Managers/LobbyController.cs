using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] private PlayerChoiceScrollView _playerChoiceScroll;
    [SerializeField] private PlayerChoicePreview _playerChoicePreview;
    [SerializeField] Button playButton;

    void Start()
    {
        if (playButton != null && GameManager.Instance != null)
            playButton.onClick.AddListener(GameManager.Instance.StartGame);
        else
            Debug.LogError("[LobbyController] Can not find playButton or GameManager instance.");

        _playerChoiceScroll.CreatePlayerChoices(this);
    }

    public void OnPlayerChoiceItemClicked(PlayerItem item)
    {
        Transform itemTransform = item.transform;

        if (itemTransform.IsChildOf(_playerChoiceScroll.transform))
        {
            MoveItem(itemTransform, _playerChoicePreview.GetPlayerPreviewContentTransform());
        }
        else if (itemTransform.IsChildOf(_playerChoicePreview.transform))
        {
            MoveItem(itemTransform, _playerChoiceScroll.GetContentPanelTransform());
        }
    }

    private void MoveItem(Transform item, Transform newParent)
    {
        item.SetParent(newParent, false);
    }
}
