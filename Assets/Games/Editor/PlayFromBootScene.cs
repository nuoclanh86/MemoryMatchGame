#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class PlayFromBootScene
{
    private const string BootScenePath = "Assets/Games/Scenes/BootScene.unity";

    static PlayFromBootScene()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    [MenuItem("Game/Play From Boot Scene")]
    public static void PlayFromBootSceneMenu()
    {
        if (EditorApplication.isPlaying)
            return;

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        EditorSceneManager.OpenScene(BootScenePath);

        EditorApplication.isPlaying = true;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        
    }
}

#endif