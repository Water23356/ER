using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
# if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif


namespace ER.Resource.Editor
{
# if UNITY_EDITOR
    public class AddressableLabelsPacker
    {
        [MenuItem("资产工具/打印所有资源标签")]
        public static void LogAllLabels()
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            List<string> allLabels = settings.GetLabels();

            Debug.Log($"Total {allLabels.Count} labels found:");
            foreach (string label in allLabels)
            {
                Debug.Log(label);
            }
        }

        [MenuItem("资产工具/将资源标签封装为加载包")]
        public static void LogResourcesByLabel()
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            List<string> allLabels = settings.GetLabels();

            Debug.Log($"共找到 {allLabels.Count} 资源标签");
            foreach (string label in allLabels)
            {
                Debug.Log($"正在封装: {label}");
                PackToResource(label); 
            }
        }

        private static void PackToResource(string label)
        {
            Addressables.LoadResourceLocationsAsync(label.Trim()).Completed += ( 
                AsyncOperationHandle<IList<IResourceLocation>> handle
            ) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log($"[{label}]: 找到 {handle.Result.Count} 个资产");

                    foreach (var location in handle.Result)
                    {
                        Debug.Log($"资产名称: {location.PrimaryKey}, 资产路径: {location.InternalId}");
                    }
                    CreateCustomDataAsset(label, handle.Result);
                }
                else
                {
                    Debug.LogError("获取资产位置失败!");
                }
            };
        }

        private static void CreateCustomDataAsset(string label, IList<IResourceLocation> paths)
        {
            LoadPackResource asset = ScriptableObject.CreateInstance<LoadPackResource>();
            var task = new ER.Resource.LoadTask();
            task.clear = ER.Resource.LoadTask.ClearMode.Keep;

            asset.task = task;
            StringBuilder sb = new StringBuilder();
            foreach (var str in paths)
            {
                Debug.Log($"{str.InternalId}->{GR.ADToRegistryName(str.InternalId)}");
                sb.Append(GR.ADToRegistryName(str.InternalId));
                sb.Append('\n');
            }
            task.loads = sb.ToString();

            AssetDatabase.CreateAsset(asset, $"Assets/res/pack/{label}.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
#endif
}
