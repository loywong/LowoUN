using System;
using LowoUN.Util;
using UnityEngine;

namespace LowoUN.Guide {
    public enum GuideAreaType {
        Rect, // 矩形引导
        Circle, // 圆形引导
        Tips, // 只有问题提示
    }

    public class GuideView : MonoBehaviour, ICanvasRaycastFilter {
        public static GuideView Instance;

        [SerializeField] Camera m_mainCamera; // Should be provided by CameraManager
        public Camera mainCamera => m_mainCamera;
        [SerializeField] Canvas m_uiCavans;
        public Canvas uiCavans => m_uiCavans;
        [SerializeField] GuideUI m_ui;

        [SerializeField] Material rectMat;
        [SerializeField] Material circleMat;

        private RectTransform uiTarget;
        private GuideAreaType guideType;

        void Awake () {
            Instance = this;
        }

        public void Init () {
            if (rectMat == null || circleMat == null)
                throw new System.Exception ("材质未赋值!");

            m_ui.Init ();
            Reset ();
        }

        public void Exit () {
            Reset ();
        }

        void Reset () {
            m_ui.Reset ();
        }

        public void SetMaskEnable () {
            m_ui.Mask.raycastTarget = true;
            m_ui.Mask.gameObject.SetActive (true);
        }

        public void SetComnBtn (Action cb_comnBtn) {
            m_ui.SetComnBtn (cb_comnBtn);
        }

        public void GuideTip (string info) {
            m_ui.ShowTextInfo (info);
        }

        public void Guide3D (Transform sceneObj, GuideAreaType guideType, float size1, float size2) {
            Reset ();

            SetPenetrate (false);

            var pos = Utils.TransformToCanvasLocalPosition (sceneObj, uiCavans, mainCamera);
            m_ui.ShowHand (pos);
        }

        public void GuideUI (RectTransform target, GuideAreaType guideType) {
            Reset ();

            SetPenetrate (true);

            this.uiTarget = target;
            this.guideType = guideType;

            if (guideType == GuideAreaType.Rect)
                m_ui.RectGuide (target, rectMat);
            else if (guideType == GuideAreaType.Circle)
                m_ui.CircleGuide (target, circleMat);
        }

        private bool isPenetrate; // 是否可以穿透点击
        private void SetPenetrate (bool penetrate) {
            isPenetrate = penetrate;
            m_ui.Mask.gameObject.SetActive (isPenetrate);
        }

        public bool IsRaycastLocationValid (Vector2 sp, Camera eventCamera) {
            if (isPenetrate)
                return !RectTransformUtility.RectangleContainsScreenPoint (uiTarget, sp, eventCamera);
            else
                return true;
        }
    }
}