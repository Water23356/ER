namespace ER.RPGProcess
{
    //进度节点
    public class ProcessNode
    {
        //注册名
        private string registryName;

        //子节点树
        private ProcessTree subTree;

        //节点状态
        private NodeStatus status;

        private string[] next;
        private string parent;

        //完成该节点直接触发的效果
        private string[] changes;

        public enum NodeStatus
        {
            /// <summary>
            /// 无效
            /// </summary>
            Disable,

            /// <summary>
            /// 已完成
            /// </summary>
            Done,

            /// <summary>
            /// 无效(锁定)
            /// </summary>
            FixedDisable,

            /// <summary>
            /// 已完成(锁定)
            /// </summary>
            FixedDone,
        }
    }
}