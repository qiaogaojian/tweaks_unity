//**********************************************************************
//#Author:  Michael
//#Time:    2018/7/3
//**********************************************************************
//#Func: 
//
// 继承自MonoBehaviour的单例模板.适用于整个游戏生命周期都存在的管理器. 单例模式不允许New
//
// 这是个MonoBehaviour类型的单例模板,可以使用协程,参考自 http://wiki.unity3d.com/index.php/Singleton  
// MonoSingleton生命周期由其自身管理,除了全局管理类,游戏中的局部功能的脚本禁止使用MonoSingleton,需要单例功能时使用Singleton + MonoBehaviour 代替
// 一定不要在OnDestroy函数中直接访问Mono单例模式！这样会使Unity在Editor下生成一份单例,造成一些莫名其妙的bug
//
// 例子:
//     public class Manager : MonoSingleton<Manager>
//     {         
//         public string myGlobalVar = "whatever";
//     }
//     public class MyClass : MonoBehaviour 
//     {
//         void Awake () 
//         {
//             Debuger.Log(Manager.Instance.myGlobalVar);
//         }
//     }
// 
//**********************************************************************

using UnityEngine;

namespace Mega
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        private static readonly object syslock = new object();

        private static bool applicationIsQuitting = false;

        public static T Instance
        {
            get { return CreateInstance(); }
        }

        public static T CreateInstance()
        {
            if (applicationIsQuitting)
            {
                Debuger.LogWarning("MonoSingleton Instance " + typeof(T) + "在程序退出时已销毁");
                return null;
            }

            lock (syslock)
            {
                if (instance == null)
                {
                    instance = (T) FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debuger.LogError("MonoSingleton Instance " + typeof(T) + "的实例超过1个");
                        return instance;
                    }

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance       = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString() + "(MonoSingleton)";
                        DontDestroyOnLoad(singleton);
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public void OnDestroy()
        {
            if (MonoSingleton<T>.instance != null)
            {
                (MonoSingleton<T>.instance as MonoSingleton<T>).UnInit();
                MonoSingleton<T>.instance = (T) ((object) null);
            }

            applicationIsQuitting = true;
        }

        public virtual void Init()
        {
            Debuger.Log("MonoSingleton Init():" + typeof(T).ToString());
        }

        //UnInit 的生命周期是自己控制自己的
        protected virtual void UnInit()
        {
            Debuger.Log("MonoSingleton UnInit():" + typeof(T).ToString());
        }
    }
}