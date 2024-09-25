using System.Collections.Generic;

namespace GameSetting
{
    public class AudioSettings : GameSettingBase<AudioSettings>
    {
        private static float m_globalVolume = 0.75f;//全局音量
        private static float m_bgmVolume = 1f;//全局音量
        private static float m_soundVolume = 1f;//音效音量

        public static float GlobalVolume { get => m_globalVolume; set => m_globalVolume = value; }
        public static float BgmVolume { get => m_bgmVolume; set => m_bgmVolume = value; }
        public static float SoundVolume { get => m_soundVolume; set => m_soundVolume = value; }

        public override Dictionary<string, string> GetSettingInfo()
        {
            var dic = new Dictionary<string, string>();
            dic["GlobalVolume"] = GlobalVolume.ToString();
            dic["BgmVolume"] = BgmVolume.ToString();
            dic["SoundVolume"] = SoundVolume.ToString();
            return dic;
        }

        public override void UpdateSettings(Dictionary<string, string> dic)
        {
            HandleDic(dic, "GlobalVolume", value =>
            {
                GlobalVolume = ToFloat(value);
            });
            HandleDic(dic, "BgmVolume", value =>
            {
                BgmVolume = ToFloat(value);
            });
            HandleDic(dic, "SoundVolume", value =>
            {
                SoundVolume = ToFloat(value);
            });
        }

        public void Set(AudioConfig config)
        {
            GlobalVolume = config.globalVolume;
            BgmVolume = config.bgmVolume;
            SoundVolume = config.soundVolume;
        }
    }
}