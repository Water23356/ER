namespace ER.RPG
{
    public class Node
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        public string name;

        /// <summary>
        /// 父节点
        /// </summary>
        public string parent;

        /// <summary>
        /// 子节点
        /// </summary>
        public string[] childs;

        public Status status;

        public bool IsLocked
        {
            get => status == Status.Locked;
        }

        public bool IsDone
        {
            get => status == Status.Enable;
        }

        public bool IsLoading
        {
            get=>status == Status.Loading;
        }

        /// <summary>
        /// 初始化指令(Lua): 如果该节点为末节点(与通过节点相邻的未通过节点), 那么就会执行该初始化指令;
        /// 作用是, 在读档时进行初步还原
        /// </summary>
        public string initCommand;

        /// <summary>
        /// 完成指令(Lua): 节点被通过时会触发的指令(通常用于实现一些全局效果)
        /// </summary>
        public string doneCommand;

        /// <summary>
        /// 额外信息
        /// </summary>
        public object extraInfo;


        public Node() { }
        public Node(ProcessGraphNode nodeInfo) 
        {
            name = nodeInfo.name;
            parent = nodeInfo.parent;
            childs = nodeInfo.childs.ToArray();
            status = nodeInfo.defaultStatus;
            extraInfo = nodeInfo.extraInfos;
        }


        public enum Status
        {
            /// <summary>
            /// 无效(未接触的节点)
            /// </summary>
            Disable,

            /// <summary>
            /// 有效(已经触发后的节点)
            /// </summary>
            Enable,

            /// <summary>
            /// 加载中(玩家正在执行的节点; 与有效节点相邻的无效节点)
            /// </summary>
            Loading,

            /// <summary>
            /// 被锁定(玩家无法通过的节点, 和无效等价, 但是无法被通过)
            /// </summary>
            Locked,
        }
    }
}