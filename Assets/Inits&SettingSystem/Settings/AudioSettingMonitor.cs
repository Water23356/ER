using UnityEngine;

/* 游戏的音频设置栏目
 * 可以根据项目需要修改设置项目
 */

public class AudioSettingMonitor : CommonSettingMonitor<AudioSettingMonitor>
{
    public const string _GLOBAL_VOLUME = "global_volume"; //全局音量
    public const string _BGM_VOLUME = "bgm_volume"; //bgm音量
    public const string _SOUND_VOLUME = "sound_volume"; //音效音量

    private static float global_volume;//全局音量
    private static float bgm_volume;//全局音量
    private static float sound_volume;//音效音量

    public static float GlobalVolume { get => global_volume; set => global_volume = value; }
    public static float BgmVolume { get => bgm_volume; set => bgm_volume = value; }
    public static float SoundVolume { get => sound_volume; set => sound_volume = value; }

    public AudioSettingMonitor()
    {
        SettingKey = GameSettings._AUDIO;
    }

    protected override void UpdateSettings()
    {
        GlobalVolume = CheckGlobalVolume();
        BgmVolume = CheckBGMVolume();
        SoundVolume = CheckSoundVolume();
    }

    public float CheckGlobalVolume()
    {
        if (settings.TryGetValue(_GLOBAL_VOLUME, out var settingText))
        {
            if (float.TryParse(settingText, out float result))
            {
                return Mathf.Clamp01(result);
            }
        }
        return 0.8f;
    }

    public float CheckBGMVolume()
    {
        if (settings.TryGetValue(_BGM_VOLUME, out var settingText))
        {
            if (float.TryParse(settingText, out float result))
            {
                return Mathf.Clamp01(result);
            }
        }
        return 0.8f;
    }

    public float CheckSoundVolume()
    {
        if (settings.TryGetValue(_SOUND_VOLUME, out var settingText))
        {
            if (float.TryParse(settingText, out float result))
            {
                return Mathf.Clamp01(result);
            }
        }
        return 0.8f;
    }
}