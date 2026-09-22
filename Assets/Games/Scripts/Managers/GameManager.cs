using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneNames
{
    public const string MEMORY_MATCH = "MemoryMatchScene";
    public const string LOBBY = "LobbyScene";
    public const string BOOT = "BootScene";
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private ListPlayerSO _listPlayer;
    [SerializeField] private ListCardSO _listCard;


    // public ListPlayerSO ListPlayer => _listPlayer;
    public ListCardSO ListCard => _listCard;

    public static GameManager Instance { get; private set; }

    public int PlayerCount { get; private set; } = 2;

    private List<(int playerIndex, int selectedIndex)> _selectedPlayerIndices = new List<(int playerIndex, int selectedIndex)>();

    public List<(int playerIndex, int selectedIndex)> SelectedPlayerIndices => _selectedPlayerIndices;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == SceneNames.BOOT)
        {
            SceneLoader.Instance.LoadLobby();
        }
    }

    public void StartGame(int playerCount)
    {
        if (playerCount < 1 || playerCount > 4)
        {
            Debug.Log($"[GameManager] Invalid player count: {playerCount}");
            return;
        }
        PlayerCount = playerCount;
        SceneLoader.Instance.LoadGame();
    }

    public void RestartGame()
    {
        if (SceneManager.GetActiveScene().name == SceneNames.MEMORY_MATCH)
        {
            SceneLoader.Instance.LoadLobby();
        }
    }

    public PlayerData GetPlayerData(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= _listPlayer.players.Count)
        {
            Debug.LogError($"[GameManager] Invalid player index: {playerIndex}");
            return null;
        }
        return _listPlayer.players[playerIndex];
    }

    public int GetTotalPlayers()
    {
        return _listPlayer.players.Count;
    }
}