using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Util {
	public static class Log {
		// 是否允许Unity运行时日志
		static void SetLogEnabled (bool isRuntimeLogEnabled) {
#if UNITY_EDITOR
			Debug.unityLogger.logEnabled = true;
#else
			Debug.unityLogger.logEnabled = isRuntimeLogEnabled;
#endif
		}
		// 设置Unity输出日志等级
		static void SetLogLevel () {
			// 用于错误类型
			var logType = LogType.Error;
			// Debug.unityLogger.Log(LogType.Error);
			// 仅输出 托管堆栈跟踪
			var traceType = StackTraceLogType.ScriptOnly;
			Application.SetStackTraceLogType (logType, traceType);
		}

		public static void Init (bool isDebug) {
			if (!isDebug) {
				Log.SetOpen (0);
				return;
			}
	
			TextAsset txt = Resources.Load ("Setting_Log") as TextAsset;
			// 以换行符作为分割点，将该文本分割成若干行字符串，并以数组的形式来保存每行字符串的内容
			string[] str = txt.text.Split ('\n');
			// 将每行字符串的内容以逗号作为分割点，并将每个逗号分隔的字符串内容遍历输出
			for (int i = 0; i < str.Length; i++) {
				// Debug.Log("___"+str[i]);
				if (i == 0) {
					Log.SetOpen (int.Parse (str[0]));
					continue;
				}

				// Debug.Log("________"+str[i]);
				if (string.IsNullOrWhiteSpace (str[i]))
					continue;

				string[] ss = str[i].Split ('#');
				// Debug.Log("________ "+ss.Length);
				if (ss.Length == 1)
					Log.OpenTag (str[i].Trim ());
			}
		}

		private static Dictionary<string, string> tags = new Dictionary<string, string> ();

		private static bool isOpen = false;
		public static bool IsOpen => isOpen;
		public static void SetOpen (int openState) {
			isOpen = openState == 1;
		}

		static void OpenTag (string tag) {
			if (!isOpen) return;

			tags[tag] = tag.ToString ();
			// Debug.Log("tag:"+tag);
		}

		// Error和Warn不需要标签
		[System.Diagnostics.Conditional ("PROJECT_LOG")]
		public static void Error (params object[] msg) {
			if (!isOpen) return;

			Debug.LogError ("【Error】" + ParseMsg (msg));
		}

		[System.Diagnostics.Conditional ("PROJECT_LOG")]
		public static void Warn (params object[] msg) {
			if (!isOpen) return;

			Debug.LogWarning ("【Warn】" + ParseMsg (msg));
		}

		[System.Diagnostics.Conditional ("PROJECT_LOG")]
		public static void Trace (string tag, params object[] msg) {
			Print (tag, ParseMsg (msg), "FFFFFF");
		}

		[System.Diagnostics.Conditional ("PROJECT_LOG")]
		public static void Red (string tag, params object[] msg) {
			Print (tag, ParseMsg (msg), "FF5C95");
		}

		[System.Diagnostics.Conditional ("PROJECT_LOG")]
		public static void Green (string tag, params object[] msg) {
			Print (tag, ParseMsg (msg), "90FF81");
		}

		[System.Diagnostics.Conditional ("PROJECT_LOG")]
		public static void Orange (string tag, params object[] msg) {
			Print (tag, ParseMsg (msg), "FFAE00");
		}

		[System.Diagnostics.Conditional ("PROJECT_LOG")]
		public static void Gray (string tag, params object[] msg) {
			Print (tag, ParseMsg (msg), "606060");
		}

		[System.Diagnostics.Conditional ("PROJECT_LOG")]
		public static void Blue (string tag, params object[] msg) {
			Print (tag, ParseMsg (msg), "3A5FCD");
		}

		private static void Print (string tag, object msg, string color) {
			if (!isOpen) return;

			if (!tags.ContainsKey (tag))
				return;

			Debug.Log ("<color=#" + color + ">" + "【" + tags[tag] + "】 " + msg + "</color>");
		}

		// [System.Diagnostics.Conditional("PROJECT_LOG")]
		// 解第一层
		private static string ParseMsg (params object[] msg) {
			// Debug.Log ("ParseObjects() length: " + msg.Length);
			if (msg.Length == 1)
				return GetString (msg[0]);

			var str = "";

			for (int i = 0; i < msg.Length; i++) {
				var s = (i == 0 ? "" : ", ") + GetString (msg[i]);
				str += s;
			}

			return str;
		}

		// 解第二层
		// [System.Diagnostics.Conditional("PROJECT_LOG")]
		private static string GetString (object msg) {
			string detail = "";
			if (msg is ICollection)
				detail = Stringify (msg as ICollection);
			else
				detail = msg.ToString ();

			return detail;
		}

		// [System.Diagnostics.Conditional("PROJECT_LOG")]
		private static string Stringify (ICollection col) {
			var str = "";
			var isFirst = true;
			foreach (var item in col) {
				// item 还是有可能是集合，不递归解下去了！！！
				if (isFirst) {
					str += item.ToString ();
					isFirst = false;
				} else
					str += "+" + item.ToString ();
			}
			return str;
		}
	}
}