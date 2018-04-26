using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Util {

	public static class Log {
		private static Dictionary<string, string> tags = new Dictionary<string, string> ();

		public static void OpenTag (string tag) {
			tags[tag] = tag.ToString ();
		}

		public static void Error (string tag, object msg) {
			if (!tags.ContainsKey (tag))
				return;
			Debug.LogError ("【" + tags[tag] + "】 " + "[Error] " + msg);
		}
		public static void Warn (string tag, object msg) {
			if (!tags.ContainsKey (tag))
				return;
			Debug.LogWarning ("【" + tags[tag] + "】 " + "[Error] " + msg);
		}

		public static string GetString (object msg) {
			string detail = "";
			if (msg is ICollection)
				detail = msg.ToString (); //JSON.stringify(msg);
			else
				detail = msg.ToString ();

			return detail;
		}

		public static void Print (string tag, object msg, string color) {
			if (!tags.ContainsKey (tag))
				return;

			Debug.Log ("<color=#" + color + ">" + "【" + tags[tag] + "】 " + GetString (msg) + "</color>");
		}

		public static void Trace (string tag, object msg) {
			// string color = GetColor (Enum_Color.Info);
			// Debug.Log("Trace color: " + color);
			// Print (tag, msg, color);
			Print (tag, msg, "ffffff");
		}

		public static void Red (string tag, object msg) {
			// string color = GetColor (Enum_Color.Red);
			// Debug.Log("Red color: " + color);
			// Print (tag, msg, color);
			Print (tag, msg, "FF5C95");
		}
		public static void Green (string tag, object msg) {
			// string color = GetColor (Enum_Color.Green);
			// Debug.Log("Green color: " + color);
			// Print (tag, msg, color);
			Print (tag, msg, "90FF81");
		}
		public static void Orange (string tag, object msg) {
			// string color = GetColor (Enum_Color.Orange);
			// Debug.Log("Orange color: " + color);
			// Print (tag, msg, color);
			Print (tag, msg, "FFAE00");
		}
		public static void Gray (string tag, object msg) {
			// string color = GetColor (Enum_Color.Gray);
			// Debug.Log("Gray color: " + color);
			// Print (tag, msg, color);
			Print (tag, msg, "606060");
		}
		public static void Blue (string tag, object msg) {
			// string color = GetColor (Enum_Color.Blue);
			// Debug.Log("Blue color: " + color);
			// Print (tag, msg, color);
			Print (tag, msg, "3a5fcd"); //00abff
		}
	}
}