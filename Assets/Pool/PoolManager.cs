using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LowoUN.Util {
    public class PoolManager : Manager<PoolManager> {
        Dictionary<string, ObjectPool<GameObject>> pools = new ();

        // public ObjectPool<GameObject> CreatePool_ByName(string resName) {
        //     return null;
        // }

        /// <summary>
        /// Init a pool
        /// </summary>
        /// <param name="resObj"></param>
        /// <param name="defaultCapacity"></param>
        /// <param name="poolMaxSize">对象池最大容量</param>
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

            if (pools == null)
                pools = new ();
            pools[resObj.name] = pool;

            return pool;
        }

        public void DestroyPool (ObjectPool<GameObject> p) {
            p.Clear (); //p.Dispose();
            p = null;
        }

        // 清空所有类型的对象池
        public void End () {
            if (pools == null || pools.Count <= 0)
                return;

            foreach (var pool in pools)
                DestroyPool (pool.Value);
            // pool.Value.Clear();
            pools.Clear ();
            pools = null;
        }
    }
}