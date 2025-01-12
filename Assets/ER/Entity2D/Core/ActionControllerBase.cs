using ER.ForEditor;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 单一动作控制器:
    /// <code>
    /// 模式:
    ///   * 控制状态机参数
    ///   * 直接控制动作对象
    /// </code>
    /// </summary>
    public abstract class ActionControllerBase : ControllerBase
    {
        [SerializeField]
        [Tooltip("开启时: 直接控制动作对象; 关闭时: 控制状态机参数")]
        [DisplayLabel("直接控制")]
        protected bool directControl = false;

        private bool m_lastState = false;

        protected bool lastState
        {
            get => m_lastState;
            set
            {
                var tmp = m_lastState;
                m_lastState = value;
                if (tmp != value)
                {
                    if (value)
                    {
                        if (handleAim.CanEntry())
                            handleAim.Entry();
                    }
                    else
                    {
                        handleAim.Exit();
                    }
                }
            }
        }

        public abstract ActionBase handleAim { get; protected set; }

        /// <summary>
        /// 更新动作控制状态
        /// </summary>
        protected void UpdateActionControlState(bool state)
        {
            if (directControl)
            {
                lastState = state;
            }
            else
            {
                smAgent.SetParam(handleAim.actionName, state);
            }
        }

        /// <summary>
        /// 更新绑定的动作对象
        /// </summary>
        public abstract void UpdateHandleAim();
    }
}