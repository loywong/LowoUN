using UnityEngine;

namespace LowoUN.Util {
    [RequireComponent (typeof (ParticleSystem))]
    public class PoolBehaviour_ParticleSytem : MonoBehaviour {
        ParticleSystem system;
        string poolKey;

        void Awake () {
            system = GetComponent<ParticleSystem> ();
            var main = system.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        public void Init (string poolKey) {
            this.poolKey = poolKey;
        }

        // 当粒子系统stop时，对象池回收，直接release
        void OnParticleSystemStopped () {
            // Debug.Log ("OnParticleSystemStopped");
            var pool = PoolManager.Instance.GetParticlePool (poolKey);
            if (pool != null)
                pool.Release (system);
            else
                Destroy (gameObject);
        }
    }
}