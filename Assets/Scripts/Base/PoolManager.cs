using UnityEngine.Serialization;

namespace Mega
{
    /// <summary>
    /// 对象池管理类
    /// </summary>
    public partial class PoolManager : GameComponent
    {
        public SpawnPool spawnPool;

        public static string poolName = "PoolRoot";

        PrefabPool Hit_Common;

        /// <summary>
        /// Effect对象池初始化
        /// </summary>
        public void InitAllEffectPool()
        {
            spawnPool              = Pools.Create(poolName);
            spawnPool.group.name   = poolName;
            spawnPool.group.parent = null;

            //通用单孔特效初始化
            // Hit_Common = new PrefabPool();
            // SpawnPollInitOption(spawnPool, Hit_Common, 50, 2, 5);
        }


        /// <summary>
        /// 生成对象池
        /// </summary>
        /// <param name="pool">SpawnPool对象</param>
        /// <param name="prefab">PrefabPool对象</param>
        /// <param name="preloadAmount">默认初始化N个Prefab</param>
        /// <param name="limitInstances">是否开启限制</param>
        /// <param name="limitFIFO">是否关闭无限取Prefab</param>
        /// <param name="limitAmount">限制池子里最大的Prefab数量</param>
        /// <param name="cullDespawned">开启自动清理池子</param>
        /// <param name="cullAbove">最终保留</param>
        /// <param name="cullDelay">多久清理一次</param>
        /// <param name="cullMaxPerpass">每次清理几个</param>
        protected void SpawnPollAddPrefab(SpawnPool spawnPool, PrefabPool prefab,      int  preloadAmount, bool limitInstances,
            bool                                    limitFIFO, int        limitAmount, bool cullDespawned, int  cullAbove, int cullDelay, int cullMaxPerpass)
        {
            if (!this.spawnPool._perPrefabPoolOptions.Contains(prefab))
            {
                //预生产预制件 实例对象 的个数,默认2
                prefab.preloadAmount = preloadAmount;
                //开启限制
                prefab.limitInstances = limitInstances;
                //关闭无限取Prefab
                prefab.limitFIFO = limitFIFO;
                //限制池子里最大的Prefab数量
                prefab.limitAmount = limitAmount;
                //开启自动清理池子
                prefab.cullDespawned = cullDespawned;
                //最终保留
                prefab.cullAbove = cullAbove;
                //多久清理一次
                prefab.cullDelay = cullDelay;
                //每次清理几个
                prefab.cullMaxPerPass = cullMaxPerpass;
                //初始化内存池
                spawnPool.CreatePrefabPool(prefab);
            }
        }


        private void SpawnPollInitOption(SpawnPool spawnPool, PrefabPool prefab, int maxCount, int cullDelay, int cullMaxPerPass)
        {
            //预生产预制件 实例对象 的个数,默认1
            prefab.preloadAmount = 2;
            //开启限制
            prefab.limitInstances = true;
            //限制池子里最大的Prefab数量,它和上面的preloadAmount是有冲突的，如果同时开启则以limitAmout为准。
            prefab.limitAmount = maxCount;
            //关闭无限取Prefab
            prefab.limitFIFO = true;
            //开启自动清理池子
            prefab.cullDespawned = true;
            //缓存池自动清理，但是始终保留几个对象不清理
            prefab.cullAbove = 2;
            //每过多久执行一遍自动清理，单位是秒
            prefab.cullDelay = cullDelay;
            //每次清理几个
            prefab.cullMaxPerPass = cullMaxPerPass;
            //初始化内存池
            spawnPool.CreatePrefabPool(prefab);
        }

        /// <summary>
        /// 销毁全部组件
        /// </summary>
        public void DestoryEffectObject()
        {
            Destroy(spawnPool.gameObject);
        }

        public void UnLoadResource()
        {
            Hit_Common = null;
            Debuger.Log("TrdPoolManager资源清理完毕", Debuger.ColorType.magenta);
        }
    }
}