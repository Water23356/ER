using ER.ForEditor;
using UnityEngine;

namespace GameSetting
{
    [CreateAssetMenu(fileName = "DisplayConfig", menuName = "游戏设置/显示")]
    public class DisplayConfig : ScriptableObject
    {
        [DisplayLabel("分辨率")]
        public OutputResolution outputResolution = OutputResolution._1080p;

        [DisplayLabel("最大帧率")]
        public FPSMaxMode fps = FPSMaxMode.Unlimited;

        [DisplayLabel("全屏")]
        public bool fullScreen = false;

        [DisplayLabel("垂直同步")]
        public bool Vsync = false;
    }
}