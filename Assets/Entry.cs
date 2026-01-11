using LowoUN.UI;
using LowoUN.Util;
using UnityEngine;

namespace LowoUN.Scene {
    public class Entry : MonoBehaviour {
        [SerializeField] bool isDebug = true;

        bool isSkipSplashEnabled = true;
        void Start () {
            Log.Init (isDebug);

            UIManager.Instance.Init();

            StartGame ();
        }

        void StartGame () {
            if (isSkipSplashEnabled)
                GameController.Instance.SetState (GameState.Splash);
            else
                GameController.Instance.SetState (GameState.Login);
        }
    }
}