using ER.StateMachine;
using UnityEngine;
using static ER.StateMachine.StateEnums;

namespace ER.GUI
{
    /// <summary>
    /// 鼠标悬浮时特殊视觉效果(大小放大)
    /// </summary>
    public class MouseHoverScale : MouseHoverHandler
    {
        private Vector3 m_defaultSize = Vector3.one;

        [SerializeField]
        private float m_magnification = 1.25f;

        private float m_timer = 0f;

        [SerializeField]
        private float m_animSpeed = 6f;

        /// <summary>
        /// 是否保持放大有效: 有效: 将一直处于放大状态
        /// </summary>
        public bool keepEnable = false;

        /// <summary>
        /// 是否保持放大无效: 有效: 将一直处于静默状态
        /// </summary>
        public bool keepDisable = false;

        public Vector3 defaultSize { get => m_defaultSize; set => m_defaultSize = value; }
        public float magnification { get => m_magnification; set => m_magnification = value; }
        public float animSpeed { get => m_animSpeed; set => m_animSpeed = value; }
        public float timer { get => m_timer; set => m_timer = value; }

        protected override void InitSateMachine(StateCellMachine<TransitionEnum> scm)
        {
            #region 静默

            var state = scm.GetState(TransitionEnum.Disable);
            state.OnUpdate = () =>
            {
                //Debug.Log("状态:静默");
                if (keepEnable && !keepDisable)
                {
                    scm.TransitionTo(TransitionEnum.Entering);
                }
            };
            state.OnExit = (s) =>
            {
                timer = 0;
            };

            #endregion 静默

            #region 静默->开启

            state = scm.GetState(TransitionEnum.Entering);
            state.OnUpdate = () =>
            {
                //Debug.Log("状态:=>开启");

                timer += Time.deltaTime * animSpeed;
                transform.localScale = defaultSize * Mathf.Lerp(1f, magnification, timer);
                if (timer > 1f)
                {
                    transform.localScale = defaultSize * magnification;
                    scm.TransitionTo(TransitionEnum.Enable);
                    return;
                }
            };

            #endregion 静默->开启

            #region 开启

            state = scm.GetState(TransitionEnum.Enable);
            state.OnUpdate = () =>
            {
                //Debug.Log("状态:开启");
                if (!keepEnable && keepDisable)
                {
                    scm.TransitionTo(TransitionEnum.Exiting);
                }
            };
            state.OnExit = (s) =>
            {
                if (s.StateIndex == TransitionEnum.Exiting)
                    timer = 1f;
            };

            #endregion 开启

            #region 开启->静默

            state = scm.GetState(TransitionEnum.Exiting);
            state.OnUpdate = () =>
            {
                //Debug.Log("状态:=>关闭");

                timer -= Time.deltaTime * animSpeed;
                transform.localScale = defaultSize * Mathf.Lerp(1f, magnification, Mathf.Clamp01(timer));
                if (timer < 0f)
                {
                    scm.TransitionTo(TransitionEnum.Disable);
                    return;
                }
            };

            #endregion 开启->静默
        }
        protected override void OnEnter()
        {
            if (!keepDisable)
                base.OnEnter();
        }
        protected override void OnExit()
        {
            if (!keepEnable)
                base.OnExit();
        }
    }
}