#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// 确保跳转的场景在编辑器的BuildSettings中
public class FixedStartupScene : Editor {
    private static string[] scenePaths = new string[] {
        "Assets/Scene/Splash.unity",
        "Assets/Scene/Login.unity",
        "Assets/Scene/Lobby.unity",
        "Assets/Scene/Battle.unity"
    };

    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize () {
        // 固定从Entry场景启动
        Scene scene = SceneManager.GetActiveScene ();
        bool isParticularScene = false;
        for (int i = 0; i < scenePaths.Length; i++) {
            if (string.Equals (scenePaths[i], scene.path)) {
                isParticularScene = true;
                break;
            }
        }
        if (isParticularScene)
            SceneManager.LoadScene ("Entry");
    }
}
#endif