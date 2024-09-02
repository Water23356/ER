using System;
using UnityEngine;

namespace ER.UTask
{
    public sealed class UTask : MonoBehaviour
    {
        public enum TaskStatus
        {
            /// <summary>
            /// 失活的
            /// </summary>
            Inactive,

            /// <summary>
            /// 开始
            /// </summary>
            Start,

            /// <summary>
            /// 更新中
            /// </summary>
            Update,

            /// <summary>
            /// 结束
            /// </summary>
            Exit
        }

        private Action _OnStart;
        private Func<bool> _OnUpdate;
        private Action _OnExit;
        private TaskStatus status;
        private string owner;

        /// <summary>
        /// 任务初始化委托
        /// </summary>
        public Action OnStart { get => _OnStart; set => _OnStart = value; }

        /// <summary>
        /// 任务更新委托,如果满足任务结束条件则返回true
        /// </summary>
        public Func<bool> OnUpdate { get => _OnUpdate; set => _OnUpdate = value; }

        /// <summary>
        /// 任务结束委托
        /// </summary>
        public Action OnExit { get => _OnExit; set => _OnExit = value; }

        public TaskStatus Status { get => status; set => status = value; }
        /// <summary>
        /// 所有者标识符
        /// </summary>
        public string Owner { get=> owner; set => owner = value; }  
        /// <summary>
        /// 任务是否激活, 如果需要暂停可设置为false,
        /// </summary>
        public bool TaskActive
        {
            get => enabled;
            set=>enabled = value;
        }

        public void SetWithInfo(UTaskInfo taskInfo)
        {
            OnStart = taskInfo.OnStart;
            OnUpdate = taskInfo.OnUpdate;
            OnExit = taskInfo.OnExit;
            owner = taskInfo.ownerName;
        }

        private void Update()
        {
            switch (status)
            {
                case TaskStatus.Inactive:
                    enabled = false;
                    break;

                case TaskStatus.Start:
                    OnStart?.Invoke();
                    status = TaskStatus.Update;
                    break;

                case TaskStatus.Update:
                    if (OnUpdate == null)
                    {
                        status = TaskStatus.Update;
                    }
                    else
                    {
                        OnUpdate?.Invoke();
                    }
                    break;

                case TaskStatus.Exit:
                    OnExit?.Invoke();
                    status = TaskStatus.Inactive;
                    break;
            }
        }
    }
}