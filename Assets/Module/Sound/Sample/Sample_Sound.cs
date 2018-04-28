using System.Collections;
using UnityEngine;

namespace LowoUN.Module.Sound.Sample {
	public class Sample_Sound : MonoBehaviour {
		void Start () {
			INI.OnInit ();
		}

		void OnGUI () {
			//1, BGM
			GUI.skin.button.fontSize = 30;

			if (GUI.Button (new Rect (10, 10, 250, 100), "Play BGM"))
				SoundMgr.ins.PlayBgm ("Bgm_Battle1");

			if (GUI.Button (new Rect (310, 10, 250, 100), "Stop BGM"))
				SoundMgr.ins.StopBgm ();

			//2, Event
			if (GUI.Button (new Rect (10, 130, 250, 100), "Play Event\n(Battle_Win)"))
				SoundMgr.ins.PlayEvt ("BattleWin");

			if (GUI.Button (new Rect (310, 130, 250, 100), "Stop Evt"))
				SoundMgr.ins.StopEvts ();

			//3, UI
			if (GUI.Button (new Rect (10, 250, 250, 100), "Play UI"))
				SoundMgr.ins.PlayUI ("ButtonClick");
		}
	}
}