using System;
using System.Collections.Generic;

namespace ER.Extender
{
    /// <summary>
    /// 装饰器
    /// </summary>
    public interface IDecorator
    {
        /// <summary>
        /// 判断属性是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key);
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key);
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key);
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value);
    }
}