using LowoUN.Util;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Sample_Pool_ParticleSytem : MonoBehaviour {
    [SerializeField] ParticleSystem particle; //case particle
    IObjectPool<ParticleSystem> cubePool;

    void Awake () {
        cubePool = PoolManager.Instance.CreatePool_ParticleSystem (particle);
    }

    void OnGUI () {
        GUI.skin.button.fontSize = 24;

        if (cubePool != null) {
            GUI.skin.label.fontSize = 30;
            GUILayout.Label ($"Pool size: {cubePool.CountInactive}");
            if (GUI.Button (new Rect (200, 5, 240, 40), "Create Particles")) {
                var amount = Random.Range (1, 10);
                for (int i = 0; i < amount; ++i) {
                    var ps = cubePool.Get ();
                    ps.transform.position = Random.insideUnitSphere * 10;
                    ps.Play ();
                }
            }
        }

        // if (GUI.Button (new Rect (10, 200, 160, 40), "Pool Init()"))
        //     PoolManager.Instance.Init ();
        if (GUI.Button (new Rect (450, 5, 160, 40), "Destroy Pool")) {
            PoolManager.Instance.DestroyPool_ParticleSystem (cubePool);
        }
    }
}