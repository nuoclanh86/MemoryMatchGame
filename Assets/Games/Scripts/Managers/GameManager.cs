using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name == "BootScene")
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
}