using System.Collections.Generic;

namespace ER.ItemStorage
{
    /// <summary>
    /// 物品堆接口
    /// </summary>
    public interface IItemStack
    {
        /// <summary>
        /// 资源母对象
        /// </summary>
        public IItemResource Resource{get;}
        /// <summary>
        /// 物品堆数量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 堆叠上限
        /// </summary>
        public int AmountMax { get; set; }
    }
}