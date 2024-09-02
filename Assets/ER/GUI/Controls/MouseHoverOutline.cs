using ER;
using ER.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ER.GUI
{
    [RequireComponent(typeof(Outline))]
    public class MouseHoverOutline : MouseHoverHandler
    {
        private Outline outline;
        private float anim_k;
        [SerializeField]
        private float lerpSpeed = 4f;
        [SerializeField]
        [Range(0, 1)]
        private float aimAlpha = 0.8f;
        protected override void Awake()
        {
            outline = GetComponent<Outline>();
            base.Awake();
        }

        protected override void InitSateMachine(StateCellMachine<Enums.TransitionEnum> scm)
        {
            var state = scm.GetState(Enums.TransitionEnum.Disable);
            state.OnEnter = s =>
            {
                outline.enabled = false;
            };

            state = scm.GetState(Enums.TransitionEnum.Entering);
            state.OnEnter = s =>
            {
                outline.enabled = true;
                anim_k = 0f;
            };
            state.OnUpdate = () =>
            {
                anim_k += Time.deltaTime * lerpSpeed;
                outline.effectColor = outline.effectColor.ModifyAlpha(anim_k * aimAlpha);
                if (anim_k > 1)
                {
                    scm.ChangeState(Enums.TransitionEnum.Enable);
                }
            };


            state = scm.GetState(Enums.TransitionEnum.Enable);

            state = scm.GetState(Enums.TransitionEnum.Exiting);
            state.OnEnter = s =>
            {
                anim_k = 0f;
            };
            state.OnUpdate = () =>
            {
                anim_k += Time.deltaTime * lerpSpeed;
                outline.effectColor = outline.effectColor.ModifyAlpha((1 - anim_k) * aimAlpha);
                if (anim_k > 1)
                {
                    scm.ChangeState(Enums.TransitionEnum.Enable);
                }
            };
        }
    }
}