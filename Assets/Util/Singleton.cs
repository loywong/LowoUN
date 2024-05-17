using UnityEngine;

namespace LowoUN.Util {
    public class Manager<T> where T : new () {
        protected static T _Instance = default (T);

        public static T Instance => _Instance ?? new T ();

        public virtual void Init () {
            Debug.Log ("[" + typeof (T).ToString () + "] of Singletom type inited!!!");
        }
    }

    // 基于UNity MonoBehaviour
    public class ManagerMono<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _Instance = default (T);

        public static T Instance {
            get {
                if (_Instance == null) {
                    GameObject go = GameObject.Find ("Manager");
                    if (go == null) {
                        go = new GameObject ("Manager");
                        DontDestroyOnLoad (go);

                        _Instance = go.AddComponent<T> ();
                    } else {
                        _Instance = go.GetComponent<T> ();
                        if (_Instance == null)
                            _Instance = go.AddComponent<T> ();
                    }
                }

                return _Instance;
            }
        }

        public virtual void Init () {
            Debug.Log ("[" + typeof (T).ToString () + "] of monobehaviour type inited!!!");
        }
    }
}