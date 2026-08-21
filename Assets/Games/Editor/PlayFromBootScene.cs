#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class PlayFromBootScene
{
    private const string BootScenePath = "Assets/Games/Scenes/BootScene.unity";
    private const string PreviousSceneKey = "PlayFromBootScene.PreviousScene";
    private const string RestoreSceneKey = "PlayFromBootScene.ShouldRestore";

    static PlayFromBootScene()
    {
        // Debug.Log("PlayFromBootScene initialized.");

        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    [MenuItem("Game/Play From Boot Scene")]
    public static void PlayFromBootSceneMenu()
    {
        // Debug.Log("Play From Boot Scene menu item clicked.");

        if (EditorApplication.isPlaying)
            return;

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        // Save current scene into SessionState
        string currentScenePath = EditorSceneManager.GetActiveScene().path;

        if (string.IsNullOrEmpty(currentScenePath))
        {
            Debug.LogWarning("Current scene has not been saved.");
            return;
        }

        SessionState.SetString(PreviousSceneKey, currentScenePath);
        SessionState.SetBool(RestoreSceneKey, true);

        // Debug.Log($"Saved previous scene: {currentScenePath}");

        // Open Boot Scene
        EditorSceneManager.OpenScene(BootScenePath);

        // Enter Play Mode
        EditorApplication.isPlaying = true;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // Debug.Log($"OnPlayModeStateChanged: {state}");

        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            // DO NOT OpenScene here.
            // Unity is still in Play Mode.
            return;
        }

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            RestorePreviousScene();
        }
    }

    private static void RestorePreviousScene()
    {
        bool shouldRestore = SessionState.GetBool(RestoreSceneKey, false);

        if (!shouldRestore)
            return;

        string previousScenePath = SessionState.GetString(PreviousSceneKey, string.Empty);

        // Debug.Log($"Restoring previous scene: {previousScenePath}");

        // Clear state BEFORE opening scene
        SessionState.SetBool(RestoreSceneKey, false);
        SessionState.EraseString(PreviousSceneKey);

        if (string.IsNullOrEmpty(previousScenePath))
        {
            // Debug.LogWarning("Previous scene path is empty.");
            return;
        }

        if (!File.Exists(previousScenePath))
        {
            // Debug.LogWarning($"Previous scene no longer exists: {previousScenePath}");
            return;
        }

        EditorSceneManager.OpenScene(previousScenePath);
        // Debug.Log($"Restored previous scene: {previousScenePath}");
    }
}

#endif