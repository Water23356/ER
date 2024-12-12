using ER.Entity2D.Agents;
using ER.ForEditor;
using System.Linq;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 角色动作基础;
    /// </summary>
    public abstract class ActionBase:MonoBehaviour
    {
        [SerializeField]
        [DisplayLabel("参数名称")]
        private string m_actionName;

        /// <summary>
        /// 动作名称: 和动画机参数绑定
        /// </summary>
        public string actionName { get => m_actionName; set => m_actionName = value; }

        public ActionAgent agent { get; set; }
        public EntityAgent entity { get => agent?.entity; }

        /// <summary>
        /// 需要通过 entity 获取其他组件的逻辑在这里初始化
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// 是否可进入该状态
        /// </summary>
        /// <returns></returns>
        public virtual bool CanEntry() { return true;  }
        /// <summary>
        /// 进入动作
        /// </summary>
        public virtual void Entry() { }
        /// <summary>
        /// 离开动作(无论是正常离开还是被打断)
        /// </summary>
        public virtual void Exit() { }
        /// <summary>
        /// 响应步骤, 输入步骤编号执行对应逻辑
        /// </summary>
        public virtual void ResponseStep(int step) 
        {
            Debug.LogWarning($"未知动作步骤: {step}");
        }

    }
}