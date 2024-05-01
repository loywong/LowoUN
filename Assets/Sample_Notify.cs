using LowoUN.Util;
using UnityEngine;

public class Sample_Notify : MonoBehaviour {
    void Start () {
        AddListener ();

        BroadcastMsg ();

        Removelistener ();

        BroadcastMsg ();
    }

    private void AddListener () {
        NotifyManager.AddListener ("EventName1", TestEventCall);
        NotifyManager.AddListener<float> ("EventName2", TestEventCallFloat);
        NotifyManager.AddListener<int, float> ("EventName3", TestEventCallIntFloat);
    }

    private void BroadcastMsg () {
        NotifyManager.Broadcast ("EventName1");
        NotifyManager.Broadcast<float> ("EventName2", 1.0f);
        NotifyManager.Broadcast<int, float> ("EventName3", 1, 1.0f);
    }

    private void Removelistener () {
        NotifyManager.RemoveListener ("EventName1", TestEventCall);
        NotifyManager.RemoveListener<float> ("EventName2", TestEventCallFloat);
        NotifyManager.RemoveListener<int, float> ("EventName3", TestEventCallIntFloat);
    }

    private void TestEventCall () {
        Debug.Log ("get the register event call");
    }

    private void TestEventCallFloat (float value) {
        Debug.Log ($"get the event call value: {string.Format ("{0:f2}", value)}");
        if (value != 1.0f) {
            Debug.LogError ("Unit test failure - wrong value on float argument");
        }
    }

    private void TestEventCallIntFloat (int intvalue, float floatvalue) {
        Debug.Log ($"get the event call value: {intvalue}, floatvalue: {string.Format ("{0:f2}", floatvalue)}");
    }
}