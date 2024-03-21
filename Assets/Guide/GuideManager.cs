using System;
using LowoUN.Util;
using UnityEngine;

namespace LowoUN.Guide {
    public enum GuideStep {
        None,
        Step1,
        Step2,
        Step3,
        Step4,
    }

    public class GuideManager : Manager<GuideManager> {
        GuideView view;
        Action cb_complete;
        GameObject target; // 目标对象
        GuideAreaType guideAreaType;

        bool hasCompleteAll = true;

        GuideStep oldGuideStep;
        GuideStep step;

        public bool isWorking => step != GuideStep.None;

        public void Init (Action cb_complete) {
            hasCompleteAll = PlayerPrefs.GetInt ("Guide_Complete") == 1;
        }
        public void Uninit () {
            view?.Exit ();
            view = null;
        }

        public void SetComplete () {
            step = GuideStep.None;
            oldGuideStep = GuideStep.None;

            cb_complete?.Invoke ();

            Uninit ();

            hasCompleteAll = true;
            PlayerPrefs.SetInt ("Guide_Complete", 1);
            PlayerPrefs.Save ();
        }

        public void ClearData () {
            PlayerPrefs.DeleteKey ("Guide_Complete");
            PlayerPrefs.Save ();
        }

        /// <summary>
        /// 触发引导的具体步骤
        /// </summary>
        /// <param name="newStep"></param>
        /// <param name="is3DOrUI">引导的是场景对象，或者是UI对象</param>
        /// <param name="target"></param>
        /// <param name="areaType"></param>
        /// <param name="textInfo"></param>
        /// <param name="onComplete"></param>
        Action onCompleteStep;
        public void EnterGuide (GuideStep newStep, bool is3DOrUI, GameObject target = null, GuideAreaType areaType = GuideAreaType.Tips, string textInfo = null, Action onCompleteStep = null) {
            if (hasCompleteAll)
                return;

            if (isWorking && newStep == step) {
                Debug.LogWarning ($"guide_ EnterGuide, but [isWorking && newStep== curGuideStep] newStep is {newStep}");
                return;
            }

            if (step != GuideStep.None) {
                Debug.LogWarning ($"guide_ EnterGuide, but [curGuideStep != GuideStep.None] newStep is {newStep}, but curGuideStep is {step}");
                return;
            }

            this.onCompleteStep = onCompleteStep;

            Debug.Log ($"guide_ LevelManager SetState() {step} ====== > {newStep}");

            oldGuideStep = step;
            step = newStep;

            if (view == null) {
                view = GuideView.Instance;
                view.Init ();
            }

            switch (newStep) {
                case GuideStep.Step1:
                    Excute (false, target, areaType);
                    break;
                case GuideStep.Step2:
                    Excute (true, target, areaType);
                    break;
                case GuideStep.Step3:
                    Excute (false, target, areaType);
                    break;
                case GuideStep.Step4:
                    Excute_JustTips (textInfo, onCompleteStep);
                    break;
                default:
                    break;
            }
        }

        public void ExitGuide (GuideStep cur) {
            if (hasCompleteAll)
                return;

            if (step != cur) {
                Debug.LogWarning ($"guide_ ExitGuide cur step {cur}, curGuideStep {step}");
                return;
            }

            switch (cur) {
                case GuideStep.Step1:
                    break;
                case GuideStep.Step2:
                    break;
                case GuideStep.Step3:
                    break;
                case GuideStep.Step4:
                    break;
                default:
                    break;
            }

            oldGuideStep = step;
            step = GuideStep.None;
            Debug.Log ($"guide_ ExitGuide succ, step is {cur}");

            onCompleteStep?.Invoke ();
            view.Exit ();
        }

        public void Excute_JustTips (string tipsInfo, Action cb_complete) {
            view.SetMaskEnable ();
            view.SetComnBtn (cb_complete);
            view.GuideTip (tipsInfo);
        }
        public void Excute (bool is3DOrUI, GameObject target, GuideAreaType guideAreaType) {
            this.target = target;
            this.guideAreaType = guideAreaType;

            if (is3DOrUI) {
                Debug.Log ($"guide_ tar.transform {target.transform.name}");
                view.Guide3D (target.transform, guideAreaType, 100, 100);
            } else {
                view.GuideUI (target.GetComponent<RectTransform> (), guideAreaType);
            }
        }
    }
}