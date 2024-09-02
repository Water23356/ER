using UnityEngine;
#if UNITY_EDITOR

using UnityEditor; // 导入Unity编辑器命名空间
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif


namespace ER.Resource.Editor
{
# if UNITY_EDITOR
    public static class AssetUtility
    {
        /// <summary>
        /// 获取指定ScriptableObject资产的路径
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static string GetAssetPath(ScriptableObject asset)
        {
            return AssetDatabase.GetAssetPath(asset);
        }

        /// <summary>
        /// 修改指定地址化资源的键名
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="newKey"></param>
        public static void ChangeAssetKey(Object asset, string newKey)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            var ad = settings.FindAssetEntry(asset.GetInstanceID().ToString());
            ad.SetAddress(newKey);

            // 保存设置
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 修正资源地址名
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static void CorrectAddress(ScriptableObject asset)
        {
            string path = GetAssetPath(asset);
            ChangeAssetKey(asset, CorrectString(path));
        }

        private static string CorrectString(string origin)
        {
            string ignore = "Assets/res/";
            string content = origin.Split('.')[0];
            if (content.StartsWith(ignore))
            {
                return content.Substring(ignore.Length);
            }
            return content;
        }

        /// <summary>
        /// 配置指定资产: 自动配置资产的 加载键
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="addressableKey"></param>
        /// <param name="groupName"></param>
        public static void ConfigureAsset(string assetPath, string addressableKey, string groupName)
        {
            // 获取AddressableAssetSettings对象
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            if (settings == null)
            {
                Debug.LogError("Addressable Asset Settings 未找到!");
                return;
            }

            // 检查或创建新的Group
            AddressableAssetGroup group = settings.FindGroup(groupName);
            if (group == null)
            {
                Debug.LogError("找不到指定资产分组" + groupName);
                return;
            }

            // 获取或添加资产到Addressables
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            var entry = settings.FindAssetEntry(guid) ?? settings.CreateOrMoveEntry(guid, group);
            if (entry == null)
            {
                entry = settings.CreateOrMoveEntry(guid, group);
            }

            // 设置Addressable Key
            entry.address = addressableKey;

            // 保存更改
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
