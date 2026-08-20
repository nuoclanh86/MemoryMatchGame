using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] Button playButton;

    void Start()
    {
        if (playButton != null && GameManager.Instance != null)
            playButton.onClick.AddListener(GameManager.Instance.StartGame);
        else
            Debug.LogError("[LobbyController] Can not find playButton or GameManager instance.");
    }
}
