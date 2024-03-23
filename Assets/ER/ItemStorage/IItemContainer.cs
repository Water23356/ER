namespace ER.ItemStorage
{
    /// <summary>
    /// 物品容器接口(未完成)
    /// </summary>
    public interface IItemContainer : IUID
    {
        /// <summary>
        /// 当前容器物品堆的数量
        /// </summary>
        public int StackCount { get; }
        /// <summary>
        /// 判断指定物品堆是否存在
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public bool Contains(IItemStack stack);
 
    }
}