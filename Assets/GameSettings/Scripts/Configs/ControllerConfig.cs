using ER.ForEditor;
using UnityEngine;

namespace GameSetting
{
    [CreateAssetMenu(fileName = "ControllerConfig", menuName = "游戏设置/控制")]
    public class ControllerConfig : ScriptableObject
    {
        [DisplayLabel("全局音量")]
        [Range(0,1)]
        public float globalVolume = 0.75f;
        [DisplayLabel("音乐音量")]
        [Range(0, 1)]
        public float bgmVolume = 0.75f;
        [DisplayLabel("音效音量")]
        [Range(0, 1)]
        public float soundVolume = 0.75f;
    }
}