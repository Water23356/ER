
using System.Collections.Generic;
using UnityEngine;

namespace ER.Resource
{
    [CreateAssetMenu(fileName = "RSettingGroup", menuName = "创建可配置资产/游戏设置组", order = 1)]
    public class SettingGroupResource : BaseAssetConfigure
    {
        public List<SettingResource> settings = new List<SettingResource>();

        public Dictionary<string, Dictionary<string, string>> GetDictionary()
        {
            Dictionary<string, Dictionary<string, string>> dic =
                new Dictionary<string, Dictionary<string, string>>();
            for (int i = 0; i < settings.Count; i++)
            {
                if (settings[i] == null)
                    continue;
                dic[settings[i].key] = settings[i].GetDictionary();
            }
            return dic;
        }
    }
}
