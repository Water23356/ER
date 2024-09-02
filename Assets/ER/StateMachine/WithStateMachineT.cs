using System;
using UnityEngine;

namespace ER.StateMachine
{
    public abstract class WithStateMachine<T> : MonoBehaviour where T : Enum
    {
        private StateCellMachine<T> m_stateMachine;
        public T state
        {
            get => (T)(object)stateMachine.currentState.StateIndex;
        }

        public StateCellMachine<T> stateMachine
        {
            get
            {
                if (m_stateMachine == null)
                    m_stateMachine = new StateCellMachine<T>(null);
                return m_stateMachine;
            }
        }

        protected void BuildStateMachine(T defaultState) 
        {
            stateMachine.CreateStates(defaultState);
        }
        protected StateCell<T> GetState(T index)
        {
            return stateMachine.GetState(index);
        }
        protected void ToState(T index) 
        {
            stateMachine.ChangeState(index);
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