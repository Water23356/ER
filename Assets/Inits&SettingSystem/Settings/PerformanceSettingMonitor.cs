public class PerformanceSettingMonitor : CommonSettingMonitor<PerformanceSettingMonitor>
{
    public PerformanceSettingMonitor()
    {
        SettingKey = GameSettings._PERFORMANCE;
    }

    protected override void UpdateSettings()
    {
        SetEnemyCountLimit();
    }

    private void SetEnemyCountLimit()
    {
    }
}
