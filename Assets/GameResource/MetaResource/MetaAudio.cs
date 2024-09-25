using UnityEngine;

namespace Dev
{
    /// <summary>
    /// 音频资源
    /// </summary>
    [CreateAssetMenu(fileName = "MetaAudio", menuName = "ER/元/音频片段")]
    [MetaTable("audio")]
    public class MetaAudio : AssetModifyConfigure
    {
        public override string metaHead => "audio";

        public override T Get<T>()
        {
            return default(T);
        }
    }
}