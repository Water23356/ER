using UnityEngine;

/* 游戏的显示设置栏目
 * 可以根据项目需要修改设置项目
 */
public class DisplaySettingMonitor : CommonSettingMonitor<DisplaySettingMonitor>
{
    public const string _MAX_PFS = "max_fps"; //最大帧率
    public const string _VERTICAL_SYNC = "vertical_sync"; //垂直同步
    public const string _FULL_SCREEN = "screen_mode"; //显示模式
    public const string _OUTPUT_RESOLUTION = "output_resolution"; //输出分辨率

    #region 静态成员

    private static bool _FullScreen;
    private static OutputResolution _GameOutputResolution;
    private static FPSMaxMode _GameFPSMax;
    private static bool vSync;

    public static bool FullScreen
    {
        get => _FullScreen;
        set
        {
            _FullScreen = value;
            Instance.settings[_FULL_SCREEN] = _FullScreen + string.Empty;
            ScreenResolution screenResolution = GetScreenResolution(_GameOutputResolution);
            Screen.SetResolution(screenResolution.width, screenResolution.height, _FullScreen);
        }
    }

    public static OutputResolution GameOutputResolution
    {
        get => _GameOutputResolution;
        set
        {
            _GameOutputResolution = value;
            Instance.settings[_OUTPUT_RESOLUTION] = (int)_GameOutputResolution + string.Empty;
            ScreenResolution screenResolution = GetScreenResolution(_GameOutputResolution);
            Screen.SetResolution(screenResolution.width, screenResolution.height, _FullScreen);
        }
    }

    public static FPSMaxMode GameFPSMax
    {
        get => _GameFPSMax;
        set
        {
            _GameFPSMax = value;
            int targetFPS = GetMaxFPS(_GameFPSMax);
            Application.targetFrameRate = targetFPS;
        }
    }

    public static bool VSync
    {
        get => vSync;
        set
        {
            vSync = value;
            Instance.settings[_VERTICAL_SYNC] = vSync + string.Empty;
            if (vSync)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }
        }
    }

    public static ScreenResolution GetScreenResolution(OutputResolution mode)
    {
        switch (mode)
        {
            case OutputResolution._720p:
                return new ScreenResolution(1280, 720);

            case OutputResolution._1080p:
                return new ScreenResolution(1920, 1080);

            case OutputResolution._1440p:
                return new ScreenResolution(2560, 1440);

            case OutputResolution._4K_UHD:
                return new ScreenResolution(3840, 2160);

            default:
                return new ScreenResolution(1920, 1080);
        }
    }

    public static int GetMaxFPS(FPSMaxMode mode)
    {
        switch (mode)
        {
            case FPSMaxMode._30fps:
                return 30;

            case FPSMaxMode._144fps:
                return 144;

            case FPSMaxMode._240fps:
                return 240;

            case FPSMaxMode._60fps:
                return 60;

            case FPSMaxMode._90fps:
                return 90;

            case FPSMaxMode.Unlimited:
                return -1;

            default:
                return -1;
        }
    }

    #endregion 静态成员

    public DisplaySettingMonitor()
    {
        SettingKey = GameSettings._DISPLAY;
    }

    protected override void UpdateSettings()
    {
        //设置分辨率和是否全屏显示
        FullScreen = CheckFullScreen();
        GameOutputResolution = CheckScreenResolution();
        //设置垂直同步
        VSync = CheckVSyncMode();
        //设置目标帧率
        GameFPSMax = CheckMaxFPS();
    }

    #region 应用设置

    private bool CheckFullScreen()
    {
        if (settings.TryGetValue(_FULL_SCREEN, out var settingText)) //检查设置全屏
        {
            if (bool.TryParse(settingText, out bool result))
            {
                return result;
            }
        }
        return false;
    }

    private OutputResolution CheckScreenResolution()
    {
        if (settings.TryGetValue(_OUTPUT_RESOLUTION, out var settingText)) //获取分辨率枚举
        {
            if (int.TryParse(settingText, out int result))
            {
                return (OutputResolution)result;
            }
        }
        return OutputResolution._1080p;
    }

    private bool CheckVSyncMode()
    {
        //垂直同步可以避免画面撕裂, 但是可能会导致输入延迟的增加, 同时会进行锁帧:
        //垂直同步间隔=1: 60fps
        //垂直同步间隔=2: 30fps
        bool vsync = false;
        if (settings.TryGetValue(_VERTICAL_SYNC, out var settingText)) //获取分辨率枚举
        {
            if (bool.TryParse(settingText, out vsync))
            {
                return vsync;
            }
        }
        return false;
    }

    private FPSMaxMode CheckMaxFPS()
    {
        if (settings.TryGetValue(_MAX_PFS, out var text))
        {
            if (int.TryParse(text, out var value))
            {
                return (FPSMaxMode)value;
            }
        }
        return FPSMaxMode._60fps;
    }

    #endregion 应用设置

    #region 枚举定义

    /// <summary>
    /// 屏幕分辨率
    /// </summary>
    public struct ScreenResolution
    {
        public int width;
        public int height;

        public ScreenResolution(int width, int height)
        {
            this.width = width;
            this.height = height;
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

    #endregion 枚举定义
}