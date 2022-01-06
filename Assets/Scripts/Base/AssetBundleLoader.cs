using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Mega
{
    #region AssetBundle内容实体类

    /// <summary>
    /// AssetBundle的内容实体类
    /// </summary>
    class AssetBundleContent
    {
        private string _assetBundleName;

        /// <summary>
        /// 内容字典
        /// key = 资源名称
        /// value = clone后的资源对象
        /// </summary>
        private Dictionary<string, Object> _contentDic = new Dictionary<string, Object>();

        public AssetBundleContent(string assetBundleName, AssetBundle bundle)
        {
            this._assetBundleName = assetBundleName;
            this.InitContent(bundle);
        }

        /// <summary>
        /// 初始化内容
        /// </summary>
        public void InitContent(AssetBundle assetBundle)
        {
            //string[] allAssetNames = assetBundle.GetAllAssetNames();
            //解压缩获得所有资源
            Object[] objects = assetBundle.LoadAllAssets();

            for (int i = 0, len = objects.Length; i < len; i++)
            {
                Object prefab = objects[i];
                Debuger.Log("AssetBundle内容名：" + prefab.name, Debuger.ColorType.magenta);
                //资源名称
                string name = prefab.name;

                //clone对象并存入字典
                Object one = Object.Instantiate(prefab);
                one.name               = name;
                this._contentDic[name] = one;
            }
        }

        /// <summary>
        /// 根据资源名称获得资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public Object GetAsset(string assetName)
        {
            if (this._contentDic.ContainsKey(assetName))
            {
                Object asset = this._contentDic[assetName];
                ;
                //Debuger.Log("获得预制资源：" +"assetName,类型" + asset.GetType());
                return asset;
            }

            return null;
        }

        public void ClearAll()
        {
            //方法1
            foreach (var content in _contentDic)
            {
                Object.DestroyImmediate(content.Value);
            }

            //方法2
            this._contentDic.Clear();
            this._contentDic = null;
        }
    }

    #endregion

    #region 场景包含AssetBundle实体类

    /// <summary>
    /// 场景包含AssetBundle实体类
    /// </summary>
    class AssetBundlesInScene
    {
        /// <summary>
        /// 场景中加载的assetbundle对象
        /// key = assetbundle名称
        /// value = assetbundle名称内容类
        /// </summary>
        private Dictionary<string, AssetBundleContent> _assetbundleDic = new Dictionary<string, AssetBundleContent>();


        #region 接口

        /// <summary>
        /// 新增一个AssetBundle
        /// </summary>
        /// <param name="key"></param>
        /// <param name="assetbundle"></param>
        public void AddAssetBundle(string key, AssetBundle assetbundle)
        {
            if (assetbundle == null)
            {
                Debuger.Log(key + "的AssetBundle为空", Debuger.ColorType.magenta);
                return;
            }

            if (this._assetbundleDic.ContainsKey(key))
            {
                _assetbundleDic.Remove(key);
            }

            //新建AssetBundleContent类
            AssetBundleContent assetBundleContent = new AssetBundleContent(key, assetbundle);
            _assetbundleDic[key] = assetBundleContent;
        }

        /// <summary>
        /// 清理操作
        /// </summary>
        public void ClearAll()
        {
            //方法1
            foreach (var ob in this._assetbundleDic)
            {
                ob.Value.ClearAll();
            }

            //方法2
            this._assetbundleDic.Clear();
            this._assetbundleDic = null;
        }

        /// <summary>
        /// 根据bundlename获得内容
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public AssetBundleContent GetAssetBundleContent(string bundleName)
        {
            if (this._assetbundleDic.ContainsKey(bundleName))
            {
                return _assetbundleDic[bundleName];
            }

            return null;
        }

        #endregion
    }

    #endregion

    /// <summary>
    /// Author:      崔丽阳
    /// CreateTime:  2018/1/27 11:17:11
    /// 主要功能：AssetBundle资源加载类
    /// </summary>
    public class AssetBundleLoader
    {
        //与场景绑定的AssetBundle资源字典
        private Dictionary<SceneType, AssetBundlesInScene> _assetBundlesInScenes = new Dictionary<SceneType, AssetBundlesInScene>();

        #region 加载AssetBundle

        /// <summary>
        /// 同步方式预加载AssetBundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="sceneType">默认None，表示跨场景</param>
        //    public void PreLoadAssetBundleSync(string bundleName,SceneType sceneType)
        //    {
        //       //拼接Bundle路径
        //        string path = System.IO.Path.Combine(Application.temporaryCachePath, "AssetBundle");
        //#if UNITY_ANDROID
        //        path = System.IO.Path.Combine(path, "Android");
        //#elif UNITY_IOS
        //        path = System.IO.Path.Combine(path, "iOS");
        //#endif
        //        path = System.IO.Path.Combine(path, bundleName);

        //        //加载AssetBundle
        //        var assetbundle = AssetBundle.LoadFromFile(path);
        //        //解压缩并存储AssetBundle资源
        //        AssetBundlesInScene assetBundlesInScene = GetAssetBundlesInScene(sceneType);
        //        assetBundlesInScene.AddAssetBundle(bundleName,assetbundle);
        //        Debuger.Log("同步加载AssetBundle:" + bundleName + " 成功");
        //        //卸载AssetBundle
        //        assetbundle.Unload(false);
        //    }

        /// <summary>
        /// 异步方式预加载AssetBundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="sceneType"></param>
        public IEnumerator PreLoadAssetBundleAsync(string bundleName, SceneType sceneType)
        {
            //方法二
            string relativePath = "AssetBundle";
#if UNITY_ANDROID
            relativePath = System.IO.Path.Combine(relativePath, "Android");
#elif UNITY_IOS
        relativePath = System.IO.Path.Combine(relativePath, "iOS");
#endif
            relativePath = System.IO.Path.Combine(relativePath, bundleName);

            string uri = FileUtils.GetRealUri(relativePath, PathMode.Streaming);
            Debuger.Log("异步预加载预制路径uri:" + uri);
            WWW www = new WWW(uri);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debuger.Log("WWW download had an error:" + www.error);
                yield break;
            }

            AssetBundle assetbundle = www.assetBundle;
            if (assetbundle == null)
            {
                Debuger.Log(" AssetBundle = null");
                yield break;
            }

            //解压缩并存储AssetBundle资源
            AssetBundlesInScene assetBundlesInScene = GetAssetBundlesInScene(sceneType);
            assetBundlesInScene.AddAssetBundle(bundleName, assetbundle);

            Debuger.Log("异步加载AssetBundle:" + bundleName + " 成功");
            //卸载AssetBundle
            assetbundle.Unload(false);

            www.Dispose();
        }


        AssetBundlesInScene GetAssetBundlesInScene(SceneType sceneType)
        {
            if (!this._assetBundlesInScenes.ContainsKey(sceneType))
            {
                this._assetBundlesInScenes[sceneType] = new AssetBundlesInScene();
            }

            return this._assetBundlesInScenes[sceneType];
        }

        #endregion


        #region 卸载AssetBundle

        /// <summary>
        /// 卸载场景关联的AssetBundle
        /// </summary>
        /// <param name="sceneType"></param>
        public void UnLoadAssetBundle(SceneType sceneType)
        {
            AssetBundlesInScene assetBundlesInScene = GetAssetBundlesInScene(sceneType);
            assetBundlesInScene.ClearAll();
            this._assetBundlesInScenes.Remove(sceneType);
            Debuger.Log("卸载" + sceneType.ToString() + "场景的AssetBundle", Debuger.ColorType.magenta);
        }

        #endregion

        #region 工具

        /// <summary>
        /// 根据AssetBundle名称和资源名称获得资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        public Object GetAsset(string bundleName, string assetName, SceneType sceneType)
        {
            AssetBundlesInScene assetBundlesInScene = GetAssetBundlesInScene(sceneType);
            AssetBundleContent  bundleContent       = assetBundlesInScene.GetAssetBundleContent(bundleName);

            if (bundleContent != null)
            {
                return bundleContent.GetAsset(assetName);
            }

            return null;
        }

        /// <summary>
        /// 根据AssetBundle名称和sprite名称获得Sprite
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="spriteName"></param>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        public Sprite GetSprite(string bundleName, string spriteName, SceneType sceneType)
        {
            SpriteAtlas spriteAtlas = this.GetAsset(bundleName, bundleName, sceneType) as SpriteAtlas;
            //非altas图集，ugui自带altas的bug比较多
            if (spriteAtlas == null)
            {
                Sprite asprite = this.GetAsset(bundleName, spriteName, sceneType) as Sprite;
                return asprite;
            }
            else
            {
                Sprite sprite = spriteAtlas.GetSprite(spriteName);
                if (sprite == null)
                {
                    Debuger.Log("图集:" + bundleName + "中，纹理：" + spriteName + "为空！！");
                }

                return sprite;
            }
        }

        /// <summary>
        /// 获得指定icon的sprite
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="spriteName"></param>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        public Sprite GetIconSprite(string bundleName, string spriteName, SceneType sceneType)
        {
            spriteName = spriteName.Replace("Sprites/_Icon/", "");
            Sprite sprite = this.GetSprite(bundleName, spriteName, sceneType);
            if (sprite == null)
            {
                Debuger.LogError("icon不存在:" + spriteName);
            }

            return sprite;
        }

        /// <summary>
        /// 获得九宫格图片
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sprite GetNineSprite(string name)
        {
            string Psth = "Sprites/_Common/";
            return Resources.Load<Sprite>(Psth + name);
        }

        #endregion
    }
}