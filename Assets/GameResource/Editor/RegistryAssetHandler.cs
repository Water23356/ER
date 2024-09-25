using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Dev
{
    /// <summary>
    /// 自动化处理资产键名和注册名工具脚本(仅处理 Assets/res/ 下的资产)
    /// </summary>
    public class RegistryAssetHandler : AssetPostprocessor
    {
        public static string DefaultGroupName = "Default Local Group";
        public static string TargetPath = "Assets/res/";

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
                    ModifyScriptableObjectRegistryName(assetPath, GR.ADToRegName(assetPath));
                }
            }
        }

        // 判断资产是否需要修改
        private static bool ShouldModifyAddressable(string path)
        {
            if (!path.StartsWith(TargetPath)) return false;
            //跳过文件夹
            if (AssetDatabase.IsValidFolder(path)) return false;
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
                    var regName = new RegistryName(newRegistryName);
                    SerializedProperty head = registryNameProperty.FindPropertyRelative("head");
                    head.stringValue = regName.Head;
                    SerializedProperty mod = registryNameProperty.FindPropertyRelative("module");
                    mod.stringValue = regName.Module;
                    SerializedProperty path = registryNameProperty.FindPropertyRelative("path");
                    path.stringValue = regName.Path;
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
    }
}