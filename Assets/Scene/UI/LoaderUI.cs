using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LowoUN.UI {
    public class LoaderUI : MonoBehaviour {
        [SerializeField] Image stateProgress;
        [SerializeField] Image stateProgress_Batttle;
        [SerializeField] Text tipsText;
        [SerializeField] GameObject theme_Lobby;
        [SerializeField] GameObject theme_Battle;

        // 提示信息
        private int tipsIndex;
        private float time;
        private List<string> tipsInfo = new List<string> () { "Loading_Tip_1", "Loading_Tip_2", "Loading_Tip_3" };

        void Awake () {
            theme_Lobby.SetActive (false);
            theme_Battle.SetActive (false);
        }

        void Update () {
            time += Time.deltaTime;
            if (time >= 3) {
                time = 0;
                UpdateTipsInfo ();
            }
        }

        public void SetTheme_Lobby () {
            return;

            theme_Lobby.SetActive (true);
            theme_Battle.SetActive (false);

            time = 0;
            tipsIndex = Random.Range (0, tipsInfo.Count);
            UpdateTipsInfo ();
        }

        public void SetTheme_Battle () {
            return;

            theme_Lobby.SetActive (false);
            theme_Battle.SetActive (true);
        }

        private void UpdateTipsInfo () {
            tipsText.text = tipsInfo[tipsIndex];
            tipsIndex++;
            if (tipsIndex >= tipsInfo.Count)
                tipsIndex = 0;
        }

        public void UpdateProgress (float porgress) {
            if (!gameObject.activeInHierarchy)
                return;

            if (porgress >= 1f) {
                // stateProgress.gameObject.SetActive(false);
                // stateProgress_Batttle.gameObject.SetActive(false);
                return;
            }

            if (theme_Lobby.activeInHierarchy)
                stateProgress.fillAmount = porgress;
            else if (theme_Battle.activeInHierarchy)
                stateProgress_Batttle.fillAmount = porgress;
        }
    }
}