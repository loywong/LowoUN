using LowoUN.Util;
using UnityEngine;

namespace LowoUN.Guide {
    public class RectGuide : MonoBehaviour {
        Material material; // 材质
        Vector3 center; // 镂空区域的中心
        float width; // 镂空区域的宽
        float height; // 镂空区域的高

        public Vector3 Center => material.GetVector ("_Center");

        // 引导
        public void Guide (RectTransform target, Material material) {
            Debug.Log ($"guide_ target {target.name}");
            Debug.Log ($"guide_ target.sizeDelta {target.sizeDelta.ToString ()}");
            Debug.Log ($"guide_ target.position {target.position.ToString ()}");
            // ---------------------------------------------------------------
            Vector3[] targetCorners = Utils.GetTargetCorners (target, GuideView.Instance.uiCavans);

            // 计算中心点
            center.x = targetCorners[0].x + (targetCorners[3].x - targetCorners[0].x) / 2;
            center.y = targetCorners[0].y + (targetCorners[1].y - targetCorners[0].y) / 2;
            // 设置中心点
            material.SetVector ("_Center", center);
            Debug.Log ($"guide_ target.position _Center {center}");

            // 计算宽 和 高 
            width = (targetCorners[3].x - targetCorners[0].x) / 2;
            height = (targetCorners[1].y - targetCorners[0].y) / 2;
            // Debug.Log ($"guide_ target.width {width}, target.height {height}");
            material.SetFloat ("_SliderX", width);
            material.SetFloat ("_SliderY", height);
        }
    }
}