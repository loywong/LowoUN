using LowoUN.Util;
using UnityEngine;

namespace LowoUN.Guide {
    public class CircleGuide : MonoBehaviour {
        Material material; // 材质
        Vector3 center; // 镂空区域的中心
        float r; // 镂空区域圆形的半径

        public Vector3 Center => material.GetVector ("_Center");

        public void Guide (RectTransform target, Material material) {
            Vector3[] targetCorners = Utils.GetTargetCorners (target, GuideView.Instance.uiCavans);

            // 计算中心点
            center.x = targetCorners[0].x + (targetCorners[3].x - targetCorners[0].x) / 2;
            center.y = targetCorners[0].y + (targetCorners[1].y - targetCorners[0].y) / 2;
            // 设置中心点
            material.SetVector ("_Center", center);
            Debug.Log ($"guide_ target.position _Center {center}");

            float width = (targetCorners[3].x - targetCorners[0].x) / 2;
            float height = (targetCorners[1].y - targetCorners[0].y) / 2;

            // 计算半径 根据实际情况取值
            r = Mathf.Min (width, height); // or Max
            // r = Mathf.Sqrt (width * width + height * height);
            // Debug.LogError($"width: {width} height: {height} 计算半径: {r}");
            material.SetFloat ("_Slider", r);
        }
    }
}