using ER;
using ER.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.GUI
{
    public class TurnTips : MonoBehaviour
    {
        [SerializeField]
        private RectTransform img_rect;

        [SerializeField]
        private Image img;

        [SerializeField]
        private TMP_Text txt;

        public string Text
        {
            get => txt.text;
            set => txt.text = value;
        }

        private StateCellMachine<Enums.TransitionEnum> scm;

        [SerializeField]
        private float lerpSpeed = 6f;

        [SerializeField]
        private float keepTime = 6f;

        private float anim_k = 0;

        public bool IsVisible
        {
            get => gameObject.activeSelf;
            set
            {
                gameObject.SetActive(value);
            }
        }

        public Color color
        {
            get => img.color;
            set => img.color = value;
        }

        public void Play()
        {
            IsVisible = true;
            scm.ChangeState(Enums.TransitionEnum.Entering);
        }

        public void Play(string title)
        {
            txt.text = title;
            IsVisible = true;
            scm.ChangeState(Enums.TransitionEnum.Entering);
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
            scm = new();
            scm.CreateStates(Enums.TransitionEnum.Disable);

            var state = scm.GetState(Enums.TransitionEnum.Disable);
            state.OnUpdate = () =>
            {
                IsVisible = false;
            };

            state = scm.GetState(Enums.TransitionEnum.Entering);
            state.OnEnter = s =>
            {
                anim_k = 0;
                img_rect.anchorMin = new Vector2(0, img_rect.anchorMin.y);
                img_rect.anchorMax = new Vector2(0, img_rect.anchorMax.y);
                txt.color = txt.color.ModifyAlpha(0);
            };
            state.OnUpdate = () =>
            {
                anim_k += Time.deltaTime * lerpSpeed;
                img_rect.anchorMax = new Vector2(Mathf.Clamp01(anim_k), img_rect.anchorMax.y);
                if (anim_k > 1)
                {
                    scm.ChangeState(Enums.TransitionEnum.Enable);
                }
            };
            state.OnExit = s =>
            {
                img_rect.anchorMax = new Vector2(1, img_rect.anchorMax.y);
            };

            state = scm.GetState(Enums.TransitionEnum.Enable);
            state.OnEnter = s =>
            {
                anim_k = 0;
            };
            state.OnUpdate = () =>
            {
                anim_k += Time.deltaTime * lerpSpeed;
                txt.color = txt.color.ModifyAlpha(Mathf.Clamp01(anim_k));
                if (anim_k > 1 + keepTime)
                {
                    scm.ChangeState(Enums.TransitionEnum.Exiting);
                }
            };

            state = scm.GetState(Enums.TransitionEnum.Exiting);
            state.OnEnter = s =>
            {
                anim_k = 0;
            };
            state.OnUpdate = () =>
            {
                anim_k += Time.deltaTime * lerpSpeed;
                img_rect.anchorMin = new Vector2(Mathf.Clamp01(anim_k), img_rect.anchorMin.y);
                if (anim_k > 1)
                {
                    scm.ChangeState(Enums.TransitionEnum.Disable);
                }
            };
            state.OnExit = s =>
            {
                img_rect.anchorMin = new Vector2(1, img_rect.anchorMin.y);
            };
        }
    }
}