using ER.GUI;
using System;
using UnityEngine;

namespace ER.SceneManager
{
    /// <summary>
    /// 场景过渡基类
    /// </summary>
    public abstract class SceneTransition : EGUIPanel
    {
        public enum TransitionState
        {
            Invisible,
            Entering,
            Keeping,
            Exiting
        }

        /// <summary>
        /// 进度
        /// </summary>
        protected float progress;

        /// <summary>
        /// 场景加载进度
        /// </summary>
        public virtual float Progress { get => progress; set => progress = value; }

        private TransitionState state;

        public TransitionState State
        {
            get => state;
            protected set
            {
                var tmp = state;
                state = value;
                if (tmp == TransitionState.Entering && state == TransitionState.Keeping)
                {
                    OnEnterKeepState?.Invoke();
                    OnEnterKeepState = null;
                }
                else if (tmp == TransitionState.Keeping && state == TransitionState.Exiting)
                {
                    OnExitKeepState?.Invoke();
                    OnExitKeepState = null;
                }
            }
        }

        /// <summary>
        /// 由进入过渡状态 进入 保持状态 时触发
        /// </summary>
        public Action OnEnterKeepState;

        /// <summary>
        /// 离开 保持状态 进入 离开过渡状态 时触发
        /// </summary>
        public Action OnExitKeepState;

        /// <summary>
        /// 加载场景(开始过渡)
        /// </summary>
        public abstract void EnterTransition();

        /// <summary>
        /// 离开场景(结束过渡)
        /// </summary>
        public abstract void ExitTransition();

        [ContextMenu("开始过渡")]
        private void _TestEnter()
        {
            EnterTransition();
        }

        [ContextMenu("离开过渡")]
        private void _TestExit()
        {
            ExitTransition();
        }
    }
}