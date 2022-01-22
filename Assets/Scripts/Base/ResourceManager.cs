using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mega
{
    public class ResourceManager : GameComponent
    {
        #region 资源

        private Dictionary<string, GameObject> dicUI = new Dictionary<string, GameObject>();

        #endregion

        private AssetBundleLoader assetBundleLoader = new AssetBundleLoader();

        #region 加载资源

        public void LoadCommonResource(Action callback)
        {
#if UNITY_WEBGL
            return ;
#endif
            StartCoroutine(LoadCommonResAsync(callback));
        }

        public IEnumerator LoadCommonResAsync(Action callback)
        {
            yield return this.LoadData();

            //执行回调
            if (callback != null)
            {
                callback.Invoke();
            }
        }

        public void LoadGameRes()
        {
        }

        IEnumerator LoadData()
        {
            Framework.Table.GetTable<HeroData>();
            yield return new WaitForEndOfFrame();
            Framework.Table.GetTable<L18nManager>();
            yield return new WaitForEndOfFrame();
        }

        private T LoadRes<T>(string path) where T : UnityEngine.Object
        {
            T obj = Resources.Load<T>(path);
            return obj;
        }

        #endregion

        #region 获取资源

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

        #endregion

        #region 卸载资源

        public void UnLoadRes(SceneType scene)
        {
            dicUI.Clear();
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public void OnDestroy()
        {
            this.dicUI = null;
        }

        #endregion
    }
}