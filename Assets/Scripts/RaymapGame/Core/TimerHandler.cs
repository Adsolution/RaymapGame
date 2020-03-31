//================================
//  By: Adsolution
//================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaymapGame {
    public class TimerHandler : MonoBehaviour {
        void FixedUpdate() {
            for (int ti = 0; ti < Timer.timers.Count; ti++) {
                var t = Timer.timers[ti];

                if (t.onFinish) {
                    t.onFinish = false;
                    Timer.timers.Remove(t);
                    continue;
                }
                if (t.active) {
                    t.frame++;
                    if (t.onUpdateAction != null)
                        t.onUpdateAction.Invoke();

                    if (t.remaining <= 0) {
                        t.active = false;
                        t.finished = true;
                        t.onFinish = true;
                        if (t.onFinishAction != null)
                            t.onFinishAction.Invoke();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Convenient class for triggering actions with delays.
    /// </summary>
    public class Timer {
        public static List<Timer> timers = new List<Timer>();

        float t_start, t_end;
        public bool active, finished, onFinish;
        public Action onFinishAction, onUpdateAction;
        public float remaining => t_end - Time.fixedTime;
        public float elapsed => Time.fixedTime - t_start;
        public int frame;

        /// <summary>
        /// Sets Timer.active to True for the duration in seconds. After the duration, finished is set to true, and onFinish is True for one frame.
        /// </summary>
        /// <param name="seconds">Duration in seconds.</param>
        public void Start(float seconds, bool reset = true) {
            if (active && !reset) return;
            active = true;
            finished = false;
            frame = 0;
            t_start = Time.fixedTime;
            t_end = t_start + seconds;
            timers.Add(this);
        }

        /// <summary>
        /// Sets Timer.active to True for the duration in seconds. After the duration, onFinishAction is invoked, finished is set to true, and onFinish is True for one frame.
        /// </summary>
        /// <param name="seconds">Duration in seconds.</param>
        /// <param name="onFinishAction">Invoked after the duration.</param>
        public void Start(float seconds, Action onFinishAction, bool reset = true) {
            Start(seconds, reset);
            this.onFinishAction = onFinishAction;
        }
        public void Start(float seconds, Action onUpdateAction, Action onFinishAction) {
            Start(seconds, false);
            this.onUpdateAction = onUpdateAction;
            this.onFinishAction = onFinishAction;
        }

        public void Set(float seconds, bool reset = true) => Start(seconds, reset);
        public void Set(float seconds, Action onFinishAction, bool reset = true) => Start(seconds, onFinishAction, reset);

        public void SetUpdateAction(Action onUpdateAction) {
            this.onUpdateAction = onUpdateAction;
        }

        public void Abort() {
            if (timers.Contains(this))
                timers.Remove(this);
            finished = false;
            active = false;
        }

        public Timer() => Init(null, null);
        public Timer(Action onFinishAction) => Init(onFinishAction, null);
        public Timer(Action onUpdateAction, Action onFinishAction) => Init(onUpdateAction, onFinishAction);
        void Init(Action onUpdateAction, Action onFinishAction) {
            this.onUpdateAction = onUpdateAction;
            this.onFinishAction = onFinishAction;
            timers.Add(this);
        }

        /// <summary>
        /// Invokes onFinishAction after a delay in seconds.
        /// </summary>
        /// <param name="seconds">Delay in seconds.</param>
        /// <param name="onFinishAction">Action to invoke after delay.</param>
        /// <returns>Returns the timer created to process the delay.</returns>
        public static Timer StartNew(float seconds, Action onFinishAction) {
            var t = new Timer();
            t.Start(seconds, onFinishAction);
            return t;
        }
        public static Timer StartNew(float seconds, Action onUpdateAction, Action onFinishAction) {
            var t = new Timer();
            t.Start(seconds, onUpdateAction, onFinishAction);
            return t;
        }
    }
}