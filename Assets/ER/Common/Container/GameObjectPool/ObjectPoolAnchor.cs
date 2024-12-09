using UnityEngine;

namespace ER
{
    /// <summary>
    /// 对象池的回收锚点
    /// </summary>
    public class ObjectPoolAnchor : MonoBehaviour
    {
        /// <summary>
        /// 来源池
        /// </summary>
        public GameObjectPool originPool;

        /// <summary>
        /// 重置对象状态
        /// </summary>
        public virtual void ResetStatus()
        {

        }

        public void Destroy()
        {
            if (originPool == null) return;
            originPool.ReturnObject(gameObject);
        }
    }
}