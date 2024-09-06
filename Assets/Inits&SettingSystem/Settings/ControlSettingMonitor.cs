using System.Collections.Generic;

public class ControlSettingMonitor:CommonSettingMonitor<ControlSettingMonitor>
{
    private Dictionary<string,string> overridePath = new Dictionary<string, string>();
    public string GetOverridePath(string key)
    {
        if(overridePath.TryGetValue(key,out var str))
        {
            return str;
        }
        return null;
    }
    public void SetOverridePath(string key,string path)
    {
        overridePath[key]=path;
    }
    public ControlSettingMonitor()
    {
        SettingKey = GameSettings._CONTROL;
    }

    protected override void UpdateSettings()
    {
        
    }
}