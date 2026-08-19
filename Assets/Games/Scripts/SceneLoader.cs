using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private const string LobbyScene = "LobbyScene";
    private const string GameScene = "MemoryMatchScene";

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

    public void LoadLobby()
    {
        LoadScene(LobbyScene);
    }

    public void LoadGame()
    {
        LoadScene(GameScene);
    }

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneLoader] Scene name is empty.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}