using ER.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using static ER.StateMachine.Enums;
namespace ER.GUI
{
    public abstract class MouseHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private StateCellMachine<Enums.TransitionEnum> m_scm;
        private bool m_IsMouseInRange;

        public StateCellMachine<Enums.TransitionEnum> scm { get => m_scm; protected set => m_scm = value; }
        public bool IsMouseInRange { get => m_IsMouseInRange; set => m_IsMouseInRange = value; }

        protected virtual void Awake()
        {
            scm = new StateCellMachine<Enums.TransitionEnum>();
            scm.CreateStates(Enums.TransitionEnum.Disable);
            InitSateMachine(scm);
        }
        protected virtual void Update()
        {
            scm?.Update();
        }
        protected virtual void InitSateMachine(StateCellMachine<Enums.TransitionEnum> scm)
        {

        }

        protected virtual void OnEnter()
        {
            scm.ChangeState(TransitionEnum.Entering);
        }
        protected virtual void OnExit()
        {
            scm.ChangeState(TransitionEnum.Exiting);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            IsMouseInRange = true;
            OnEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsMouseInRange = false;
            OnExit();
        }
        private void OnDisable()
        {
            if (IsMouseInRange)
            {
                IsMouseInRange = false;
                OnExit();
            }
        }
    }
}