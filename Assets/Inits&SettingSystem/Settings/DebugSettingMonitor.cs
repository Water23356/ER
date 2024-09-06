public class DebugSettingMonitor : CommonSettingMonitor<DebugSettingMonitor>
{
    public enum GameDebugMode
    {
        /// <summary>
        /// 关闭debug模式
        /// </summary>
        None,

        /// <summary>
        /// 一般调试模式
        /// </summary>
        Debug,
    }

    public const string _DEBUG_MODE = "debug_mode"; //调试模式
    public const string _BAN_LEVEL_UP = "ban_level_up"; //禁用升级模式

    private static GameDebugMode _DebugMode;
    private static bool _BanLevelUp;

    /// <summary>
    /// 调试模式
    /// </summary>
    /// <value></value>
    public static GameDebugMode DebugMode
    {
        get => _DebugMode;
        set
        {
            _DebugMode = value;
            Instance.settings[_DEBUG_MODE] = (int)_DebugMode + string.Empty;
        }
    }
    public static bool BanLevelUp
    {
        get => _BanLevelUp;
        set
        {
            _BanLevelUp = value;
            Instance.settings[_BAN_LEVEL_UP] = false + string.Empty;
        }
    }

    public DebugSettingMonitor()
    {
        SettingKey = GameSettings._DEBUG;
    }

    private GameDebugMode CheckDebugMode()
    {
        if (settings.TryGetValue(_DEBUG_MODE, out var rst))
        {
            if (int.TryParse(rst, out int value))
            {
                return (GameDebugMode)value;
            }
        }
        return GameDebugMode.None;
    }

    public bool CheckBanLevelUp()
    {
        if (settings.TryGetValue(_BAN_LEVEL_UP, out var rst))
        {
            if (bool.TryParse(rst, out bool value))
            {
                return value;
            }
        }
        return false;
    }

    protected override void UpdateSettings()
    {
        DebugMode = CheckDebugMode();
        BanLevelUp = CheckBanLevelUp();
    }
}
