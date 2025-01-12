using ER.ForEditor;
using UnityEngine;

namespace Dev.AI
{
    /// <summary>
    /// 输入单元
    /// </summary>
    public abstract class InputCell:MonoBehaviour
    {
        [SerializeField]
        [DisplayLabel("输出值")]
        private double m_state;
        public double state { get=>m_state; protected set => m_state = value; }
        /// <summary>
        /// 更新并获取细胞状态
        /// </summary>
        /// <returns></returns>
        public abstract double UpdateState();
    }
}