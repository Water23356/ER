using ER.StateMachine;
using Neo.IronLua;
using System;
using TMPro;
using UnityEngine;

namespace ER.GUI
{
    [RequireComponent(typeof(TMP_Text))]
    public class DamageNumber : Water
    {
        private TMP_Text txt;
        private float anim_k;

        public float offset_x_speed = 50f;  //x速度偏移量
        public float speed_y = 100f;    //y初速度
        public float ay = 100f; //y加速度
        public Vector2 startPos;

        private StateCellMachine<StateEnums.SwithEnum> scm;

        public void SetDamage(float damage, int precision = 1)
        {
            txt.text = string.Format(Math.Round(damage, precision).ToString());
        }

        public void SetDamage(float damage, Color color, int precision = 1)
        {
            txt.text = string.Format(Math.Round(damage, precision).ToString());
            txt.color = color;
        }

        private void Awake()
        {
            txt = GetComponent<TMP_Text>();
            InitStateMachine();
        }

        private void OnEnable()
        {
            //Debug.Log(startPos);
            scm.TransitionTo(StateEnums.SwithEnum.Enable);
        }

        private void Update()
        {
            scm.Update();
        }

        private void InitStateMachine()
        {
            scm = new StateCellMachine<StateEnums.SwithEnum>();
            scm.CreateStates(StateEnums.SwithEnum.Disable);

            var state = scm.GetState(StateEnums.SwithEnum.Disable);
            state.OnEnter = s =>
            {
                Destroy();
            };

            state = scm.GetState(StateEnums.SwithEnum.Enable);
            state.OnEnter = s =>
            {
                anim_k = 1;
            };
            state.OnUpdate = () =>
            {
                anim_k -= Time.deltaTime;
                float k = Mathf.Clamp01(anim_k);
                float p = 1 - k;
                transform.position = startPos + new Vector2(offset_x_speed * p, speed_y * p - ay * p * p);
                transform.localScale = Vector3.one * (1 + k * 0.5f);
                txt.color = txt.color.ModifyAlpha(k);

                if (anim_k < 0)
                {
                    scm.TransitionTo(StateEnums.SwithEnum.Disable);
                }
            };
        }
    }
}