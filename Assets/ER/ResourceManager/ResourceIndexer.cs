using System.Collections.Generic;

namespace ER.ResourceManager
{
    /// <summary>
    /// 资源索引器, 将资源注册名 转化为 地址或者url
    /// 外部资源使用@作为前缀标识
    /// </summary>
    public sealed class ResourceIndexer
    {
        public static ResourceIndexer instance;

        public static ResourceIndexer Instance
        { get { if (instance == null) instance = new ResourceIndexer(); return instance; } }

        /// <summary>
        /// 键值字典: 注册名->url
        /// </summary>
        private Dictionary<RegistryName, string> dic = new Dictionary<RegistryName, string>();

        /// <summary>
        /// 情况字典
        /// </summary>
        public void Clear()
        {
            dic.Clear();
        }

        public void Modify(string key, string url)
        {
            RegistryName rName = new RegistryName(key);
            dic[rName] = url;
        }

        public void Modify(RegistryName regName, string url)
        {
            dic[regName] = url;
        }

        public void Modify(Dictionary<string, string> moddic)
        {
            foreach (var mod in moddic)
            {
                RegistryName rName = new RegistryName(mod.Key);
                dic[rName] = mod.Value;
            }
        }

        public void Modify(MetaIndexDic indexDic)
        {
            foreach (var mod in indexDic.indexes)
            {
                dic[new RegistryName(mod.regName)] = mod.loadPath;
            }
        }

        public static LoadURL Convert(RegistryName regName)
        {
            return Instance.PathConvert(regName);
        }

        /// <summary>
        /// 将注册名转化为加载路径
        /// </summary>
        /// <param name="registryName">资源注册名</param>
        /// <param name="defResource">是否是本地路径</param>
        /// <returns></returns>
        public LoadURL PathConvert(RegistryName regName)
        {
            if (dic.TryGetValue(regName, out var value))
            {
                if (value.StartsWith('*'))
                {
                    return new LoadURL()
                    {
                        url = value.Substring(1),
                        state = LoadURL.State.FromWeb
                    };
                }

                return new LoadURL()
                {
                    url = value,
                    state = LoadURL.State.FromAddressable
                };
            }
            return new LoadURL()
            {
                url = null,
                state = LoadURL.State.Error
            };
        }

        public struct LoadURL
        {
            public string url;
            public State state;

            public enum State
            {
                Error = 0,
                FromAddressable,
                FromWeb,
            }
        }
    }
}