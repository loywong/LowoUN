using System;
using LowoUN.Util;
using UnityEngine;

public class Sample_TimeWatcher : MonoBehaviour {
	[Header ("毫秒延迟_不循环")][SerializeField] private uint milliseconds_1 = 5000;
	[Header ("毫秒延迟_循环")][SerializeField] private uint milliseconds_2 = 3000;
	[Header ("指定时间点_24小时制")][SerializeField] private string dateTime_specificTime = "13:00:00";

	void Start () {
		Debug.Log ("Test Start!");

		TimeWatcher.Instance.AddWatcher ("test01", milliseconds_1, false, OnTest01);
		TimeWatcher.Instance.AddWatcher ("test02", milliseconds_2, true, OnTest02);

		var timeString = DateTime.Now.Date.ToShortDateString () + " " + dateTime_specificTime;
		Debug.Log ($"timeString {timeString}");
		TimeWatcher.Instance.AddWatcher ("test03", DateTime.Parse (timeString), false, OnTest03);
		TimeWatcher.Instance.AddWatcher ("test04", DateTime.Parse (timeString), true, OnTest04);
	}

	private void OnTest01 () {
		Debug.Log ("[test01] run 5's later once");
	}

	private void OnTest02 () {
		Debug.Log ("[test02] run every 3's");
	}

	private void OnTest03 () {
		Debug.Log ($"[test03] run at {dateTime_specificTime}");
	}

	private void OnTest04 () {
		Debug.Log ($"[test04] run at {dateTime_specificTime} everyday!");
	}

	void OnGUI () {
		GUI.skin.button.fontSize = 20;
		GUI.skin.button.alignment = TextAnchor.MiddleLeft;

		if (GUI.Button (new Rect (30, 5, 240, 40), "RemoveWatcher Test01"))
			TimeWatcher.Instance.RemoveWatcher ("test01");

		if (GUI.Button (new Rect (30, 55, 240, 40), "RemoveWatcher Test02"))
			TimeWatcher.Instance.RemoveWatcher ("test02");

		if (GUI.Button (new Rect (30, 105, 240, 40), "RemoveWatcher Test03"))
			TimeWatcher.Instance.RemoveWatcher ("test03");

		if (GUI.Button (new Rect (30, 155, 240, 40), "RemoveWatcher Test04"))
			TimeWatcher.Instance.RemoveWatcher ("test04");
	}
}