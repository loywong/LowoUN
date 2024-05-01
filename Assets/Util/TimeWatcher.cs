using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace LowoUN.Util {
    public class TimeWatcher : ManagerMono<TimeWatcher> {
        private Dictionary<string, WatchData> name2watch = new Dictionary<string, WatchData> ();
        private Dictionary<string, Action> name2action = new Dictionary<string, Action> ();
        private Queue<Action> cbqueue = new Queue<Action> ();

        public void AddWatcher_Once (string name, uint mseconds, Action callback, bool isTimeScaleAff = true) {
            AddWatcher (name, mseconds, false, callback);
        }
        public void AddWatcher_Loop (string name, uint mseconds, Action callback, bool isTimeScaleAff = true) {
            AddWatcher (name, mseconds, true, callback);
        }
        public void AddWatcher_Multi (string name, uint mseconds, uint times, Action callback, bool isTimeScaleAff = true) {
            AddWatcher (name, mseconds, times, callback);
        }
        public void AddWatcher_DateTime (string name, string dateTime_specificTime, bool loop, Action callback, bool isTimeScaleAff = true) {
            var timeString = DateTime.Now.Date.ToShortDateString () + " " + dateTime_specificTime;
            Debug.Log ($"timeString {timeString}");
            AddWatcher ("test4", DateTime.Parse (timeString), loop, callback);
        }

        /// <summary>
        /// add delay event
        /// </summary>
        /// <param name="name">uniqueue name for delay event</param>
        /// <param name="mseconds">milliseconds for delay</param>
        /// <param name="loop">repeat or not</param>
        private void AddWatcher (string name, uint mseconds, bool loop, Action callback, bool isTimeScaleAff = true) {
            if (mseconds <= 0) {
                Debug.LogWarning ("watch time can not be less or equal 0");
                return;
            }

            if (name2watch.ContainsKey (name)) {
                Debug.LogWarning ($"This name {name} of timer has exist!");
                return;
            }

            uint ms = isTimeScaleAff ? (uint) ((float) mseconds * (1f / Time.timeScale)) : mseconds;
            if (ms <= 0)
                return;

            name2watch[name] = new WatchData (name, ms, loop);
            name2action[name] = callback;
            name2watch[name].Start ();
        }

        // times: repeat for fixed number of times
        private void AddWatcher (string name, uint mseconds, uint times, Action callback, bool isTimeScaleAff = true) {
            if (times <= 1) {
                Debug.LogWarning ("times must be more then 1");
                return;
            }

            if (mseconds <= 0) {
                Debug.LogWarning ("watch time can not be less or equal 0");
                return;
            }

            if (name2watch.ContainsKey (name)) {
                Debug.LogWarning ($"This name {name} of timer has exist!");
                return;
            }

            uint ms = isTimeScaleAff ? (uint) ((float) mseconds * (1f / Time.timeScale)) : mseconds;
            if (ms <= 0)
                return;

            name2watch[name] = new WatchData (name, ms, times);
            name2action[name] = callback;
            name2watch[name].Start ();
        }

        /// <summary>
        /// add specific time event
        /// </summary>
        /// <param name="name">uniqueue name for each delay event</param>
        /// <param name="time">specific time</param>
        /// <param name="loop">repeat ot not, if true, every 24 houra work!</param>
        private void AddWatcher (string name, DateTime time, bool loop, Action callback) {
            if (name2watch.ContainsKey (name)) {
                Debug.LogWarning ($"This name {name} of timer is exist!");
                return;
            }

            var w = new WatchData (name, time, loop);
            name2watch[name] = w;
            name2action[name] = callback;

            w.Start ();
        }

        // /// <summary>
        // /// add specific time event with time string
        // /// </summary>
        // /// <param name="name">uniqueue name for each delay event</param>
        // /// <param name="time">specific time string include hour, minute, second, eg."18:00:00"</param>
        // /// <param name="loop">repeat or not</param>
        // /// <param name="callback"></param>
        // public void AddWatcher (string name, string time, bool loop, Action callback) {
        //     AddWatcher (name, DateTime.Parse (time), loop, callback);
        // }

        public void RemoveWatcher (string name) {
            if (!name2watch.ContainsKey (name)) {
                Debug.Log ($"RemoveWatcher has no event key:{name}");
                return;
            }

            name2watch[name].End ();
            name2watch.Remove (name);
            name2action.Remove (name);
            Debug.Log ($"RemoveWatcher event key: {name}, succ!");
        }

        public void OnCallback (string name) {
            if (!name2action.ContainsKey (name))
                return;

            cbqueue.Enqueue (name2action[name]);
            if (!name2watch[name].loop || name2watch[name].loopLimitAndComplete) {
                RemoveWatcher (name);
            }
        }

        void Update () {
            if (cbqueue.Count > 0) {
                Action action = cbqueue.Dequeue ();
                action?.Invoke ();
            }
        }

        void OnApplicationQuit () {
            foreach (var kvp in name2watch) {
                kvp.Value.End ();
            }

            name2watch.Clear ();
            name2action.Clear ();
            cbqueue.Clear ();
        }

        // Check time event is exist
        public bool ContainKey (string key) {
            if (key == null) {
                Debug.LogWarning ("Invalid key:null");
                return false;
            }

            return name2watch.ContainsKey (key);
        }
    }

    public class WatchData {
        private string name;
        private Timer timer;
        public bool loop { private set; get; }
        private bool isForDateTime;

        // limit times loop
        private bool loopLimit { get { return times > 1; } }
        public bool loopLimitAndComplete { get { return loopLimit && curTimes >= times; } }
        private uint times;
        private uint mseconds;
        private uint curTimes;

        public WatchData (string name, uint mseconds, bool loop) {
            isForDateTime = false;

            this.name = name;
            this.loop = loop;

            timer = new Timer (mseconds);
            timer.Elapsed += OnElapsed;
            timer.AutoReset = loop;
        }

        public WatchData (string name, uint mseconds, uint times) {
            isForDateTime = false;

            this.name = name;
            this.loop = true;
            this.times = times;
            this.mseconds = mseconds;

            timer = new Timer (mseconds);
            timer.Elapsed += OnElapsed;
            timer.AutoReset = loop;

            curTimes = 0;
        }

        public WatchData (string name, DateTime time, bool loop) {
            isForDateTime = true;

            this.name = name;
            this.loop = loop;

            TimeSpan span = time - DateTime.Now;
            if (span.TotalMilliseconds > 0) {
                timer = new Timer (span.TotalMilliseconds);
            } else {
                span = time.AddDays (1) - DateTime.Now;
                timer = new Timer (span.TotalMilliseconds);
            }
            timer.Elapsed += OnElapsed;
            timer.AutoReset = false;
        }

        private void OnElapsed (object sender, ElapsedEventArgs e) {
            if (!isForDateTime) {
                if (loopLimit) {
                    curTimes += 1;
                    if (curTimes >= times) {
                        timer.Stop ();
                        TimeWatcher.Instance.OnCallback (name);
                    } else {
                        TimeWatcher.Instance.OnCallback (name);
                        timer.Interval = mseconds;
                        timer.Start ();
                    }
                } else {
                    if (!loop)
                        timer.Stop ();
                    TimeWatcher.Instance.OnCallback (name);
                }
            } else {
                timer.Stop ();
                TimeWatcher.Instance.OnCallback (name);

                if (loop) {
                    timer.Interval = new TimeSpan (24, 0, 0).Milliseconds;
                    timer.Start ();
                }
            }
        }

        public void Start () {
            timer.Start ();
        }

        public void Stop () {
            timer.Stop ();
        }

        public void End () {
            timer.Stop ();
            timer.Elapsed -= OnElapsed;
            timer.Dispose ();
        }
    }
}