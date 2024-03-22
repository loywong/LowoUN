using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LowoUN.Util {
    public class PoolManager : Manager<PoolManager> {
        // 粒子特效 专用
        // ------------------------------------------------------------------------------------ PaticleSystem begin
        IObjectPool<ParticleSystem> m_Pool;
        public IObjectPool<ParticleSystem> Pool_ParticleSystem => m_Pool;

        public void CreatePool_ParticleSystem () {
            m_Pool = new LinkedPool<ParticleSystem> (CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        }

        ParticleSystem CreatePooledItem () {
            var go = new GameObject ("Pooled Particle System");
            var ps = go.AddComponent<ParticleSystem> ();

            ps.Stop (true, ParticleSystemStopBehavior.StopEmittingAndClear);

            var main = ps.main;
            main.duration = 1;
            main.startLifetime = 1;
            main.loop = false;

            // This is used to return ParticleSystems to the pool when they have stopped.
            go.AddComponent<PoolReleaseParticleSytem> ();

            return ps;
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
        Dictionary<string, ObjectPool<GameObject>> pools = new ();

        // public ObjectPool<GameObject> CreatePool_ByName(string resName) {
        //     return null;
        // }

        public ObjectPool<GameObject> CreatePool (GameObject resObj, int defaultCapacity, int poolMaxSize) {
            var pool = new ObjectPool<GameObject> (
                () => {
                    GameObject gameObject = GameObject.Instantiate (resObj);
                    Debug.Log ($"______ Create() {gameObject.name}");
                    return gameObject;
                },
                (GameObject gameObject) => {
                    gameObject.SetActive (true);
                    Debug.Log ($"______ Get() {gameObject.name}");
                },
                (GameObject gameObject) => {
                    gameObject.SetActive (false);
                    Debug.Log ($"______ Release() {gameObject.name}");
                },
                (GameObject gameObject) => {
                    Debug.Log ($"______ Destroy() Pool obj: {gameObject.name}");
                    GameObject.Destroy (gameObject);
                },
                true,
                defaultCapacity,
                poolMaxSize);

            pools[resObj.name] = pool;

            return pool;
        }

        public void DestroyPool (ObjectPool<GameObject> p) {
            p.Clear (); //p.Dispose();
            p = null;
        }
        // ------------------------------------------------------------------------------------ GameObject end

        // 清空所有类型的对象池
        public void Init () {
            pools = new Dictionary<string, ObjectPool<GameObject>> ();
            CreatePool_ParticleSystem ();
        }
        public void End () {
            if (pools != null && pools.Count > 0) {
                foreach (var pool in pools)
                    DestroyPool (pool.Value);
                // pool.Value.Clear();
                pools.Clear ();
            }
            pools = null;

            if (m_Pool != null)
                m_Pool.Clear ();
            m_Pool = null;
        }
    }
}