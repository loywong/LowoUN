#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// 确保跳转的场景在编辑器的 Build Settings中
public class FixedStartupScene : Editor {
    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize () {
        // 固定从Entry场景启动
        Scene scene = SceneManager.GetActiveScene ();
        if (scene.name.Equals ("Splash") || scene.name.Equals ("Login") || scene.name.Equals ("Lobby") || scene.name.Equals ("Battle"))
            SceneManager.LoadScene ("Entry");
    }
}
#endif