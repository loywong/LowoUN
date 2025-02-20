using LowoUN.Sound;
using LowoUN.Util;
using UnityEngine;

public class Sample_Sound : MonoBehaviour {
	void OnGUI () {
		//1, BGM
		GUI.skin.button.fontSize = 30;

		if (GUI.Button (new Rect (10, 10, 250, 100), "Play BGM"))
			SoundManager.Instance.PlayBgm ("Bgm_Battle1");

		if (GUI.Button (new Rect (310, 10, 250, 100), "Stop BGM"))
			SoundManager.Instance.StopBgm ();

		//2, Event
		if (GUI.Button (new Rect (10, 130, 250, 100), "Play Event\n(Battle_Win)"))
			SoundManager.Instance.PlayEvt ("BattleWin");

		if (GUI.Button (new Rect (310, 130, 250, 100), "Stop Evt"))
			SoundManager.Instance.StopEvts ();

		//3, UI
		if (GUI.Button (new Rect (10, 250, 250, 100), "Play UI"))
			SoundManager.Instance.PlayUI ("ButtonClick");
	}
}