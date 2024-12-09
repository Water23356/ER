using ER;
using ER.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.GUI
{
    [RequireComponent(typeof(TMP_Text))]
    public class DamageNumber : Water
    {
        private TMP_Text txt;
        private float anim_k;

        public float offset_x_speed = 50f;
        public float speed_y = 100f;
        public float ay = 100f;
        public Vector2 startPos;

        private StateCellMachine<StateEnums.SwithEnum> scm;

        public void SetDamage(float damage)
        {
            txt.text = damage.ToString();
        }

        public void SetDamage(float damage, Color color)
        {
            txt.text = damage.ToString();
            txt.color = color;
        }

        private void Awake()
        {
            txt = GetComponent<TMP_Text>();
            InitStateMachine();
        }

        private void OnEnable()
        {
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

                transform.position = startPos + new Vector2(offset_x_speed * k, speed_y * k - ay * k * k);
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