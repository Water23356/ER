using System.Collections.Generic;

namespace ER.Extender
{
    /// <summary>
    /// 装饰器
    /// </summary>
    public class Decorator
    {

        private Dictionary<string,object> datas = new Dictionary<string,object>();

        public bool Contains(string key)
        {
            return datas.ContainsKey(key);
        }

        public object Get(string key)
        {
            return datas[key];
        }
        public T Get<T>(string key)
        {
            if (datas[key] is T)
            {
                return (T)datas[key];
            }
            return default(T);
        }

        public void Set<T>(string key, T value)
        {
            datas[key]= value;
        }

        public Dictionary<string,object> GetAll()
        {
            return datas;
        }

    }
}