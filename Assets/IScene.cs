public interface IScene {
    void OnStart ();
    // void OnUpdate ();
    void OnEnd ();
    // void OnRestart (); // 在不切换到其他场景的情况下，重新开始
    // void OnRecover (); // 恢复现场（断线重连）
}