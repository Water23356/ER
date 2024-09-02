using UnityEngine;

namespace ER
{
    /// <summary>
    /// 锚点
    /// </summary>
    public interface Anchor
    {
        /// <summary>
        /// 锚点标签
        /// </summary>
        public string AnchorTag { get; set; }

        /// <summary>
        /// 锚点位置
        /// </summary>
        public Vector3 Point { get; set; }

        /// <summary>
        /// 锚点的所有者
        /// </summary>
        public object Owner { get; }

        /// <summary>
        /// 销毁函数
        /// </summary>
        public void Destroy();
    }
}