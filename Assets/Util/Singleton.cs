using UnityEngine;

namespace LowoUN.Util {
    public class Manager<T> where T : new () {
        protected static T _Instance = default (T);

        public static T Instance {
            get {
                if (_Instance == null)
                    _Instance = new T ();
                return _Instance;
            }
        }

        public virtual void OnInit () {

        }
        public virtual void OnDispose () {
            _Instance = default (T);
        }
    }

    // 基于UNity MonoBehaviour
    public class ManagerMono<T> : MonoBehaviour where T : MonoBehaviour {
        protected static T _Instance = default (T);

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

        public virtual void OnInit () {
            Debug.Log ("[" + typeof (T).ToString () + "] of monobehaviour type oninited!!!");
        }
    }
}