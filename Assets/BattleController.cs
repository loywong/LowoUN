using System;
using System.Collections;
using LowoUN.Util;

namespace LowoUN.Scene {
    public class BattleController : Manager<BattleController>, IScene {
        public void Enter___ () { }

        public void Exit___ () { }

        // 在不切换到其他场景的情况下，重新开始
        public void OnRestart () { }

        // 恢复现场（断线重连）
        public void OnRecover () { }

        public void Update___ () {
            throw new NotImplementedException ();
        }

        public void GetInitDataOnceFromServer___ (Action cb) {
            throw new NotImplementedException ();
        }

        public void Init___ () {
            throw new NotImplementedException ();
        }

        public IEnumerator Load_3D___ (Action initEnd_3D) {
            throw new NotImplementedException ();
        }

        public void Load_HudUI___ (Action cb) {
            throw new NotImplementedException ();
        }

        public void Check_Hide_Loading___ (Action cb) {
            throw new NotImplementedException ();
        }

        public void Clear___ () {
            throw new NotImplementedException ();
        }
    }
}