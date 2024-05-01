using LowoUN.Util;
using UnityEngine;

public class Sample_TimeWatcher : MonoBehaviour {
	[Header ("Test1_毫秒延迟_不循环")][SerializeField] private uint milliseconds_1 = 5000;
	[Header ("Test2_毫秒延迟_循环")][SerializeField] private uint milliseconds_2 = 3000;
	[Header ("Test3_毫秒延迟_循环")][SerializeField] private uint milliseconds_3 = 1000;
	[Header ("Test3_毫秒延迟_有限次数循环")][SerializeField] private uint numOfTimes = 5;
	[Header ("Test4+5_指定时间点_24小时制")][SerializeField] private string dateTime_specificTime = "13:00:00";

	private void OnTest1 () {
		Debug.Log ($"[test1] run {milliseconds_1/1000}'s later once");
	}

	private void OnTest2 () {
		Debug.Log ($"[test2] run every {milliseconds_2/1000}'s");
	}

	private void OnTest3 () {
		Debug.Log ($"[test3] run every {milliseconds_3/1000}'s for {numOfTimes} timers");
	}

	private void OnTest4 () {
		Debug.Log ($"[test4] run at {dateTime_specificTime}");
	}

	private void OnTest5 () {
		Debug.Log ($"[test5] run at {dateTime_specificTime} everyday!");
	}

	void OnGUI () {
		GUI.skin.button.fontSize = 20;
		GUI.skin.button.alignment = TextAnchor.MiddleLeft;


		if (GUI.Button (new Rect (30, 5, 240, 40), "AddWatcher Test01"))
			TimeWatcher.Instance.AddWatcher_Once ("test1", milliseconds_1, OnTest1);

		if (GUI.Button (new Rect (30, 55, 240, 40), "AddWatcher Test02"))
			TimeWatcher.Instance.AddWatcher_Loop ("test2", milliseconds_2, OnTest2);

		if (GUI.Button (new Rect (30, 105, 240, 40), "AddWatcher Test03"))
			TimeWatcher.Instance.AddWatcher_Multi ("test3", milliseconds_3, numOfTimes, OnTest3);

		if (GUI.Button (new Rect (30, 155, 240, 40), "AddWatcher Test04"))
			TimeWatcher.Instance.AddWatcher_DateTime ("test4", dateTime_specificTime, false, OnTest4);

		if (GUI.Button (new Rect (30, 205, 240, 40), "AddWatcher Test05"))
			TimeWatcher.Instance.AddWatcher_DateTime ("test5", dateTime_specificTime, true, OnTest5);

		if (GUI.Button (new Rect (330, 5, 240, 40), "RemoveWatcher Test01"))
			TimeWatcher.Instance.RemoveWatcher ("test1");

		if (GUI.Button (new Rect (330, 55, 240, 40), "RemoveWatcher Test02"))
			TimeWatcher.Instance.RemoveWatcher ("test2");

		if (GUI.Button (new Rect (330, 105, 240, 40), "RemoveWatcher Test03"))
			TimeWatcher.Instance.RemoveWatcher ("test3");

		if (GUI.Button (new Rect (330, 155, 240, 40), "RemoveWatcher Test04"))
			TimeWatcher.Instance.RemoveWatcher ("test4");

		if (GUI.Button (new Rect (330, 205, 240, 40), "RemoveWatcher Test05"))
			TimeWatcher.Instance.RemoveWatcher ("test5");
	}
}