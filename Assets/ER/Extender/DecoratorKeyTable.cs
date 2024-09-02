using System;

namespace ER.Extender
{
    /// <summary>
    /// 装饰器 键表(DKT)
    /// </summary>
    public interface DecoratorKeyTable
    {
        /// <summary>
        /// 获取该拓展属性的类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Type TypeOf(string key);
    }
}