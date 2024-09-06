using ER;
using System.Collections.Generic;

public abstract class CommonSettingMonitor<T> : Singleton<T> where T : class, new()
{
    public string SettingKey;
    protected Dictionary<string, string> settings = new Dictionary<string, string>();

    /// <summary>
    /// 将设置字典值更新至 newDic
    /// </summary>
    /// <param name="newDic"></param>
    protected void UpdateSettingDic(Dictionary<string, string> newDic) //同步设置
    {
        settings.Clear();
        if (newDic == null)
            return;
        foreach (var pair in newDic)
        {
            settings[pair.Key] = pair.Value;
        }
    }
    /// <summary>
    /// 应用设置
    /// </summary>
    protected abstract void UpdateSettings();
    /// <summary>
    /// 推送本类的设置更改
    /// </summary>
    protected void PushSettingDic()
    {
        Dictionary<string, string> newDic = new Dictionary<string, string>(settings);
        GameSettings.Instance.SetSettings(SettingKey, newDic);
    }

    #region 公开方法
    /// <summary>
    /// 同步GameSettings中的设置
    /// </summary>
    public void PullSettings()
    {
        UpdateSettingDic((Dictionary<string, string>)GameSettings.Instance.GetSettings(SettingKey));
        UpdateSettings();
        PushSettingDic();
    }

    /// <summary>
    /// 本设置更改推送至 GameSettings
    /// </summary>
    public void PushSttings()
    {
        PushSettingDic();
    }

    /// <summary>
    /// 获取指定输入, 如果找不到设置则返回null
    /// </summary>
    /// <returns></returns>
    public string GetSetting(string key)
    {
        if (settings.TryGetValue(key, out var setting))
        {
            return setting;
        }
        return null;
    }
    /// <summary>
    /// 更改设置
    /// </summary>
    /// <param name="newDic"></param>
    public void SetNewSettings(Dictionary<string, string> newDic)
    {
        UpdateSettingDic(newDic);
        UpdateSettings();
        PushSettingDic();
    }
    /// <summary>
    /// 获取设置的字典拷贝
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> GetSettings()
    {
        return settings.Copy();
    }
    #endregion
}