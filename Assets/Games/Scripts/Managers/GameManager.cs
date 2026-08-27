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
    [SerializeField] private ListCardScriptableObject _listCard;
    public ListCardScriptableObject ListCard => _listCard;

    public static GameManager Instance { get; private set; }

    public int PlayerCount { get; private set; } = 2;

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
}