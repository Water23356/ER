using System;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 区域检测接口
    /// </summary>
    public interface IRegion
    {
        /// <summary>
        /// 所属对象, 指向该区域的拥有者
        /// </summary>
        public IRegionOwner Owner { get; set; }
        /// <summary>
        /// 区域标签过滤器
        /// </summary>
        public TagFilter TagFilter { get; set; }
        /// <summary>
        /// 区域的位置
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// 区域进入接触事件 (激活该事件的 IRegion, 目标Collider)
        /// </summary>
        public event Action<IRegion,Collider2D> EnterEvent;

        /// <summary>
        /// 区域保持接触事件 (激活该事件的 IRegion, 目标Collider)
        /// </summary>
        public event Action<IRegion, Collider2D> StayEvent;

        /// <summary>
        /// 区域离开接触事件 (激活该事件的 IRegion, 目标Collider)
        /// </summary>
        public event Action<IRegion, Collider2D> ExitEvent;
    }
}