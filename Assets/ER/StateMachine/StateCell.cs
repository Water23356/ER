using ER.Dynamic;
using System;

namespace ER.StateMachine
{
    /// <summary>
    /// 通用状态单元
    /// </summary>
    public class StateCell:WithDynamicProperties
    {
        protected int stateIndex;

        protected Action<StateCell> m_onEnter;
        protected Action m_onUpdate;
        protected Action<StateCell> m_onExit;

        public Action<StateCell> OnEnter
        {
            protected get => m_onEnter;
            set
            {
                m_onEnter = value;
            }
        }
        public Action OnUpdate
        {
            protected get => m_onUpdate;
            set
            {
                m_onUpdate = value;
            }
        }

        public Action<StateCell> OnExit
        {
            protected get => m_onExit;
            set
            {
                m_onExit = value;
            }
        }


        public int StateIndex => stateIndex;

        public void Enter(StateCell lastState)
        {
            OnEnter?.Invoke(lastState);
        }
        public void Exit(StateCell lastState)
        {
            OnExit?.Invoke(lastState);
        }



        public virtual void Enter()//进入该状态的初始化
        {
            OnEnter?.Invoke(null);
        }

        public virtual void Update()//处于该状态的持续更新逻辑, 包含跳转到其他
        {
            OnUpdate?.Invoke();
        }

        public virtual void Exit()//离开该状态需要处理的逻辑
        {
            OnExit?.Invoke(null);
        }

        public StateCell SetEvent(Action<StateCell> _onEnter, Action _onUpdate = null, Action<StateCell> _onExit = null)
        {
            OnEnter = _onEnter;
            OnUpdate = _onUpdate;
            OnExit = _onExit;
            return this;
        }

        public StateCell(int index)
        {
            stateIndex = index;
        }
    }
}