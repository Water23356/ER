using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Dev
{
    // 新增的自定义编辑器类
    [CustomEditor(typeof(RefDataTable))]
    public class DataTableEditorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RefDataTable table = (RefDataTable)target;
            if (GUILayout.Button("打开编辑窗口"))
            {
                DataTableEditorWindow.OpenWindow(table);
            }
        }

        [MenuItem("Assets/添加进当前加载索引表")]
        private static void MyCustomFunction()
        {
            var window = EditorWindow.GetWindow<DataTableEditorWindow>();
            if (window != null)
            {
                Object[] selectedAssets = Selection.objects;

                List<Object> objs = new List<Object>();

                foreach (Object selectedAsset in selectedAssets)
                {
                    if (selectedAsset == null) continue;
                    string assetPath = AssetDatabase.GetAssetPath(selectedAsset);
                    //Debug.Log($"检查资产: {assetPath} 是否为文件夹: {IsFolder(selectedAsset)}");
                    if (!IsFolder(selectedAsset))
                    {
                        objs.Add(selectedAsset);
                    }
                }
                window.AddRefData(objs);
                return;
            }
            Debug.Log("未打开索引表编辑窗口");
        }

        private static bool IsFolder(Object asset)
        {
            // 检查资产是否是文件夹
            return asset is DefaultAsset && AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(asset));
        }

        public static bool IsWindowOpen<T>() where T : EditorWindow
        {
            return EditorWindow.GetWindow<T>() != null;
        }
    }
}