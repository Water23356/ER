namespace ER.RPG.TaskSystem
{
    /// <summary>
    /// 任务进度
    /// </summary>
    public class TaskProgress
    {
        public int total;
        public int current;

        public bool isDone
        {
            get => current >= total;
        }
    }
}