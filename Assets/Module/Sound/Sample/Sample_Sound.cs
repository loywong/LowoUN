using System.Collections;
using UnityEngine;

namespace LowoUN.Module.Sound.Sample {
	public class Sample_Sound : MonoBehaviour {
		void OnGUI () {
			//1, BGM
			GUI.skin.button.fontSize = 30;

			if (GUI.Button (new Rect (10, 10, 250, 100), "Play BGM"))
				Module_Sound.instance.PlayBgm ("Bgm_Battle1");

			if (GUI.Button (new Rect (310, 10, 250, 100), "Stop BGM"))
				Module_Sound.instance.StopBgm ();

			//2, Event
			if (GUI.Button (new Rect (10, 130, 250, 100), "Play Event\n(Battle_Win)"))
				Module_Sound.instance.PlayEvt ("Battle_Win");

			if (GUI.Button (new Rect (310, 130, 250, 100), "Stop Evt"))
				Module_Sound.instance.StopEvts ();

			//3, UI
			if (GUI.Button (new Rect (10, 250, 250, 100), "Play UI"))
				Module_Sound.instance.PlayUI ("Click_Button");
		}
	}
}