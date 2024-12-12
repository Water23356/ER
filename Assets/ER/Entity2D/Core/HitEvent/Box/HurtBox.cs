using ER.Entity2D.Agents;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 受击箱: 需要和 Area 挂载在同一个物体上有效
    /// </summary>
    public class HurtBox : MonoBehaviour
    {
        private HurtHandler m_handler;

        public HurtHandler handler
        {
            get => handler; set => handler = value;
        }

        public EntityAgent entity => handler?.entity;
    }
}