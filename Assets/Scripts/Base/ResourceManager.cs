using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mega
{
    public class ResourceManager : GameComponent
    {
        private Dictionary<string, GameObject> dicUI = new Dictionary<string, GameObject>();

        public GameObject GetUIPrefab(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debuger.LogError("UI path is null!");
                return null;
            }

            GameObject prefabUI;
            if (dicUI.ContainsKey(path))
            {
                prefabUI = dicUI[path];
            }
            else
            {
                prefabUI = LoadRes<GameObject>(path);
                if (prefabUI == null)
                {
                    Debuger.LogError("资源本地不存在! 路径:" + path);
                    return null;
                }

                dicUI.Add(path, prefabUI);
            }

            return prefabUI;
        }

        private T LoadRes<T>(string path) where T : UnityEngine.Object
        {
            T obj = Resources.Load<T>(path);
            return obj;
        }

        public void UnLoadAll()
        {
            dicUI.Clear();
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
    }
}