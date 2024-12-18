﻿using ER.Dynamic;
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
        public void Update()//处于该状态的持续更新逻辑, 包含跳转到其他
        {
            OnUpdate?.Invoke();
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