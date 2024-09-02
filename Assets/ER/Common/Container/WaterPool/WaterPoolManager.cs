using System.Collections.Generic;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 对象池管理类（非组件单例模式）
    /// </summary>
    public sealed class WaterPoolManager : Singleton<WaterPoolManager>
    {
        #region 字段

        /// <summary>
        /// 池字典
        /// </summary>
        private Dictionary<string, WaterPool> poolDictionary = new Dictionary<string, WaterPool>();

        #endregion 字段

        #region 功能函数

        /// <summary>
        /// 从对应对象池中取对象
        /// </summary>
        /// <param name="poolName">对象池名称</param>
        /// <returns>获取到的对象</returns>
        public IWater GetObject(string poolName)
        {
            if (poolDictionary.TryGetValue(poolName, out WaterPool pool))
            {
                return pool.GetObject();
            }
            else
            {
                Debug.LogWarning("对象池不存在：" + poolName);
                return null;
            }
        }

        /// <summary>
        /// 从对应对象池中取对象
        /// </summary>
        /// <param name="poolName">对象池名称</param>
        /// <returns>获取到的对象</returns>
        public IWater this[string poolName]
        {
            get => GetObject(poolName);
        }

        /// <summary>
        /// 将对象还回对象池
        /// </summary>
        /// <param name="obj">游戏物体对象</param>
        /// <param name="poolName">对象池名称</param>
        public void ReturnObject(IWater obj)
        {
            if (obj == null || obj.Pool == null) return;
            string poolName = obj.Pool.PoolName;
            if (poolDictionary.TryGetValue(poolName, out WaterPool pool))
            {
                pool.ReturnObject(obj);
            }
            else
            {
                Debug.LogWarning("对象池不存在：" + poolName);
            }
        }
        /// <summary>
        /// 获取指定池
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns></returns>
        public WaterPool GetPool(string poolName)
        {
            if (poolDictionary.TryGetValue(poolName, out WaterPool pool))
            {
                return pool;
            }
            return null;
        }
        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <param name="pool">对象池实例</param>
        public void RegisterPool(WaterPool pool)
        {
            if (!poolDictionary.ContainsKey(pool.PoolName))
            {
                poolDictionary.Add(pool.PoolName, pool);
            }
            else
            {
                Debug.LogWarning("对象池已存在：" + pool.PoolName);
            }
        }

        /// <summary>
        /// 注销对象池
        /// </summary>
        /// <param name="poolName">对象池名称</param>
        public void UnregisterPool(string poolName)
        {
            if (poolDictionary.ContainsKey(poolName))
            {
                poolDictionary.Remove(poolName);
            }
            else
            {
                Debug.LogWarning("对象池不存在：" + poolName);
            }
        }

        #endregion 功能函数
    }
}