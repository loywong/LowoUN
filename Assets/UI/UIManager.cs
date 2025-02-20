using System;
using LowoUN.Scene;
using LowoUN.Util;
using UnityEngine;

namespace LowoUN.UI {
    public class UIManager : MonoBehaviour {
        public static UIManager Instance { get; private set; }
        public Canvas canvasRoot = null;
        LoaderUI loadingPanel;
        
        void Awake () {
            if (Instance != null) {
                Destroy (gameObject);
                return;
            }

            Log.Green ("flow", ">>>>>> UIManager{} Awake()");

            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        public void ShowLoadingUI (GameState state) {
            if (loadingPanel == null)
                loadingPanel = Load<LoaderUI> ("LoadingPanel", null, "LoadingPanel");

            if (state == GameState.Lobby || state == GameState.Login)
                loadingPanel.SetTheme_Lobby ();
            else if (state == GameState.Battle)
                loadingPanel.SetTheme_Battle ();

            loadingPanel.gameObject.SetActive (true);
        }

        public void HideLoadingUI () {
            if (loadingPanel != null)
                loadingPanel.gameObject.SetActive (false);
        }

        public void LoadingProgress (float porgress) {
            loadingPanel.UpdateProgress (porgress);
        }

        public T Load<T> (string prefabName, Transform parent = null, string assetPath = null) {
            var panel = Load (prefabName, parent, assetPath);
            if (panel == null) {
                Debug.LogError ("No Asset with prefabName:" + prefabName);
                return default (T);
            }
            return panel.GetComponent<T> ();
        }

        public GameObject Load (string prefabName, Transform parent = null, string assetPath = null) {
            if (parent == null) {
                if (canvasRoot == null)
                    return null;
                else
                    parent = canvasRoot.transform;
            }

            var path = String.IsNullOrEmpty (assetPath) ? prefabName : assetPath;

            // TODO parent != null
            var asset = Resources.Load (path); //, typeof(GameObject)
            if (asset == null) {
                Log.Error ("Path: " + path + ", has no asset");
                return null;
            }

            GameObject go = Instantiate (asset) as GameObject;
            go.name = prefabName;

            if (parent != null)
                SetParent (parent, go.transform);
            // panels[path] = go;

            return go;
        }

        // 设置子父节点关系
        private void SetParent (Transform parent, Transform child) {
            child.SetParent (parent, false);
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localEulerAngles = Vector3.zero;
        }
    }
}