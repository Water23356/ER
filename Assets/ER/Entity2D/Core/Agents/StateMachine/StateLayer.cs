using System;

namespace ER.Entity2D.Agents
{
    public class StateLayer
    {
        /// <summary>
        /// 动作层名称
        /// </summary>
        public string stateName;

        /// <summary>
        /// 层有效时触发
        /// </summary>
        public Action onEnable;

        /// <summary>
        /// 层无效时触发
        /// </summary>
        public Action onDisable;

        /// <summary>
        /// 检查是否有效
        /// <code>
        ///     return bool:  true时转入有效
        /// </code>
        /// </summary>
        public Func<bool> checkEnable;

        /// <summary>
        /// 检查是否无效
        /// <code>
        ///     return bool:  true时转入无效
        /// </code>
        /// </summary>
        public Func<bool> checkDisable;

        private bool m_state = false;

        /// <summary>
        /// 层有效状态
        /// </summary>
        public bool state
        {
            get => m_state;
            set { m_state = value; }
        }

        public void CheckTransition()
        {
            if (state)
            {
                if (checkDisable?.Invoke() ?? false)
                {
                    Disable();
                }
            }
            else
            {
                if (checkEnable?.Invoke() ?? false)
                {
                    Enable();
                }
            }
        }

        public void Enable()
        {
            state = true;
            onEnable?.Invoke();
        }

        public void Disable()
        {
            state = false;
            onDisable?.Invoke();
        }
    }
}