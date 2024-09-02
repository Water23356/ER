using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 预制体资源
    /// </summary>
    [CreateAssetMenu(fileName = "PrefabResource", menuName = "创建可配置资产/预制体资产", order = 3)]
    public class PrefabResource : BaseAssetConfigure
    {
        [SerializeField]
        private GameObject prfb;

        /// <summary>
        /// 预制体资源对象
        /// </summary>
        public GameObject Value => prfb;

        public static explicit operator GameObject(PrefabResource source)
        {
            return source.Value;
        }

        public PrefabResource(string _registryName, GameObject origin)
        {
            prfb = origin;
            registryName = _registryName;
        }

        /// <summary>
        /// 获取该预制体的拷贝物体对象
        /// </summary>
        public GameObject Copy
        {
            get
            {
                return GameObject.Instantiate(prfb);
            }
        }
    }
}