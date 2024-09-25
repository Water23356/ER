using ER.ForEditor;
using UnityEngine;

namespace GameSetting
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "游戏设置/音频")]
    public class AudioConfig:ScriptableObject
    {
        [DisplayLabel("全局音量")]
        [Range(0,1)]
        public float globalVolume = 0.75f;
        [DisplayLabel("音乐音量")]
        [Range(0, 1)]
        public float bgmVolume = 1f;
        [DisplayLabel("音效音量")]
        [Range(0, 1)]
        public float soundVolume = 1f;
    }
}