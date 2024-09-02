using System.Collections.Generic;
using UnityEngine;

namespace ER.Resource
{
    public static class LangText
    {

        public static string GetLangText(string fullName, string defaultValue)
        {
            string[] parts = fullName.Split('~');
            if (parts.Length != 3)
            {
                Debug.LogError($"输入文本注册名格式不正确: {fullName}");
                return defaultValue;
            }
            return GetLangText(parts[0], parts[1], parts[2], defaultValue);
        }
        public static string GetLangText(string assetName, string @namespace, string key, string defaultValue)
        {
            var resLang = GR.Get<TextResource>(assetName.Trim());
            if (resLang == null)
            {
                Debug.LogWarning($"获取文本资源失败: {assetName}~{@namespace}~{key}");
                return defaultValue;
            }
            if (resLang.GetDictionary().TryGetValue(@namespace.Trim(), out var dic))
            {
                return dic.GetValueOrDefault(key.Trim(), defaultValue);
            }
            else
            {
                Debug.LogWarning($"获取文本资源失败: {assetName}~{@namespace}~{key}");
                return defaultValue;
            }
        }
    }
}