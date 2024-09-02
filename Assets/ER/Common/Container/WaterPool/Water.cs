using UnityEngine;

namespace ER
{
    /// <summary>
    /// 对象池内容物
    /// </summary>
    public abstract class Water : MonoBehaviour,IWater
    {
        /// <summary>
        /// 所属池子对象
        /// </summary>
        protected WaterPool pool;

        /// <summary>
        /// 所属池子对象
        /// </summary>
        public WaterPool Pool { get => pool; set => pool = value; }

        public GameObject GameObject => gameObject;

        public void Destroy()
        {
            IWater.Destroy(this);
        }
        public virtual void OnGetFormPool()
        {
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public virtual void ResetState()
        { }

        /// <summary>
        /// 自身返回对象池时触发的函数
        /// </summary>
        public virtual void OnHide()
        { }
    }

    public interface IWater
    {
        public WaterPool Pool { get; set ; }
        public GameObject GameObject { get; }
        public Transform Transform { get => GameObject.transform; }

        /// <summary>
        /// 重置状态
        /// </summary>
        public void ResetState();
        /// <summary>
        /// 从池中取出时
        /// </summary>
        public void OnGetFormPool();
        /// <summary>
        /// 返回对象池时
        /// </summary>
        public void OnHide();
        public void Destroy()
        {
            Destroy(this);
        }
        /// <summary>
        /// 销毁(返回对象池)
        /// </summary>
        public static void Destroy(IWater water)
        {
            if (water.Pool != null)
            {
                water.Pool.ReturnObject(water);
            }
            else
            {
                if (water != null && water.GameObject != null)
                    GameObject.Destroy(water.GameObject);
            }
            water.OnHide();
        }
    }
}