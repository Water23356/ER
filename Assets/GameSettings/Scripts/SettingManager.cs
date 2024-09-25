using ER;
using System;
using System.IO;
using UnityEngine;

namespace GameSetting
{
    public class SettingManager : MonoSingleton<SettingManager>
    {
        public const string DISPLAY = "DISPLAY";
        public const string AUDIO = "AUDIO";
        public const string CONTROLLER = "CONTROLLER";

        #region 配置

        public AudioConfig audioConfig;
        public DisplayConfig displayConfig;
        public ControllerConfig controllerConfig;

        #endregion 配置

        public string SettingFilePath
        {
            //在 streamingAssetsPath/settings.ini
            get { return ERinbone.Combine(Application.streamingAssetsPath, "settings.ini"); }
        }

        public event Action onChanged;

        private INIHandler settingHandler; //ini读写器

        public void Init()
        {
            AudioSettings.Instance.Set(audioConfig);
            ControllerSettings.Instance.Set(controllerConfig);
            DisplaySettings.Instance.Set(displayConfig);
        }

        public void LoadFromFile()
        {
            LoadSettingFromFile(SettingFilePath);
        }

        public void SaveToFile()
        {
            SaveSettingToFile(SettingFilePath);
        }

        public void LoadSettingFromFile(string path)
        {
            if (File.Exists(path))
            {
                settingHandler.Clear();
                settingHandler.ParseINIFile(path);
                UpdateSettings();
            }
        }

        public void SaveSettingToFile(string path)
        {
            settingHandler.Clear();
            SerializSetting();
            settingHandler.Save(path);
        }

        public void SerializSetting()
        {
            settingHandler.SetSection(DISPLAY, DisplaySettings.Instance.GetSettingInfo());
            settingHandler.SetSection(AUDIO, AudioSettings.Instance.GetSettingInfo());
            settingHandler.SetSection(CONTROLLER, ControllerSettings.Instance.GetSettingInfo());
            /*
           如果需要添加设置栏目, 则需要在这里更改设置
           */
            onChanged?.Invoke();
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        public void UpdateSettings()
        {
            DisplaySettings.Instance.UpdateSettings(settingHandler.GetSection(DISPLAY));
            AudioSettings.Instance.UpdateSettings(settingHandler.GetSection(AUDIO));
            ControllerSettings.Instance.UpdateSettings(settingHandler.GetSection(CONTROLLER));

            /*
            如果需要添加设置栏目, 则需要在这里更改设置
            */

            onChanged?.Invoke();
        }
    }
}