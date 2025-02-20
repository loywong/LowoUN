using UnityEngine;

namespace LowoUN.Util {
	public static class Utils {
		public static Vector3[] GetTargetCorners (RectTransform target, Canvas uiCavans) {
			Vector3[] targetCorners = new Vector3[4]; // 要引导的目标的边界

			// 获取中心点 
			target.GetWorldCorners (targetCorners);

			// 把世界坐标 转成屏幕坐标
			for (int i = 0; i < targetCorners.Length; i++)
				targetCorners[i] = WorldToScreenPoint (targetCorners[i], uiCavans);

			return targetCorners;
		}

		static Vector2 WorldToScreenPoint (Vector3 world, Canvas uiCavans) {
			// Debug.Log ($"target world pos {world}");
			// 把世界坐标转成 屏幕坐标
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (uiCavans.worldCamera, world);
			// Debug.Log ($"guide target screenPoint {screenPoint}");
			Vector2 localPoint;
			// 把屏幕坐标 转成 局部坐标
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle (uiCavans.GetComponent<RectTransform> (), screenPoint, uiCavans.worldCamera, out localPoint))
				return localPoint;

			return Vector2.zero;
		}

		// Scene对象坐标转换到Canvas的局部坐标 WorldToUGUIPosition
		public static Vector2 TransformToCanvasLocalPosition (Transform current, Canvas canvas, Camera mainCam) {
			Debug.Log ($"current {current} canvas {canvas} canvas.worldCamera {canvas.worldCamera}");
			var uiCam = canvas.worldCamera;
			var screenPos = uiCam.WorldToScreenPoint (current.position - mainCam.transform.position);
			var canvasRect = canvas.GetComponent<RectTransform> ();
			Vector2 localPos;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle (canvasRect, screenPos, uiCam, out localPos))
				return localPos;

			return Vector2.zero;
		}

		public static int GetEnumID (string name, System.Type e) {
			foreach (int intValue in System.Enum.GetValues (e)) {
				if (name == System.Enum.GetName (e, intValue))
					return intValue;
			}

			//range(-2147483648～+2147483647)
			return -2147483648;
		}

		public static Color hexToColor (string hex) {
			//in case the string is formatted 0xFFFFFF
			hex = hex.Replace ("0x", "");
			//in case the string is formatted #FFFFFF 
			hex = hex.Replace ("#", "");

			//assume fully visible unless specified in hex
			byte a = 255;
			byte r = byte.Parse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);

			//Only use alpha if the string has enough characters
			if (hex.Length == 8) {
				a = byte.Parse (hex.Substring (6, 2), System.Globalization.NumberStyles.HexNumber);
			}

			return new Color32 (r, g, b, a);
		}
	}
}