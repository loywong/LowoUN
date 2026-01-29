using System;
using System.Collections;

public interface IScene {
    void Update___ ();

    // ---------------------------------------------------------------------------- 进入 流程
    // 初始化 
    // 场景相关的 玩家初始数据，只需要请求一次
    void GetInitDataOnceFromServer___ (Action cb);
    // 逻辑数据 MVC的Modle 业务逻辑相关的数据模型 等
    void Init___ ();

    // 预加载
    // 非阻塞，但是影响 Start的时机
    // 场景最开始触发的资源, 比如 1 公共资源 2 常见逻辑(第一次刷怪, 第一次冲刺)
    // 背景音乐应该是非阻塞式的，且不阻塞Start的时机
    // void PreLoad ();

    // // 3D资源 和 UI资源全部加载完成 和 初始化之后 开始正式逻辑
    IEnumerator Load_3D___ (Action initEnd_3D);
    void Load_HudUI___ (Action cb);

    // 每个场景可以自定义loadingScene ui的隐藏时机
    void Check_Hide_Loading___ (Action cb);

    void Enter___ ();

    // ---------------------------------------------------------------------------- 退出 流程
    // 离开本场景
    // 切换场景 有效退出
    // 1 清理 逻辑
    void Exit___ ();
    // 2 清理资源 开始加载新场景文件之前，逻辑清理本场景各种事件与资源
    void Clear___ ();

    // ---------------------------------------------------------------------------- Reload
    // void OnRestart (); // 在不切换到其他场景的情况下，重新开始
    // void OnRecover (); // 恢复现场（断线重连）
}