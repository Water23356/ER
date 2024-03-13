using ER.Resource;

namespace ER.ItemStorage
{
    /// <summary>
    /// 物品资源接口
    /// </summary>
    public interface IItemResource:IResource
    {
        /// <summary>
        /// 根据注册文本初始化信息
        /// </summary>
        /// <param name="registryText"></param>
        public void Init(string registryText);
    }
}