using ER.Resource;
using UnityEngine;

namespace Dev
{
    /// <summary>
    /// 音频资源
    /// </summary>
    [CreateAssetMenu(fileName = "MetaSprite", menuName = "ER/元/精灵图")]
    [MetaTable("sprite")]
    public class MetaSprite : AssetModifyConfigure
    {
        public override string metaHead => "sprite";

        public override T Get<T>()
        {
            return default(T);
        }
    }
}