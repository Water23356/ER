using ER.ForEditor;
using ER.StateMachine;
using System;
using UnityEngine;

namespace ER.GUI.Animations
{
    /// <summary>
    /// 淡出淡入动画
    /// </summary>
    public class UIScaleAnimation : MonoBehaviour
    {
        private StateCellMachine<Enums.TransitionEnum> scm;

        [DisplayLabel("插值速度")]
        [SerializeField]
        private float lerpSpeed = 6;

        [DisplayLabel("目标缩放")]
        [SerializeField]
        private float aimScale = 1.25f;

        [DisplayLabel("默认缩放")]
        [SerializeField]
        private float defaultScale = 1f;

        [Tooltip("启用后: 抵达Enable状态立即进入Exiting过渡")]
        [DisplayLabel("自动回弹")]
        [SerializeField]
        private bool autoScaleBack = false;

        [ReadOnly]
        [SerializeField]
        private float anim_k;

        public Action onDisable;

        public Action onEnable;

        public float LerpSpeed { get => lerpSpeed; set => lerpSpeed = value; }
        public float AimScale { get => aimScale; set => aimScale = value; }
        public float DefaultScale { get => defaultScale; set => defaultScale = value; }
        public bool AutoScaleBack { get => autoScaleBack; set => autoScaleBack = value; }

        public void ClearOnDisableEvent()
        {
            onDisable = null;
        }

        public void ClearOnEnableEvent()
        {
            onEnable = null;
        }

        private void Awake()
        {
            InitStateMachine();
        }

        private void Update()
        {
            scm.Update();
        }

        private void InitStateMachine()
        {
            scm = new StateCellMachine<Enums.TransitionEnum>();
            scm.CreateStates(Enums.TransitionEnum.Disable);

            var state = scm.GetState(Enums.TransitionEnum.Disable);
            state.OnEnter = s =>
            {
                onDisable?.Invoke();
            };

            state = scm.GetState(Enums.TransitionEnum.Entering);
            state.OnEnter = s =>
            {
                if (s.StateIndex != Enums.TransitionEnum.Entering && s.StateIndex != Enums.TransitionEnum.Exiting)
                {
                    anim_k = 0;
                }
            };
            state.OnUpdate = () =>
            {
                anim_k += Time.deltaTime * LerpSpeed;
                transform.localScale = Vector3.one * Mathf.Lerp(DefaultScale, AimScale, Mathf.Clamp01(anim_k));

                if (anim_k > 1)
                {
                    scm.ChangeState(Enums.TransitionEnum.Enable);
                }
            };

            state = scm.GetState(Enums.TransitionEnum.Enable);
            state.OnEnter = s =>
            {
                onEnable?.Invoke();
                if (AutoScaleBack)
                {
                    scm.ChangeState(Enums.TransitionEnum.Exiting);
                }
            };

            state = scm.GetState(Enums.TransitionEnum.Exiting);
            state.OnEnter = s =>
            {
                if (s.StateIndex != Enums.TransitionEnum.Entering && s.StateIndex != Enums.TransitionEnum.Exiting)
                {
                    anim_k = 1;
                }
            };
            state.OnUpdate = () =>
            {
                anim_k -= Time.deltaTime * LerpSpeed;
                transform.localScale = Vector3.one * Mathf.Lerp(DefaultScale, AimScale, Mathf.Clamp01(anim_k));

                if (anim_k < 0)
                {
                    scm.ChangeState(Enums.TransitionEnum.Disable);
                }
            };
        }

        [ContextMenu("播放激活")]
        public void PlayDisplay()
        {
            enabled = true;
            scm.ChangeState(Enums.TransitionEnum.Entering);
        }

        [ContextMenu("播放失活")]
        public void PlayHiden()
        {
            scm.ChangeState(Enums.TransitionEnum.Exiting);
        }
    }
}