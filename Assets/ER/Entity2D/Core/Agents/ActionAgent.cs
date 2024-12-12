using ER.ForEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ER.Entity2D.Agents
{
    /// <summary>
    /// 动作代理器
    /// </summary>
    public class ActionAgent : MonoBehaviour
    {
        [Tooltip("与该脚本不在同一个物体的*动作对象*需要在这里配置")]
        [DisplayLabel("动作")]
        [SerializeField]
        private ActionBase[] requires;

        private List<ActionBase> actions = new List<ActionBase>();
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
            var self_actions = GetComponentsInChildren<ActionBase>();
            foreach (var action in self_actions)
            {
                Register(action);
            }
            foreach (var action in requires)
            {
                Register(action);
            }
        }

        /// <summary>
        /// 初始化所有动作
        /// </summary>
        public void InitAll()
        {
            foreach(var action in actions)
            {
                action.Init();
            }
        }
        /// <summary>
        /// 注册动作
        /// </summary>
        /// <param name="action"></param>
        public void Register(ActionBase action)
        {
            if (actions.Contains(action)) return;
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].GetType() == action.GetType())
                {
                    Debug.LogError($"无法添加新的动作组件, 已存在相同动作组件: {action.GetType()}");
                    return;
                }
            }

            action.agent = this;
            actions.Add(action);
        }
        /// <summary>
        /// 注销动作
        /// </summary>
        /// <param name="action"></param>
        public void Unregister(ActionBase action)
        {
            actions.Remove(action);
            Destroy(action);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Unregister<T>() where T : ActionBase
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i] is T)
                {
                    actions.RemoveAt(i);
                    return;
                }
            }
        }
        /// <summary>
        /// 取得动作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T Get<T>() where T : ActionBase
        {
            foreach (var action in actions)
            {
                if (action is T)
                {
                    return (T)action;
                }
            }
            return null;
        }
        /// <summary>
        /// 取得动作
        /// </summary>
        /// <typeparam name="actionName"></typeparam>
        public ActionBase Get(string actionName)
        {
            foreach (var action in actions)
            {
                if (action.actionName == actionName) return action;
            }
            return null;
        }


        /// <summary>
        /// 触发动作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Trigger<T>() where T : ActionBase
        {
            var action = Get<T>();
            action.Entry();
        }
        /// <summary>
        /// 中断动作
        /// </summary>
        /// <param name="actionName"></param>
        public void Break(string actionName)
        {
            foreach (var action in actions)
            {
                if (action.actionName == actionName)
                {
                    action.Exit();
                    return;
                }
            }
        }
        /// <summary>
        /// 中断动作
        /// </summary>
        /// <param name="actionNames"></param>
        public void Break(params string[] actionNames)
        {
            foreach (var action in actions)
            {
                if (actionNames.Contains(action.actionName))
                {
                    action.Exit();
                    return;
                }
            }
        }


    }
}
