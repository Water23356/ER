using UnityEngine;

namespace ER.Entity2D.Components
{
    /// <summary>
    /// 拖尾重置脚本: 对象失活时自动清空渲染的位置信息
    /// </summary>
    public class TrailReseter:MonoBehaviour
    {
        private TrailRenderer m_Renderer;
        private void Awake()
        {
            m_Renderer = GetComponent<TrailRenderer>();
        }
        private void OnDisable()
        {
            m_Renderer.Clear();
        }
    }
}