using ER;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ER.Resource;
public class GameSettings : Singleton<GameSettings>, ISettings
{
    public const string _DEBUG = "DEBUG";
    public const string _DISPLAY = "DISPLAY";
    public const string _CONTROL = "CONTROL";
    public const string _AUDIO = "AUDIO";
    public const string _PERFORMANCE = "PERFORMANCE";

    public static string DefaultSettingPath //默认的外部读写路径
    {
        get { return ERinbone.Combine(Application.streamingAssetsPath, "settings.ini"); }
    }
    private INIHandler setting_changes; //ini读写器
    public static SettingGroupResource settingGroup;
    private Dictionary<string, Dictionary<string, string>> settings; //用于暂存本地设置

    public event Action OnSettingChanged; //当设置发生变更时执行的事件

    public GameSettings()
    {
        setting_changes = new INIHandler();
    }

    /// <summary>
    /// 限定 返回值 为 Dictionary<string,string>
    /// </summary>
    /// <param name="registryName"></param>
    /// <returns></returns>
    public object GetSettings(string registryName)
    {
        Debug.Log($"读取设置: {registryName}");
        var dic = new Dictionary<string, string>();
        if (settings.TryGetValue(registryName, out var _dic_1))
        {
            foreach (var pair in _dic_1)
            {
                dic[pair.Key] = pair.Value;
            }
        }
        var _dic_2 = setting_changes.GetSection(registryName);
        if (_dic_2 != null) //外部设置进行复写
        {
            foreach (var pair in _dic_2)
            {
                dic[pair.Key] = pair.Value;
            }
        }
        return dic;
    }

    public string GetSettings(string registryName, string key)
    {
        Dictionary<string, string> dic = setting_changes.GetSection(registryName); //优先读取外部设置
        if (dic == null)
        {
            settings.TryGetValue(registryName, out dic);
        }
        if (dic != null)
        {
            if (dic.TryGetValue(key, out string result))
            {
                return result;
            }
        }
        return null;
    }

    public void Save()
    {
        setting_changes.Save(DefaultSettingPath);
    }

    /// <summary>
    /// 限定 settings 为Dictionary<string,string>
    /// </summary>
    /// <param name="registryName"></param>
    /// <param name="settings"></param>
    public void SetSettings(string registryName, object settings)
    {
        setting_changes.SetSection(registryName, (Dictionary<string, string>)settings);
    }

    public void UpdateSettings()
    {
        if (settingGroup != null)
        {
            settings = settingGroup.GetDictionary();
        }
        else
        {
            Debug.LogError("缺失默认配置组");
        }

        //接下来是读取外部的设置文件
        if (!File.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        if (!File.Exists(DefaultSettingPath))
        {
            File.Create(DefaultSettingPath).Close();
        }

        //外部配置覆盖内部配置
        setting_changes.Clear();
        setting_changes.ParseINIFile(DefaultSettingPath);
        Debug.Log("游戏设置更新成功!");

        SyncSettings();
    }

    /// <summary>
    /// 同步设置
    /// </summary>
    public void SyncSettings()
    {
        OnSettingChanged?.Invoke();
    }
}
