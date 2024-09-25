using Dev;
using ER;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Dev3
{
    public static class GLL
    {
        public static string GetText(string key, string defaultValue = "")
        {
            return GameLocalized.Instance.GetText(key, defaultValue);
        }
    }

    /// <summary>
    /// 游戏文本资产管理器;
    /// 推荐使用 GameResource 入口加载文本(统一资源加载入口), 文本资产的读取方式和其他资产不一样;
    /// 所有的文本资产都是外部资产, 存放在 streamingAssetPath/ 的子目录下;
    /// 可使用静态替身 GLL 获取文本值;
    /// </summary>
    public class GameLocalized : MonoSingleton<GameLocalized>
    {
        public static string ModLangAdapterPath
        {
            get => ERinbone.Combine(Application.streamingAssetsPath, "lang_adapter.json");
        }

        public static string LanguageOptionListPath
        {
            get => ERinbone.Combine(Application.streamingAssetsPath, "lang_list.json");
        }

        public string UsingLangCode { get => usingLangCode; }
        public Dictionary<string, string> LangOptionList { get => langOptionList; }
        public Dictionary<string, string> ModPathAdpator { get => modPathAdpator; }

        /// <summary>
        /// 模组文本目录 (模组名:相对路径)
        /// 例如: origin:lang ; 说明原版的所有文本资源将会在 streamingAssetPath/lang/ 目录下
        /// </summary>
        private Dictionary<string, string> modPathAdpator;

        /// <summary>
        /// 游戏支持的语言, 键是语言代码, 值是语言的显示文本;
        /// {"zh":"Chinese(中文)","en":"English"}
        /// </summary>
        private Dictionary<string, string> langOptionList;

        /// <summary>
        /// 文本键值对
        /// </summary>
        private Dictionary<string, string> langDic;

        /// <summary>
        /// 文本文件: 该文件包含的键值对; 用于文本情理
        /// </summary>
        private Dictionary<RegistryName, string[]> packs;

        /// <summary>
        /// 当前正在使用中的语言代码, 语言代码和语言包的总目录同名
        /// </summary>
        private string usingLangCode;

        /// <summary>
        /// 更新语言列表
        /// </summary>
        public void LoadLanguageList()
        {
            if (!File.Exists(LanguageOptionListPath))
            {
                File.Create(LanguageOptionListPath).Close();
            }
            string json = File.ReadAllText(LanguageOptionListPath);
            langOptionList = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        /// <summary>
        /// 更新模组语言适配路径
        /// </summary>
        public void LoadLanguageAdapter()
        {
            if (!File.Exists(ModLangAdapterPath))
            {
                File.Create(ModLangAdapterPath).Close();
            }
            string json = File.ReadAllText(ModLangAdapterPath);
            modPathAdpator = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        /// <summary>
        /// 获取文本(需要提前加载)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetText(string key, string defaultValue = "")
        {
            if (Instance?.langDic.TryGetValue(key, out var value) ?? false)
                return value;
            return defaultValue;
        }

        /// <summary>
        /// 加载语言文件
        /// </summary>
        /// <param name="path">目录中的相对路径</param>
        public void LoadTextFile(RegistryName regName)
        {
            if (ModPathAdpator.TryGetValue(regName.Module, out var directory))
            {
                var path = Path.Combine(directory, usingLangCode, regName.Path);
                if (File.Exists(path))
                {
                    LoadText(regName, File.ReadAllText(path));
                }
                else
                {
                    Debug.LogError($"缺失语言文件: {path}");
                }
            }
            else
            {
                Debug.LogError($"该模组语言路径未配置: {regName.Module}");
            }
        }

        /// <summary>
        /// 卸载语言文件
        /// </summary>
        /// <param name="regName"></param>
        public void UnLoadTextFile(RegistryName regName)
        {
            if (packs.TryGetValue(regName, out var pack))
            {
                foreach (var key in pack)
                {
                    langDic.Remove(key);
                }
                packs.Remove(regName);
            }
        }

        public void LoadText(RegistryName regName, string contentJson)
        {
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(contentJson);
            foreach (var pair in dic)
            {
                langDic[pair.Key] = pair.Value;
            }
            packs[regName] = dic.Keys.ToArray();
        }

        public void LoadText(RegistryName regName, Dictionary<string, string> pairs)
        {
            foreach (var pair in pairs)
            {
                langDic[pair.Key] = pair.Value;
            }
            packs[regName] = pairs.Keys.ToArray();
        }

        public bool IsLoaded(RegistryName regName)
        {
            return packs.ContainsKey(regName);
        }

        public void Clear()
        {
            langDic.Clear();
        }

        /// <summary>
        /// 取得已加载的文件所包含的文本键
        /// </summary>
        /// <returns></returns>
        public string[] GetTextFile(RegistryName regName)
        {
            if (packs.TryGetValue(regName, out var keys))
            {
                return keys;
            }
            return new string[0];
        }

        public Dictionary<string, string> GetLangDicCopy()
        {
            Dictionary<string, string> newDic = new Dictionary<string, string>();
            foreach (var key in langDic.Keys)
            {
                newDic[key] = langDic[key];
            }
            return newDic;
        }

        #region 配置修改

        /// <summary>
        /// 切换使用的语言
        /// </summary>
        /// <param name="langCode"></param>
        public void ChangeLangCode(string langCode)
        {
            usingLangCode = langCode;
            foreach (var pack in packs)
            {
                LoadTextFile(pack.Key);
            }
        }

        /// <summary>
        /// 添加语言选项
        /// </summary>
        /// <param name="langCode"></param>
        /// <param name="displatText"></param>
        public void AddLanguageOption(string langCode, string displatText)
        {
            LangOptionList[langCode] = displatText;
        }

        /// <summary>
        /// 添加模组适配路径
        /// </summary>
        /// <param name="modCode"></param>
        /// <param name="adpaterPath"></param>
        public void AddLanguageAdapter(string modCode, string adpaterPath)
        {
            ModPathAdpator[modCode] = adpaterPath;
        }

        /// <summary>
        /// 移除语言选项
        /// </summary>
        /// <param name="langCode"></param>
        public void RemoveLangaugeOption(string langCode)
        {
            LangOptionList.Remove(langCode);
        }

        /// <summary>
        /// 移除模组语言适配路径
        /// </summary>
        /// <param name="modCode"></param>
        public void RemoveLangaugeAdapter(string modCode)
        {
            ModPathAdpator.Remove(modCode);
        }

        /// <summary>
        /// 保存语言选项文件
        /// </summary>
        public void SaveOptionFile()
        {
            if (!File.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            string json = JsonConvert.SerializeObject(LangOptionList);
            File.WriteAllText(LanguageOptionListPath, json);
        }

        /// <summary>
        /// 保存模组语言适配器文件
        /// </summary>
        public void SaveAdpaterFile()
        {
            if (!File.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            string json = JsonConvert.SerializeObject(ModPathAdpator);
            File.WriteAllText(ModLangAdapterPath, json);
        }

        #endregion 配置修改
    }
}