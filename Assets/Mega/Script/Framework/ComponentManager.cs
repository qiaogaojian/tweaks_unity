using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mega
{
    public class ComponentManager
    {
        private static readonly List<GameComponent> frameworkComponents = new List<GameComponent>();

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架组件类型。</typeparam>
        /// <returns>要获取的游戏框架组件。</returns>
        public static T GetComponent<T>() where T : GameComponent
        {
            return (T) GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <param name="type">要获取的游戏框架组件类型。</param>
        /// <returns>要获取的游戏框架组件。</returns>
        public static GameComponent GetComponent(Type type)
        {
            foreach (var t in frameworkComponents)
            {
                if (t.GetType() == type)
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <param name="typeName">要获取的游戏框架组件类型名称。</param>
        /// <returns>要获取的游戏框架组件。</returns>
        public static GameComponent GetComponent(string typeName)
        {
            for (var i = 0; i < frameworkComponents.Count; i++)
            {
                Type type = frameworkComponents[i].GetType();
                if (type.FullName == typeName || type.Name == typeName)
                {
                    return frameworkComponents[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 注册游戏框架组件。
        /// </summary>
        /// <param name="gameFrameworkComponent">要注册的游戏框架组件。</param>
        internal static void RegisterComponent(GameComponent gameFrameworkComponent)
        {
            if (gameFrameworkComponent == null)
            {
                Debuger.LogError("Game Framework component is invalid.");
                return;
            }

            Type typeComponent = gameFrameworkComponent.GetType();
            for (var i = 0; i < frameworkComponents.Count; i++)
            {
                Type type = frameworkComponents[i].GetType();
                if (type == typeComponent)
                {
                    Debuger.LogError(String.Format("Game Framework component type '{0}' is already exist.", type.FullName));
                    return;
                }
            }

            frameworkComponents.Add(gameFrameworkComponent);
        }
    }
}