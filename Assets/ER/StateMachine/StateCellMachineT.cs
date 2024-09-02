// Ignore Spelling: Unregistry

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.StateMachine
{
    public class StateCellMachine<T> where T:Enum
    {
        public enum StateEventType
        {
            OnEnter,
            OnUpdate,
            OnExit
        }

        private Dictionary<T, StateCell<T>> states = new Dictionary<T, StateCell<T>>();
        private StateCell<T> m_currentState;
        private StateCell<T> m_defaultState;

        public StateCell<T> defaultState
        {
            get => m_defaultState;
            set
            {
                if (value == null)
                {
                    m_defaultState = null;
                    return;
                }
                if (!states.ContainsKey(value.StateIndex))
                {
                    RegistryState(value);
                }
                m_defaultState = value;
            }
        }

        public StateCell<T> currentState
        { 
            get { return m_currentState; }
            protected set => m_currentState=value;
        }

        /// <summary>
        /// 根据指定枚举类型创建状态机模板, 每个枚举值都会有一个对应的状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CreateStates(T defaultState)
        {
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                var state = new StateCell<T>(value);
                RegistryState(state);
                if (value.Equals(defaultState))
                {
                    this.defaultState = state;
                    currentState = state;
                }
            }
        }

        public void RegistryState(StateCell<T> state)
        {
            states[state.StateIndex] = state;
        }

        public void UnregistryState(T index)
        {
            if (states.ContainsKey(index))
                states.Remove(index);
        }

        /// <summary>
        /// 跳转至指定状态, 如果目标状态不存在则保持当前状态
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(T state)
        {
            var current = currentState;
            var next = GetState(state);

            currentState = next;
            current?.Exit(next);
            next?.Enter(current);
        }
        /// <summary>
        /// 跳转到指定状态, 不做状态过渡
        /// </summary>
        public void SkipTo(T state)
        {
            var next = GetState(state);
            currentState = next;
        }

        public StateCell<T> GetState(T stateIndex)
        {
            if (states.TryGetValue(stateIndex, out StateCell<T> state)) return state;
            //Debug.LogError($"指定状态不存在: {stateIndex}");
            return null;
        }
        /// <summary>
        /// 获取当前状态的枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stateIndex"></param>
        /// <returns></returns>
        public T CurrentState()
        {
            if (currentState == null)
            {
                Debug.LogError($"当前状态为空, 检查该状态机是否初始化!");
                return default(T);
            }
            return (T)(object)currentState.StateIndex;
        }

        public void Clear()
        {
            states.Clear();
        }

        public void Update()
        {
            //if(currentState != null)
            //{
            //    Debug.Log(currentState.StateIndex);
            //}
            m_currentState?.Update();
        }

        public StateCellMachine()
        {
            defaultState = null;
            currentState = null;
        }

        public StateCellMachine(StateCell<T> defaultState)
        {
            this.defaultState = defaultState;
            currentState = defaultState;
        }
    }
}