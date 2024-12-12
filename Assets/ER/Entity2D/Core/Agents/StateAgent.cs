using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D.Agents
{
    /// <summary>
    /// 实体状态代理器
    /// </summary>
    public class StateAgent : MonoBehaviour
    {


        [Tooltip("与该脚本不在同一个物体的*状态对象*需要在这里配置")]
        [DisplayLabel("状态")]
        [SerializeField]
        private StateBase[] requires;

        private List<StateBase> states = new List<StateBase>();
        private EntityAgent m_entity;

        public EntityAgent entity
        {
            get
            {
                if (m_entity == null)
                    m_entity = GetComponent<EntityAgent>();
                return m_entity;
            }
            set
            {
                m_entity = value;
            }
        }


        private void Awake()
        {
            var self_states = GetComponentsInChildren<StateBase>();
            foreach (var state in self_states)
            {
                Register(state);
            }
            foreach (var state in requires)
            {
                Register(state);
            }
        }

        /// <summary>
        /// 注册状态
        /// </summary>
        /// <param name="state"></param>
        public void Register(StateBase state)
        {
            if (states.Contains(state)) return;
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetType() == state.GetType())
                {
                    Debug.LogError($"无法添加新的动作组件, 已存在相同动作组件: {state.GetType()}");
                    return;
                }
            }

            state.agent = this;
            states.Add(state);
        }
        /// <summary>
        /// 注销状态
        /// </summary>
        /// <param name="state"></param>
        public void Unregister(StateBase state)
        {
            states.Remove(state);
            Destroy(state);
        }
        /// <summary>
        /// 注销状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Unregister<T>() where T : StateBase
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i] is T)
                {
                    states.RemoveAt(i);
                    return;
                }
            }
        }
        /// <summary>
        /// 取得状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T Get<T>() where T : StateBase
        {
            foreach (var action in states)
            {
                if (action is T)
                {
                    return (T)action;
                }
            }
            return null;
        }
    }
}
