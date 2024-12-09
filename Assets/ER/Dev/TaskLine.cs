using System.Collections.Generic;
using UnityEngine;

namespace Dev
{
    public interface ITaskExecutor
    {
        public TaskHandler Execute(ITaskMonitor monitor);
    }

    public interface ITaskMonitor
    {
        public void Continue();
    }

    public class TaskHandler
    {
        public ITaskExecutor executor;
        public ITaskMonitor monitor;
        public TaskStatus status;

        public enum TaskStatus
        {
            Ready,
            Running,
            Done,
            Error
        }
    }

    public abstract class TaskMonitor : TaskExecutor, ITaskMonitor
    {
        public abstract void Continue();

        public abstract void InitState();
    }

    public class TaskQueueExecutor : ITaskMonitor, ITaskExecutor
    {
        private List<TaskStage> tasks = new List<TaskStage>();
        private TaskHandler handler;

        public void Append(TaskStage task)
        {
            tasks.Add(task);
        }

        public void Clear()
        {
            tasks.Clear();
        }

        public void Continue()
        {
            if (tasks.Count == 0)
            {
                OnCompleted();
                return;
            }
            bool cont = true;
            while (cont)
            {
                var task = tasks[0];
                tasks.RemoveAt(0);
                cont = (task.Invoke(this) == null);
            }
        }

        public TaskHandler Execute(ITaskMonitor monitor)
        {
            if (handler == null)
            {
                handler = new TaskHandler()
                {
                    monitor = monitor,
                    executor = this,
                    status = TaskHandler.TaskStatus.Ready
                };
            }
            else
            {
                //如果存在先前未执行完毕的任务, 应当处理(还没想好)
            }
            Continue();
            return handler;
        }

        private void OnCompleted()
        {
            if (handler == null) return;
            handler.monitor.Continue();
            handler = null;
        }
        /// <summary>
        /// 是否正在执行任务
        /// </summary>
        public bool IsExecuting
        {
            get
            {
                return handler != null;
            }
        }

        public delegate TaskHandler TaskStage(ITaskMonitor monitor);
    }

    /// <summary>
    /// 异步任务:
    /// - Execute(): 启动该任务
    /// - OnCompleted(): 在任务结束时应当调用该函数
    /// </summary>
    public class TaskExecutor : MonoBehaviour, ITaskExecutor
    {
        protected TaskHandler handler;

        /// <summary>
        /// 启动该任务
        /// </summary>
        /// <param name="handler">任务协议</param>
        public virtual TaskHandler Execute(ITaskMonitor monitor)
        {
            handler = new TaskHandler()
            {
                monitor = monitor,
                executor = this,
                status = TaskHandler.TaskStatus.Ready
            };
            enabled = true;
            return handler;
        }

        /// <summary>
        /// 在任务结束时应当调用该函数
        /// </summary>
        protected virtual void OnCompleted()
        {
            if (handler == null) return;
            handler.monitor.Continue();
            handler = null;
            enabled = false;
        }
    }
}