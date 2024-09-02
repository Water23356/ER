using ER.ForEditor;
using ER.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.SceneManager.Trasitions
{
    public class Fade : SceneTransition
    {
        #region 组件

        [Header("组件")]
        [SerializeField]
        private Image img;

        [SerializeField]
        private TMP_Text txt_prograss;

        #endregion 组件

        #region 设置

        [Header("设置")]
        [SerializeField]
        [DisplayLabel("淡化速度")]
        private float anim_speed = 1f;

        [SerializeField]
        [DisplayLabel("最小过度时间")]
        private float minKeepTime = 0.5f;

        private float smoothPrograss = 0f;

        #endregion 设置

        private float anim_k = 0f;

        private StateCellMachine sm;

        private void Lerp(float k)
        {
            float a_img = Mathf.Clamp(Mathf.Lerp(0, 1, k), 0, 1);
            float a_txt = Mathf.Clamp(Mathf.Lerp(0, 1, (k - 1) * 2f), 0, 1);
            //Debug.Log(a_img+" "+anim_k);
            img.color = img.color.ModifyAlpha(a_img);
            txt_prograss.color = txt_prograss.color.ModifyAlpha(a_txt);
        }

        private void Awake()
        {
            InitStateMachine();
            gameObject.SetActive(false);
        }

        private void InitStateMachine()
        {
            sm = new StateCellMachine(null);
            sm.CreateStates(TransitionState.Invisible);

            var state = sm.GetState(TransitionState.Invisible);
            state.OnEnter =
            (s) =>
            {
                //Debug.Log($"进入{TransitionState.Invisible}阶段");

                IsVisible = false;
            };


            state = sm.GetState(TransitionState.Entering);
            state.OnEnter = (s) =>
            {
                //Debug.Log($"进入{TransitionState.Entering}阶段");
                txt_prograss.text = Mathf.Clamp01(0).ToString("P2");//以百分数的形式显示, 同时保留两位小数

                anim_k = 0f;
                smoothPrograss = 0f;
                State = TransitionState.Entering;
            };
            state.OnUpdate = () =>
            {
                anim_k += Time.deltaTime * anim_speed;
                //Debug.Log($"{TransitionState.Entering}阶段: {anim_k}");
                Lerp(anim_k);
                if (anim_k >= 1.5f)
                {
                    sm.ChangeState(TransitionState.Keeping);
                }
            };


            state = sm.GetState(TransitionState.Keeping);
            state.OnEnter = (s) =>
            {
                //Debug.Log($"进入{TransitionState.Keeping}阶段");
                anim_k = 0f;
                State = TransitionState.Keeping;
            };
            state.OnUpdate = () =>
            {
                anim_k += Time.deltaTime;
                if (Progress - smoothPrograss > 0.01f)
                {
                    smoothPrograss = Mathf.Lerp(smoothPrograss, Progress + 0.01f, Mathf.Clamp01(Time.deltaTime * 10));
                }
                else
                {
                    smoothPrograss = Progress;
                }
                txt_prograss.text = Mathf.Clamp01(smoothPrograss).ToString("P2");//以百分数的形式显示, 同时保留两位小数
                                                                                 //Debug.Log($"{TransitionState.Keeping}阶段: {anim_k}");

                if (smoothPrograss >= 1f && anim_k >= minKeepTime)
                {
                    sm.ChangeState(TransitionState.Exiting);
                }
            };


            state = sm.GetState(TransitionState.Exiting);
            state.OnEnter = (s) =>
            {
                //Debug.Log($"进入{TransitionState.Exiting}阶段");

                anim_k = 1.5f;
                State = TransitionState.Exiting;
            };
            state.OnUpdate = () =>
            {
                anim_k -= Time.deltaTime * anim_speed;
                //Debug.Log($"{TransitionState.Exit}阶段: {anim_k}");
                Lerp(anim_k);
                if (anim_k < 0)
                {
                    sm.ChangeState(TransitionState.Invisible);
                }
            };
        }

        public override void EnterTransition()
        {
            //Debug.Log("进入过渡");
            IsVisible = true;
            sm.ChangeState(TransitionState.Entering);
        }

        public override void ExitTransition()
        {
            sm.ChangeState(TransitionState.Exiting);
        }

        private void Update()
        {
            sm.Update();
        }
    }
}