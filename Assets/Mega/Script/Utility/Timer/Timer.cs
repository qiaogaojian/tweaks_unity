using UnityEngine;
using System;

namespace Mega
{
    public class Timer : IPooledObjSupporter
    {
        private bool  b_Tricking;
        private float f_CurTime;
        private float f_TriggerTime;
        private int   _groupId = 0;


        public        TimerTickCallback onTimerTick;
        private       string            name;
        public static int               Seq;

        public Timer()
        {
            f_CurTime     = 0.0f;
            this._groupId = 0;
        }

        public Timer(float second)
        {
            f_CurTime     = 0.0f;
            f_TriggerTime = second;
            this._groupId = 0;
            name          = "Timer-" + (++Seq);
        }

        public Timer(float second, TimerTickCallback callback)
        {
            f_CurTime     = 0.0f;
            f_TriggerTime = second;
            onTimerTick   = callback;
            this._groupId = 0;
            name          = "Timer-" + (++Seq);
        }

        public Timer(float second, TimerTickCallback callback, int groupId)
        {
            f_CurTime     = 0.0f;
            f_TriggerTime = second;
            onTimerTick   = callback;
            this._groupId = groupId;
            name          = "Timer-" + (++Seq);
        }

        public void OnUpdate()
        {
            if (!b_Tricking)
            {
                return;
            }

            f_CurTime += Time.deltaTime;
            if (f_CurTime > f_TriggerTime)
            {
                f_CurTime = 0;
//			b_Tricking = false;
                if (onTimerTick != null)
                {
                    try
                    {
                        onTimerTick(this);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                        Stop();
//					onTimerTick = null;
                        Debuger.Log("Timer stopped");
                    }
                }
//			b_Tricking = true;
            }
        }

        public void Stop()
        {
            Running   = false;
            f_CurTime = 0;
        }

        public void Restart()
        {
            Running   = true;
            f_CurTime = 0.0f;
        }

        public void ResetTriggerTime(float second)
        {
            TriggerTime = second;
            Restart();
        }

        public bool Running
        {
            set { b_Tricking = value; }
            get { return this.b_Tricking; }
        }

        public void Reset()
        {
            Stop();
            onTimerTick = null;
            _groupId    = 0;
        }

        public void Dispose()
        {
            onTimerTick = null;
            _groupId    = 0;
        }

        public void Destory()
        {
            Stop();
            TimerManager.Instance.Dispose(this);
        }

        public float TriggerTime
        {
            get { return this.f_TriggerTime; }
            set { f_TriggerTime = value; }
        }

        public float CurTime
        {
            get { return this.f_CurTime; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }
    }
}