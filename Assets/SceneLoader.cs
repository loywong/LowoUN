using System;
using LowoUN.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LowoUN.Scene {
    public class SceneLoader : ManagerMono<SceneLoader> {
        Action<float> cb_loading;
        AsyncOperation asyncLoad;
        bool isLoading = false;
        public void LoadScene_New (string sceneName, Action<float> cb_loading, Action cb) {
            isLoading = true;
            this.cb_loading = cb_loading;
            asyncLoad = SceneManager.LoadSceneAsync (sceneName);
            asyncLoad.completed += (AsyncOperation ao) => {
                if (cb != null) cb ();
                isLoading = false;
            };
        }
        void Update () {
            if (isLoading)
                cb_loading (asyncLoad.progress);
        }
    }
}