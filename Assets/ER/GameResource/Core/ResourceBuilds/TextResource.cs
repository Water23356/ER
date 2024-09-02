using ER.ForEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Resource
{
    [CreateAssetMenu(fileName = "Text", menuName = "创建可配置资产/游戏文本资产", order = 1)]
    public class TextResource : BaseAssetConfigure
    {
        public List<TextPart> content;
        private Dictionary<string, Dictionary<string, string>> dic;

        /// <summary>
        /// 清除字典缓存
        /// </summary>
        public void ClearTemp()
        {
            dic.Clear();
        }

        public Dictionary<string, Dictionary<string, string>> GetDictionary()
        {
            if (dic == null)
            {
                dic = new Dictionary<string, Dictionary<string, string>>();
            }
            if (dic.Count == 0)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    dic[content[i].key] = content[i].GetDictionary();
                }
            }
            return dic;
        }
    }

    [Serializable]
    public struct TextPart
    {
        [GetFromRegistryName]
        public string key;
        public List<StringPair> parts;

        private Dictionary<string, string> dic;

        /// <summary>
        /// 清除字典缓存
        /// </summary>
        public void ClearTemp()
        {
            dic.Clear();
        }

        public Dictionary<string, string> GetDictionary()
        {
            if (dic == null)
            {
                dic = new Dictionary<string, string>();
            }
            if (dic.Count == 0)
            {
                for (int i = 0; i < parts.Count; i++)
                {
                    dic[parts[i].key] = parts[i].value;
                }
            }
            return dic;
        }

        public string this[string key]
        {
            get
            {
                for (int i = 0; i < parts.Count; i++)
                {
                    if (parts[i].key == key)
                        return parts[i].value;
                }
                Debug.LogError($"[StringPair]:取值失败: {key}");
                return null;
            }
        }
    }
}