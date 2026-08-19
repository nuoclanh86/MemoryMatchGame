using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    public void SetPlayerCount(int count)
    {
        if (count < 2 || count > 4)
        {
            Debug.LogWarning($"[GameManager] Invalid player count: {count}");
            return;
        }

        PlayerCount = count;
    }

    public void StartGame()
    {
        SceneLoader.Instance.LoadGame();
    }
}