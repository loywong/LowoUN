using UnityEngine;

namespace LowoUN.Util {
    [RequireComponent (typeof (ParticleSystem))]
    public class PoolReleaseParticleSytem : MonoBehaviour {
        ParticleSystem system;

        void Awake () {
            system = GetComponent<ParticleSystem> ();
            var main = system.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        // 当粒子系统stop时，对象池回收，直接release
        void OnParticleSystemStopped () {
            PoolManager.Instance.Pool_ParticleSystem.Release (system);
        }
    }
}