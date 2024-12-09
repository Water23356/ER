using ER.ForEditor;
using ER.StateMachine;
using System;
using UnityEngine;

namespace ER.GUI.Animations
{
    /// <summary>
    /// 淡出淡入动画
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFadeAnimation : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private StateCellMachine<StateEnums.TransitionEnum> scm;

        [DisplayLabel("插值速度")]
        [SerializeField]
        private float lerpSpeed = 6;

        [DisplayLabel("默认激活")]
        [SerializeField]
        private bool defaultActive = false;

        [Tooltip("启用后: 如果 defaultActive = false, Awake将会触发失活事件委托")]
        [DisplayLabel("默认激活事件")]
        [SerializeField]
        private bool defaultInvokeEvent = false;

        public float LerpSpeed { get => lerpSpeed; set => lerpSpeed = value; }
        public bool DefaultActive { get => defaultActive; set => defaultActive = value; }
        public bool DefaultInvokeEvent { get => defaultInvokeEvent; set => defaultInvokeEvent = value; }

        [ReadOnly]
        [SerializeField]
        private float anim_k;

        /// <summary>
        /// 在进入 Enable 状态后触发的事件
        /// </summary>
        public event Action onEnable;

        /// <summary>
        /// 在进入 Disable 状态后触发的事件
        /// </summary>
        public event Action onDisable;

        public void ClearOnEnableEvent()
        {
            onEnable = null;
        }

        public void ClearOnDisableEvent()
        {
            onDisable = null;
        }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            InitStateMachine();
            if (DefaultActive)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                canvasGroup.alpha = 0f;
                enabled = false;
                if (DefaultInvokeEvent)
                    onDisable?.Invoke();
            }
        }

        private void Update()
        {
            scm.Update();
        }

        private void InitStateMachine()
        {
            scm = new StateCellMachine<StateEnums.TransitionEnum>();
            scm.CreateStates(StateEnums.TransitionEnum.Disable);

            var state = scm.GetState(StateEnums.TransitionEnum.Disable);
            state.OnEnter = s =>
            {
                enabled = false;
                onDisable?.Invoke();
            };

            state = scm.GetState(StateEnums.TransitionEnum.Entering);
            state.OnEnter = s =>
            {
                if (s.StateIndex != StateEnums.TransitionEnum.Entering && s.StateIndex != StateEnums.TransitionEnum.Exiting)
                {
                    anim_k = 0;
                }
            };
            state.OnUpdate = () =>
            {
                anim_k += Time.deltaTime * LerpSpeed;
                canvasGroup.alpha = Mathf.Clamp01(anim_k);

                if (anim_k > 1)
                {
                    scm.TransitionTo(StateEnums.TransitionEnum.Enable);
                }
            };

            state = scm.GetState(StateEnums.TransitionEnum.Enable);
            state.OnEnter = s =>
            {
                onEnable?.Invoke();
            };

            state = scm.GetState(StateEnums.TransitionEnum.Exiting);
            state.OnEnter = s =>
            {
                if (s.StateIndex != StateEnums.TransitionEnum.Entering && s.StateIndex != StateEnums.TransitionEnum.Exiting)
                {
                    anim_k = 1;
                }
            };
            state.OnUpdate = () =>
            {
                anim_k -= Time.deltaTime * LerpSpeed;
                canvasGroup.alpha = Mathf.Clamp01(anim_k);

                if (anim_k < 0)
                {
                    scm.TransitionTo(StateEnums.TransitionEnum.Disable);
                }
            };
        }

        [ContextMenu("播放淡入")]
        public void PlayDisplay()
        {
            enabled = true;
            scm.TransitionTo(StateEnums.TransitionEnum.Entering);
        }

        [ContextMenu("播放淡出")]
        public void PlayHiden()
        {
            scm.TransitionTo(StateEnums.TransitionEnum.Exiting);
        }
    }
}