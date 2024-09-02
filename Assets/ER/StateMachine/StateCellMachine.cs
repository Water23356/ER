// Ignore Spelling: Unregistry

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.StateMachine
{
    public class StateCellMachine
    {
        public enum StateEventType
        {
            OnEnter,
            OnUpdate,
            OnExit
        }

        private Dictionary<int, StateCell> states = new Dictionary<int, StateCell>();
        private StateCell m_currentState;
        private StateCell m_defaultState;

        public StateCell defaultState
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

        public StateCell currentState
        { 
            get { return m_currentState; }
            protected set => m_currentState=value;
        }

        /// <summary>
        /// 根据指定枚举类型创建状态机模板, 每个枚举值都会有一个对应的状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CreateStates<T>(T defaultState) where T : Enum
        {
            int defaultIndex = (int)(object)defaultState;
            foreach (int value in Enum.GetValues(typeof(T)))
            {
                var state = new StateCell(value);
                RegistryState(state);
                if (value == defaultIndex)
                {
                    this.defaultState = state;
                    currentState = state;
                }
            }
        }
        /// <summary>
        /// 根据指定枚举类型创建状态机模板, 每个枚举值都会有一个对应的状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CreateStates<T>() where T : Enum
        {
            int defaultIndex = (int)(object)defaultState;
            foreach (int value in Enum.GetValues(typeof(T)))
            {
                var state = new StateCell(value);
                RegistryState(state);
                if (value == 0)
                {
                    this.defaultState = state;
                    currentState = state;

                }
            }
        }

        public void RegistryState(StateCell state)
        {
            states[state.StateIndex] = state;
        }

        public void UnregistryState(int index)
        {
            if (states.ContainsKey(index))
                states.Remove(index);
        }

        public void UnregistryState<T>(T stateIndex) where T : Enum
        {
            UnregistryState((int)(object)stateIndex);
        }

        /// <summary>
        /// 跳转至指定状态, 如果目标状态不存在则保持当前状态
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(int state)
        {
            var current = currentState;
            var next = GetState(state);

            currentState = next;
            current?.Exit(next);
            next?.Enter(current);
        }

        public void ChangeState<T>(T stateIndex) where T : Enum
        {
            ChangeState((int)(object)stateIndex);
        }
        /// <summary>
        /// 跳转到指定状态, 不做状态过渡
        /// </summary>
        public void SkipTo<T>(T state) where T : Enum
        {
            var next = GetState(state);
            currentState = next;
        }

        public StateCell GetState(int stateIndex)
        {
            if (states.TryGetValue(stateIndex, out StateCell state)) return state;
            //Debug.LogError($"指定状态不存在: {stateIndex}");
            return null;
        }

        public StateCell GetState<T>(T stateIndex) where T : Enum
        {
            return GetState((int)(object)stateIndex);
        }
        /// <summary>
        /// 获取当前状态的枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stateIndex"></param>
        /// <returns></returns>
        public T CurrentState<T>() where T : Enum
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

        public StateCellMachine(StateCell defaultState)
        {
            this.defaultState = defaultState;
            currentState = defaultState;
        }
    }
}