﻿using UnityEngine;

namespace LowoUN.Util {
	public static class ColorTrans {
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