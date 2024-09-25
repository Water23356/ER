using UnityEngine;
using UnityEngine.U2D;

namespace ER.ResourceManager
{
    /// <summary>
    /// 精灵图集: 规定: 同一个精灵图集只允许
    /// </summary>
    public class SpriteAtlasResource : IRegisterResource
    {
        private RegistryName m_registryName;
        public RegistryName registryName => m_registryName;

        private SpriteResource[] values;

        public SpriteAtlasResource(RegistryName regName, SpriteAtlas value)
        {
            m_registryName = regName;
            values = new SpriteResource[value.spriteCount];
            var group = new Sprite[value.spriteCount];
            value.GetSprites(group);

            //从 Atlas 提取 sprite
            for (int i = 0; i < group.Length; i++)
            {
                values[i] = new SpriteResource(new RegistryName($"{m_registryName}/{i}"), group[i]);
            }
        }

        public SpriteAtlasResource(RegistryName regName, Sprite[] group)
        {
            m_registryName = regName;
            values = new SpriteResource[group.Length];

            // 提取 sprite
            for (int i = 0; i < group.Length; i++)
            {
                values[i] = new SpriteResource(new RegistryName($"{m_registryName}/{i}"), group[i]);
            }
        }

        public T Get<T>()
        {
            if (values is T)
                return (T)(object)values;
            Debug.LogWarning($"资源类型封装错误: 预期类型: {typeof(T).FullName} 实际类型: {values.GetType().FullName}");
            return default(T);
        }

        public SpriteResource this[int index]
        {
            get
            {
                return values[index];
            }
        }
    }
}