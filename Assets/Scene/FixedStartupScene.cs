#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;

// 确保跳转的场景在编辑器的 Build Settings中
public class FixedStartupScene : MonoBehaviour {
    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize () {
        // 固定从Entry场景启动
        SceneManager.LoadScene ("Entry");
    }
}
#endif