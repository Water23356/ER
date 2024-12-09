using System;
using UnityEngine;

namespace ER.StateMachine
{
    public abstract class CompStateMachine : MonoBehaviour
    {
        private StateCellMachine m_stateMachine;
        public StateCellMachine stateMachine
        {
            get
            {
                if (m_stateMachine == null)
                    m_stateMachine = new StateCellMachine(null);
                return m_stateMachine;
            }
        }
        protected StateCell GetState<T>(T index)where T:Enum
        {
            return stateMachine.GetState(index);
        }
        protected void ToState<T>(T index)where T:Enum
        {
            stateMachine.TransitionTo(index);
        }

        protected virtual void Awake()
        {
            InitState();
        }

        public abstract void InitState();

        protected virtual void Update()
        {
            stateMachine.Update();
        }
    }
}