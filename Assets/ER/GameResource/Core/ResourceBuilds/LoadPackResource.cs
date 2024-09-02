using UnityEngine;

namespace ER.Resource
{
    [CreateAssetMenu(fileName = "LoadPack", menuName = "创建可配置资产/加载包", order = 1)]
    public class LoadPackResource : BaseAssetConfigure
    {
        public LoadTask task;
    }
}