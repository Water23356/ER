using System.Collections.Generic;

namespace GameSetting
{
    public class DisplaySettings : GameSettingBase<DisplaySettings>
    {
        private static bool m_fullScreen = false;
        private static bool m_vSync = false;
        private static OutputResolution m_gameOutputResolution = OutputResolution._1080p;
        private static FPSMaxMode m_gameFPSMax = FPSMaxMode.Unlimited;

        public static bool FullScreen { get => m_fullScreen; set => m_fullScreen = value; }
        public static OutputResolution GameOutputResolution { get => m_gameOutputResolution; set => m_gameOutputResolution = value; }
        public static FPSMaxMode GameFPSMax { get => m_gameFPSMax; set => m_gameFPSMax = value; }
        public static bool VSync { get => m_vSync; set => m_vSync = value; }

        public override Dictionary<string, string> GetSettingInfo()
        {
            var dic = new Dictionary<string, string>();
            dic["FullScreen"] = FullScreen.ToString().ToLower();
            dic["VSync"] = VSync.ToString().ToLower();
            dic["FPSMax"] = ((int)GameFPSMax).ToString();
            dic["OutputResolution"] = ((int)GameOutputResolution).ToString();
            return dic;
        }

        public override void UpdateSettings(Dictionary<string, string> dic)
        {
            HandleDic(dic, "FullScreen", value =>
            {
                FullScreen = ToBool(value);
            });
            HandleDic(dic, "OutputResolution", value =>
            {
                GameOutputResolution = (OutputResolution)ToInt(value);
            });
            HandleDic(dic, "FPSMax", value =>
            {
                GameFPSMax = (FPSMaxMode)ToInt(value);
            });
            HandleDic(dic, "VSync", value =>
            {
                VSync = ToBool(value);
            });
        }

        public void Set(DisplayConfig config)
        {
            FullScreen = config.fullScreen;
            VSync = config.Vsync;
            GameOutputResolution = config.outputResolution;
            GameFPSMax = config.fps;
        }
    }

    /// <summary>
    /// 输出分辨率
    /// </summary>
    public enum OutputResolution
    {
        _720p,
        _1080p,
        _1440p,
        _4K_UHD
    }

    /// <summary>
    /// 最大帧率
    /// </summary>
    public enum FPSMaxMode
    {
        Unlimited = -1, //无限
        _30fps = 0,
        _60fps = 1,
        _90fps = 2,
        _144fps = 3,
        _240fps = 4,
    }
}