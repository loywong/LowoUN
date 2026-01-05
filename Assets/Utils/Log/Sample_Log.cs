//注意: 需要添加脚本编译宏 PROJECT_LOG

using LowoUN.Util;
using UnityEngine;

public class Sample_Log : MonoBehaviour {
	[SerializeField] bool isDebug;
	void Start () {
		Log.Init (isDebug);

		Log.Warn("some warning log");
		Log.Error("some error log");

		Log.Blue("data","log data");
		Log.Gray("asset","log asset");
		Log.Gray("ui","log ui");
		Log.Green("scene","log scene");
		Log.Green("flow","log flow");
		Log.Trace("util","log util");
		Log.Orange("test","log test");

		Log.Trace("map","log map");
		Log.Trace("battle","log battle");
		Log.Trace("nav","log nav");
	}
}