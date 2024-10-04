using System.Collections.Generic;

namespace ER.ResourceManager
{
    public class LanguageFileResource : IRegisterResource
    {
        private RegistryName m_registryName;
        public RegistryName registryName => m_registryName;

        private Dictionary<string, string> pairs;

        public LanguageFileResource(RegistryName regName, Dictionary<string, string> value)
        {
            m_registryName = regName;
            pairs = value;
        }

        public Dictionary<string, string> GetDic()
        {
            return pairs;
        }

        public T Get<T>()
        {
            if (pairs is T)
                return (T)(object)pairs;
            return default(T);
        }
    }
}