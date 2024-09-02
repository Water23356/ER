using System.Collections;
using System.Collections.Generic;

namespace ER.Dynamic
{
    public class DynamicProperties:IEnumerable<KeyValuePair<string, object>>
    {
        private Dictionary<string, object> properties = new Dictionary<string, object>();

        private object m_defualtValue = null;

        /// <summary>
        /// 默认值, 当属性不存在时返回该值
        /// </summary>
        public object defaultValue
        {
            get => m_defualtValue;
            set => m_defualtValue = value;
        }

        public object this[string key]
        {
            get
            {
                return properties.ContainsKey(key) ? properties[key] : defaultValue;
            }
            set
            {
                properties[key] = value;
            }
        }

        /// <summary>
        /// 检查属性是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return properties.ContainsKey(key);
        }

        /// <summary>
        /// 移除属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return properties.Remove(key);
        }

        /// <summary>
        /// 获取所有属性名称
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys()
        {
            return properties.Keys;
        }
        /// <summary>
        /// 清除所有属性
        /// </summary>
        public void Clear()
        { 
            properties.Clear(); 
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            foreach (var kp in properties)
                yield return kp;
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var kp in properties)
                yield return kp;
        }
    }
}