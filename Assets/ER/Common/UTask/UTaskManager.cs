using System.Collections.Generic;

namespace ER
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

        public UTask CreateUTask(UTaskInfo info)
        {
            var task = gameObject.AddComponent<UTask>();
            task.SetWithInfo (info);
            task.Status =  UTask.TaskStatus.Start;
            task.enabled = true;
            tasks[task.Owner] = task;
            return task;
        }
        public void RemoveUTask(string owner)
        {
            if(tasks.TryGetValue(owner,out var cmp))
            {
                Destroy(cmp);
                tasks.Remove(owner);
            }
        }
    }
}