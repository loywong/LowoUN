using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LowoUN.Util {
    public class PoolManager : Manager<PoolManager> {
        // 粒子特效 专用
        // ------------------------------------------------------------------------------------ PaticleSystem begin
        Dictionary<string, IObjectPool<ParticleSystem>> pools_particle = new ();
        public IObjectPool<ParticleSystem> GetParticlePool (string poolKey) {
            // Debug.Log ($"GetParticlePool {poolKey}");
            IObjectPool<ParticleSystem> p;
            if (pools_particle.TryGetValue (poolKey, out p))
                return p;
            return null;
        }
        public IObjectPool<ParticleSystem> CreatePool_ParticleSystem (ParticleSystem psAsset) {
            var pooKey = psAsset.name;
            var pool = new LinkedPool<ParticleSystem> (() => {
                    GameObject go = GameObject.Instantiate (psAsset.gameObject);

                    var psBehaviour = go.GetComponent<PoolBehaviour_ParticleSytem> ();
                    psBehaviour.Init (pooKey);

                    var ps = go.GetComponent<ParticleSystem> ();
                    // This is used to return ParticleSystems to the pool when they have stopped.
                    ps.Stop (true, ParticleSystemStopBehavior.StopEmittingAndClear);

                    var main = ps.main;
                    main.duration = 1;
                    main.startLifetime = 1;
                    main.loop = false;

                    return ps;
                },
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject);

            pools_particle[pooKey] = pool;
            return pool;
        }

        public void DestroyPool_ParticleSystem (IObjectPool<ParticleSystem> p) {
            p.Clear (); //p.Dispose();
            p = null;
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool (ParticleSystem system) {
            system.gameObject.SetActive (true);
        }
        // Called when an item is returned to the pool using Release
        void OnReturnedToPool (ParticleSystem system) {
            system.gameObject.SetActive (false);
        }
        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject (ParticleSystem system) {
            GameObject.Destroy (system.gameObject);
        }
        // ------------------------------------------------------------------------------------ PaticleSystem end

        // ------------------------------------------------------------------------------------ GameObject begin
        // Dictionary<string, ObjectPool<GameObject>> pools = new ();

        // public ObjectPool<GameObject> CreatePool_ByName(string resName) {
        //     return null;
        // }

        public ObjectPool<GameObject> CreatePool (GameObject resObj, int defaultCapacity, int poolMaxSize) {
            var pool = new ObjectPool<GameObject> (
                () => {
                    GameObject gameObject = GameObject.Instantiate (resObj);
                    return gameObject;
                },
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                true,
                defaultCapacity,
                poolMaxSize);

            // pools[resObj.name] = pool;

            return pool;
        }

        public void DestroyPool (ObjectPool<GameObject> p) {
            p.Clear (); //p.Dispose();
            p = null;
        }

        void OnTakeFromPool (GameObject go) {
            go.SetActive (true);
        }
        // Called when an item is returned to the pool using Release
        void OnReturnedToPool (GameObject go) {
            go.SetActive (false);
        }
        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject (GameObject go) {
            GameObject.Destroy (go);
        }
        // ------------------------------------------------------------------------------------ GameObject end

        // // 清空所有类型的对象池
        // public void Init () {
        //     pools = new();
        //     pools_particle = new();
        // }
        // public void End () {

        // }
        // void End_Pools_Cube () {
        //     if (pools != null && pools.Count > 0) {
        //         foreach (var pool in pools)
        //             DestroyPool (pool.Value);
        //         // pool.Value.Clear();
        //         pools.Clear ();
        //     }
        //     pools = null;
        // }
        // void End_Pools_ParticleSystem () {
        //     if (pools_particle != null && pools_particle.Count > 0) {
        //         foreach (var pool in pools_particle)
        //             DestroyPool_ParticleSystem (pool.Value);
        //         // pool.Value.Clear();
        //         pools_particle.Clear ();
        //     }
        //     pools_particle = null;
        // }
    }
}