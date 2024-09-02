

using UnityEngine;
using ER.ForEditor;

namespace ER.Resource
{
    /// <summary>
    /// ScriptableObject 配置资产
    /// </summary>
    public class BaseAssetConfigure:ScriptableObject,IResource
    {
        [ReadOnly]
        public string registryName;
        public string RegistryName
        {
            get => registryName;
            set=>registryName = value;
        }
    }
}