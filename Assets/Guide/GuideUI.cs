using System;
using UnityEngine;
using UnityEngine.UI;

namespace LowoUN.Guide {
    public class GuideUI : MonoBehaviour {
        [SerializeField] Image m_Mask;
        public Image Mask => m_Mask;
        [SerializeField] GameObject m_Hand;
        [SerializeField] GameObject m_SelectBox;
        [SerializeField] GameObject m_textInfoCon;
        [SerializeField] Text m_textInfo;
        [SerializeField] Button m_btnComn;
        [SerializeField] CircleGuide circleGuide;
        [SerializeField] RectGuide rectGuide;
        Action cb_btnComn;

        void Awake () {
            // throw new System.Exception ("初始化失败!");
            Reset ();
        }

        public void Init () {
            // AdjustSomething?()
        }

        public void Reset () {
            m_Mask.raycastTarget = true;
            m_Mask.gameObject.SetActive (false);
            m_Hand.SetActive (false);
            m_SelectBox.SetActive (false);
            m_textInfoCon.SetActive (false);
            m_textInfo.text = string.Empty;
            m_btnComn.gameObject.SetActive (false);
            m_btnComn.onClick.RemoveAllListeners ();
        }

        public void ShowTextInfo (string info) {
            m_textInfoCon.SetActive (true);
            m_textInfo.text = info;
        }

        public void SetComnBtn (Action cb_btnComn) {
            m_btnComn.gameObject.SetActive (true);
            m_btnComn.onClick.AddListener (() => { cb_btnComn (); });
        }

        public void RectGuide (RectTransform target, Material material) {
            m_Mask.material = material;
            rectGuide.Guide (target, m_Mask.material);
        }

        public void CircleGuide (RectTransform target, Material material) {
            m_Mask.material = material;
            circleGuide.Guide (target, m_Mask.material);
        }

        public void ShowHand (Vector2 pos, float rotationZ = 0) {
            m_Hand.SetActive (true);
            m_Hand.GetComponent<RectTransform> ().anchoredPosition = pos;
            m_Hand.GetComponent<RectTransform> ().localEulerAngles = new Vector3 (0, 0, rotationZ);
        }
    }
}