# if UNITY_EDITOR
using Dev;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace ER.Resource.Editor
{
    /*

    /// <summary>
    /// 自动化处理资产键名和注册名工具脚本(仅处理 Assets/res/ 下的资产)
    /// </summary>
    public class RegistryAssetHandler : AssetPostprocessor
    {
        public static string DefaultGroupName = "Default Local Group";
        public static string TargetPath ="Assets/res/";

        private static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
        {
            var paths = new HashSet<string>();
            foreach (string assetPath in importedAssets)
            {
                paths.Add(assetPath);
            }
            foreach (string assetPath in movedAssets)
            {
                paths.Add(assetPath);
            }
            foreach (string assetPath in paths)
            {
                AssetHandle(assetPath);
            }
            AssetDatabase.SaveAssets(); // 保存更改
        }

        private static void AssetHandle(string assetPath)
        {
            // 检查是否需要处理的资产
            if (ShouldModifyAddressable(assetPath))
            {
                ModifyAddressableAssetAddress(assetPath); // 修改地址
                                                          // 检查是否是 ScriptableObject 并修改其属性
                if (IsScriptableObject(assetPath))
                {
                    ModifyScriptableObjectRegistryName(assetPath, GR.ADToRegistryName(assetPath));
                }
            }
        }


        // 判断资产是否需要修改
        private static bool ShouldModifyAddressable(string path)
        {
            if (!path.StartsWith(TargetPath)) return false;
            //跳过文件夹
            if (AssetDatabase.IsValidFolder(path))return false;
            return true;
        }

        // 修改 Addressable 地址
        private static void ModifyAddressableAssetAddress(string assetPath)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            AddressableAssetEntry entry = settings.FindAssetEntry(guid);

            if (entry != null)
            {
                entry.SetAddress(GR.ADToKey(assetPath));
                settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, true);
                
                Debug.Log($"- 更新Addressable地址: {assetPath} -> {entry.address}");
            }
            //else
            //{
            //    Debug.LogWarning($"找不到该资产: '{assetPath}'.");
            //}
        }

        // 检查资产是否是 ScriptableObject
        private static bool IsScriptableObject(string assetPath)
        {
            // 加载资产并检查其类型
            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            return asset is ScriptableObject;
        }

        // 修改 ScriptableObject 的 registryName 属性
        private static void ModifyScriptableObjectRegistryName(string assetPath, string newRegistryName)
        {
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

            if (obj != null)
            {
                SerializedObject serializedObject = new SerializedObject(obj);
                SerializedProperty registryNameProperty = serializedObject.FindProperty("m_registryName");

                if (registryNameProperty != null)
                {
                    SerializedProperty head = registryNameProperty.FindPropertyRelative("Head");
                    head.stringValue=
                    registryNameProperty.stringValue = newRegistryName;
                    serializedObject.ApplyModifiedProperties(); // 保存更改
                    Debug.Log($"- 更新 registryName :'{assetPath}' -> '{newRegistryName}'");
                }
                else
                {
                    Debug.LogWarning($"未找到 registryName 属性: '{assetPath}'");
                }
            }
            else
            {
                Debug.LogWarning($"加载 ScriptableObject 失败: '{assetPath}'");
            }
        }


        /*

        /// <summary>
        /// 在资产发生变动时执行
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            // 检查移动后的资产
            for (int i = 0; i < movedAssets.Length; i++)
            {
                AssetRNReset(movedAssets[i]);
            }

            // 检查新导入的资产
            
            foreach (string path in importedAssets)
            {
                AssetRNReset(path);
            }
            AssetDatabase.SaveAssets();//重新写入本地
        }

        private static void AssetRNReset(string path)
        {
            // 只处理特定目录下的资产
            if (!path.StartsWith(TargetPath)) return;

            //跳过文件夹
            if (AssetDatabase.IsValidFolder(path))
                return;

            // 获取当前的 AddressableAssetSettings 实例
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            if (settings == null)
            {
                Debug.LogError("Unable to find AddressableAssetSettings.");
                return;
            }

            // 查找或创建资源所在的 Addressable 组
            string groupName = "ImportedAssets"; // 根据需要改变分组名
            AddressableAssetGroup group = settings.FindGroup(groupName);
            if (group == null)
            {
                group = settings.CreateGroup(groupName, false, false, true, null);
                group.AddSchema<BundledAssetGroupSchema>();
                group.AddSchema<ContentUpdateGroupSchema>();
            }


            // 获取或创建 Addressable 资源条目
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(
                AssetDatabase.AssetPathToGUID(path),
                group
            );

            if (entry != null)
            {
                // 生成 Addressable Key，去掉文件后缀 和 前缀
                entry.address = GR.ADToKey(path);

                // 保存更改
                settings.SetDirty(
                    AddressableAssetSettings.ModificationEvent.EntryModified,
                    entry,
                    true
                );
                Debug.Log($"更新资源地址: {path} -> {entry.address}");
            }

            // 加载位于此路径的ScriptableObject
            var asset = AssetDatabase.LoadAssetAtPath<BaseAssetConfigure>(path);
            if (asset != null)
            {
                string key = GR.ADToKey(path);
                string registryName = GR.KeyToRegistryName(key);

                asset.RegistryName = registryName;
                //AssetUtility.ConfigureAsset(path, key, DefaultGroupName);
                //EditorUtility.SetDirty(asset); // 标记资产已改变，确保变更被保存
            }
        }
    }*/
}
#endif
