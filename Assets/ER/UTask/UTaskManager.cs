using System.Collections.Generic;

namespace ER.UTask
{
    /// <summary>
    /// UTask 管理器
    /// </summary>
    public sealed class UTaskManager : MonoSingletonAutoCreate<UTaskManager>
    {
        private readonly static float _checkTimeCD = 3f;
        private float checktimer;
        private Dictionary<string,UTask> tasks = new Dictionary<string, UTask> ();
        private LinkedList<UTask> needRemoved = new LinkedList<UTask>();

        public void CreateUTask(UTaskInfo info)
        {
            var task = gameObject.AddComponent<UTask>();
            task.SetWithInfo (info);
            task.Status =  UTask.TaskStatus.Start;
            task.enabled = true;
            tasks[task.Owner] = task;
        }
        public void RemoveUTask(string owner)
        {
            if(tasks.TryGetValue(owner,out var cmp))
            {
                Destroy(cmp);
            }
        }

        private void CheckUTask()
        {
            var node = needRemoved.First;
            while(node!=null)
            {
                tasks.Remove(node.Value.Owner);
                node = node.Next;
            }
            needRemoved.Clear();

            foreach (var task in tasks.Values)
            {
                if(task.Status == UTask.TaskStatus.Inactive)
                {
                    needRemoved.AddLast(task);
                }
            }
        }

        private void Start()
        {
            TimerManager.UnscaledInvoke(CheckUTask,_checkTimeCD,-1);
        }
    }
}