using System;

namespace ER.UTask
{
    /// <summary>
    /// 更新任务;
    /// 当非 MonoBehaviour 的脚本需要进行组件逻辑模拟时, 可以使用该类作为模拟
    /// </summary>
    public struct UTaskInfo
    {
        public string ownerName;
        public Action OnStart;
        public Func<bool> OnUpdate;
        public Action OnExit;
    }
}