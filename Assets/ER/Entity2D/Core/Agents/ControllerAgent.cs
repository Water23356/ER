using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D.Agents
{
    /// <summary>
    /// 控制代理器
    /// </summary>
    public class ControllerAgent : MonoBehaviour
    {
        [Tooltip("与该脚本不在同一个物体的*控制器对象*需要在这里配置")]
        [DisplayLabel("控制器")]
        [SerializeField]
        private ControllerBase[] requires;


        private List<ControllerBase> controllers = new List<ControllerBase>();
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
            var self_controllers = GetComponentsInChildren<ControllerBase>();
            foreach (var controller in self_controllers)
            {
                Register(controller);
            }
            foreach(var controller in requires)
            {
                Register(controller);
            }
        }

        public void InitAll()
        {
            foreach(var controller in controllers)
            {
                controller.Init();
            }
        }
        /// <summary>
        /// 注册控制器
        /// </summary>
        /// <param name="controller"></param>
        public void Register(ControllerBase controller)
        {
            if (controllers.Contains(controller)) return;
            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i].GetType() == controller.GetType())
                {
                    Debug.LogError($"无法添加新的控制组件, 已存在相同控制组件: {controller.GetType()}");
                    return;
                }
            }

            controller.agent = this;
            controllers.Add(controller);
        }
        /// <summary>
        /// 注销控制器
        /// </summary>
        /// <param name="state"></param>
        public void Unregister(ControllerBase state)
        {
            controllers.Remove(state);
            Destroy(state);
        }
        /// <summary>
        /// 注销控制器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Unregister<T>() where T : ControllerBase
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i] is T)
                {
                    controllers.RemoveAt(i);
                    return;
                }
            }
        }
        /// <summary>
        /// 取得控制器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T Get<T>() where T : ControllerBase
        {
            foreach (var action in controllers)
            {
                if (action is T)
                {
                    return (T)action;
                }
            }
            return null;
        }

        private void OnEnable()
        {
            foreach (var controller in controllers)
            {
                controller.enabled = true;
            }
        }
        private void OnDisable()
        {
            foreach (var controller in controllers)
            {
                controller.enabled = false;
            }
        }
    }
}
