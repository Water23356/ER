using ER.ResourceManager;
using ER;
using ER.GUI;
using System.Collections;
using UnityEngine;

namespace ER.SceneJumper
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
        protected GameResource.ResourceLoadTask m_progress;

        /// <summary>
        /// 场景加载进度
        /// </summary>
        public GameResource.ResourceLoadTask progress { get => m_progress; set => m_progress = value; }
        public abstract string statusText { get; set; }

        private TransitionState state;

        public TransitionState State
        {
            get => state;
            protected set
            {
                state = value;
            }
        }

        /// <summary>
        /// 开始过渡: 返回一个协程迭代器, 该迭代器一直阻塞到 动画播放结束
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public abstract IEnumerator PlayStartAsync();

        /// <summary>
        /// 结束过渡: 返回一个协程迭代器, 该迭代器一直阻塞到 动画播放结束
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public abstract IEnumerator PlayEndAsync();

        [ContextMenu("开始过渡")]
        private void _TestEnter()
        {
            StartCoroutine(PlayStartAsync());
        }

        [ContextMenu("离开过渡")]
        private void _TestExit()
        {
            StartCoroutine(PlayEndAsync());
        }
    }
}