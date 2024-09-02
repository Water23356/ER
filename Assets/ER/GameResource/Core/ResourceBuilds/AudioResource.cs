using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 音频资源
    /// </summary>
    [CreateAssetMenu(fileName = "AudioResource", menuName = "创建可配置资产/音频资产", order = 2)]
    public class AudioResource : BaseAssetConfigure
    {
        [SerializeField]
        private AudioClip clip;

        /// <summary>
        /// AudioClip 资源对象
        /// </summary>
        public AudioClip Value => clip;

        public static explicit operator AudioClip(AudioResource source)
        {
            return source.Value;
        }

        public AudioResource(string _registryName, AudioClip origin)
        {
            clip = origin;
            registryName = _registryName;
        }
    }
}