using System;
using UnityEngine;

namespace ER.GUI
{
    public interface ILerpLayoutItem
    {
        /// <summary>
        /// 布局位置(世界坐标)
        /// </summary>
        public Vector2 LayoutPos { get; set; }

        /// <summary>
        /// 所属布局组
        /// </summary>
        public ILerpLayoutGroup OwnerGroup { get; set; }
    }
}