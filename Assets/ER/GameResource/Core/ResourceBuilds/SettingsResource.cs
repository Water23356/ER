using ER.ForEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Resource
{
    [CreateAssetMenu(fileName = "RSetting", menuName = "创建可配置资产/游戏设置", order = 1)]
    [Serializable]
    public class SettingResource : BaseAssetConfigure
    {
        [DisplayLabel("设置标签")]
        public string key;

        [DisplayLabel("设置项目")]
        public List<StringPair> pairs = new List<StringPair>();

        public Dictionary<string, string> GetDictionary()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < pairs.Count; i++)
            {
                dic[pairs[i].key] = pairs[i].value;
            }
            return dic;
        }
    }
}