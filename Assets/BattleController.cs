using LowoUN.Util;

namespace LowoUN.Scene {
    public class BattleController : Manager<BattleController>, IScene {
        public void Enter___ () { }

        public void Exit___ () { }

        // 在不切换到其他场景的情况下，重新开始
        public void OnRestart () { }

        // 恢复现场（断线重连）
        public void OnRecover () { }
    }
}