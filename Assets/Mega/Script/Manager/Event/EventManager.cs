//**********************************************************************
//#Author:  Michael
//#Time:    2018/7/9
//**********************************************************************
//#Func: 
//
// 事件管理器组件
// 
// 1. 使用事件之前 先声明相应的 EventId 枚举
// 2. 事件有注册必须要有注销  成对出现 不然会出现不可预知的bug                                                                             
//
//**********************************************************************

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mega
{
    public class EventManager : GameComponent
    {
        private Dictionary<int, Delegate> _dicEvents = new Dictionary<int, Delegate>();


        #region 检查

        private void LogTypeError(EventId eventId, HandleType handleType, Delegate targetEventType, Delegate listener)
        {
            Debuger.LogError(string.Format("## Event Id {0}, [{1}] Wrong Listener Type {2}, needed Type {3}.", eventId.ToString(),
                EventSystemDefine.dicHandleType[(int) handleType],
                targetEventType.GetType(),
                listener.GetType()));
        }

        private bool CheckAddEventListener(EventId eventId, Delegate listener)
        {
            if (!this._dicEvents.ContainsKey((int) eventId))
            {
                this._dicEvents.Add((int) eventId, null);
            }

            Delegate tmDelegate = this._dicEvents[(int) eventId];

            if (tmDelegate != null && tmDelegate.GetType() != listener.GetType())
            {
                LogTypeError(eventId, HandleType.Add, _dicEvents[(int) eventId], listener);
                return false;
            }

            return true;
        }

        private bool CheckRemoveEventListener(EventId eventId, Delegate listener)
        {
            if (!_dicEvents.ContainsKey((int) eventId))
                return false;

            Delegate tmpDel = _dicEvents[(int) eventId];
            if (tmpDel != null && tmpDel.GetType() != listener.GetType())
            {
                LogTypeError(eventId, HandleType.Remove, _dicEvents[(int) eventId], listener);
                return false;
            }

            return true;
        }

        #endregion


        #region 无参事件

        public void AddEventListener(EventId eventId, Action listener)
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = this._dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = (Action) Delegate.Combine((Action) del, listener);
            }
        }

        public void RemoveEventListener(EventId eventId, Action listener)
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = Delegate.Remove(del, listener);
            }
        }

        public void SendEvent(EventId eventId)
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int) eventId, out del))
            {
                if (del == null)
                    return;

                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    Action action = invocationList[i] as Action;
                    if (action == null)
                    {
                        Debuger.LogError(string.Format("## Trigger Event {0} Parameters type [void] are not match  target type : {1}.", eventId.ToString(), invocationList[i].GetType()));
                        return;
                    }

                    action();
                }
            }
        }

        #endregion


        #region 一个参数事件

        public void AddEventListener<T>(EventId eventId, Action<T> listener)
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = (Action<T>) Delegate.Combine((Action<T>) del, listener);
            }
        }

        public void RemoveEventListener<T>(EventId eventId, Action<T> listener)
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = Delegate.Remove(del, listener);
            }
        }

        public void SendEvent<T>(EventId eventId, T p)
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int) eventId, out del))
            {
                if (del == null)
                    return;

                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    Action<T> action = invocationList[i] as Action<T>;
                    if (action == null)
                    {
                        Debuger.LogError(string.Format("## Trigger Event {0} Parameters type [ {1} ] are not match  target type : {2}. ",
                            eventId.ToString(),
                            p.GetType(),
                            invocationList[i].GetType()));
                        return;
                    }

                    action(p);
                }
            }
        }

        #endregion


        #region 两个参数事件

        public void AddEventListener<T0, T1>(EventId eventId, Action<T0, T1> listener)
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = (Action<T0, T1>) Delegate.Combine((Action<T0, T1>) del, listener);
            }
        }

        public void RemoveEventListener<T0, T1>(EventId eventId, Action<T0, T1> listener)
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = Delegate.Remove(del, listener);
            }
        }

        public void SendEvent<T0, T1>(EventId eventId, T0 p0, T1 p1)
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int) eventId, out del))
            {
                if (del == null)
                    return;

                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    Action<T0, T1> action = invocationList[i] as Action<T0, T1>;
                    if (action == null)
                    {
                        Debuger.LogError(string.Format("## Trigger Event {0} Parameters type [ {1}, {2}] are not match  target type : {3}.",
                            eventId.ToString(),
                            p0.GetType(),
                            p1.GetType(),
                            invocationList[i].GetType()));
                        return;
                    }

                    action(p0, p1);
                }
            }
        }

        #endregion


        #region 三个参数事件

        public void AddEventListener<T0, T1, T2>(EventId eventId, Action<T0, T1, T2> listener)
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = (Action<T0, T1, T2>) Delegate.Combine((Action<T0, T1, T2>) del, listener);
            }
        }

        public void RemoveEventListener<T0, T1, T2>(EventId eventId, Action<T0, T1, T2> listener)
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = Delegate.Remove(del, listener);
            }
        }

        public void SendEvent<T0, T1, T2>(EventId eventId, T0 p0, T1 p1, T2 p2)
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int) eventId, out del))
            {
                if (del == null)
                    return;
                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    Action<T0, T1, T2> action = invocationList[i] as Action<T0, T1, T2>;
                    if (action == null)
                    {
                        Debuger.LogError(string.Format("## Trigger Event {0} Parameters type [{1}, {2}, {3}] are not match  target type : {4}.",
                            eventId.ToString(),
                            p0.GetType(),
                            p1.GetType(),
                            p2.GetType(),
                            invocationList[i].GetType()));
                        return;
                    }

                    action(p0, p1, p2);
                }
            }
        }

        #endregion
    }
}