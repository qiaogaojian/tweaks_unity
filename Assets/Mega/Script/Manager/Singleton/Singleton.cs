//**********************************************************************
//#Author:  Michael
//#Time:    2018/7/3
//**********************************************************************
//#Func: 
//
// 这个是C#原生单例模板类.适用于游戏数据管理器, 单例模式不允许New
//
// 例子:
//      public class Manager : Singleton<Manager>
//      {          
//          public string myGlobalVar = "whatever";
//      }
//
//      public class MyClass : MonoBehaviour 
//      {
//          void Awake () 
//          {
//              Debuger.Log(Manager.Instance.myGlobalVar);
//          }
//      }
// 
//**********************************************************************

using System;

namespace Mega
{
    public class Singleton<T> where T : class
    {
        private static T instance;

        private static readonly object syslock = new object();

        public static T Instance
        {
            get
            {
                if (Singleton<T>.instance == null)
                {
                    lock (syslock)
                    {
                        CreateInstance();
                    }
                }

                return Singleton<T>.instance;
            }
        }

        protected Singleton()
        {
        }

        public static void CreateInstance()
        {
            if (Singleton<T>.instance == null)
            {
                Singleton<T>.instance = Activator.CreateInstance<T>();
            }
        }

        public void DestroyInstance()
        {
            if (Singleton<T>.instance != null)
            {
                (Singleton<T>.instance as Singleton<T>).UnInit();
                Singleton<T>.instance = (T) ((object) null);
            }
        }

        public virtual void Init()
        {
            Debuger.Log("Singleton Init():" + typeof(T).ToString());
        }

        //UnInit()自己控制自己的生命周期
        protected virtual void UnInit()
        {
            Debuger.Log("Singleton UnInit():" + typeof(T).ToString());
        }
    }
}