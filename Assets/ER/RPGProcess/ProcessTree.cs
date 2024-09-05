namespace ER.RPGProcess
{
    //进度树
    public class ProcessTree
    {
        //注册名
        private string registryName;
        //首节点
        private string startNode;

        //本进度树(单一层)所包含的所有节点注册名
        private string[] nodes;
        //本进度树(单一层)所包含的所有 节点效果 注册名
        private string[] changes;
    }
}