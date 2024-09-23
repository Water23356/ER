using ER.Resource;
using UnityEngine;

namespace Dev
{
    public class SpriteResource : IRegisterResource
    {
        private RegistryName m_registryName;
        public RegistryName registryName => m_registryName;
        private MetaSprite m_meta;

        public MetaSprite meta
        {
            get
            {
                if(m_meta == null)
                {
                    var regName = registryName;
                    regName.Head = "meta";
                    regName.Path = string.Join('/', "sprite", regName.Path);

                    GR.ELoad(regName, (res) =>
                    {
                        if(res is MetaSprite)
                        {
                            m_meta = (MetaSprite)res;
                        }
                    });
                }
                return m_meta;
            }
            set=>m_meta = value;
        }

        private Sprite value;

        public SpriteResource(RegistryName regName, Sprite value)
        {
            m_registryName = regName;
            this.value = value;
        }

        public T Get<T>()
        {
            if (value is T)
                return (T)(object)value;
            Debug.LogWarning($"资源类型封装错误: 预期类型: {typeof(T).FullName} 实际类型: {value.GetType().FullName}");
            return default(T);
        }

        public static implicit operator Sprite(SpriteResource obj)
        {
            return obj.value;
        }

    }
}