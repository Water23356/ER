using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static TreeEditor.TextureAtlas;

namespace ER.ResourceManager
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
            if (GUILayout.Button("创建索引表"))
            {
                string path = AssetDatabase.GetAssetPath(table);
                string directory = Path.GetDirectoryName(path);
                string name = Path.GetFileNameWithoutExtension(path);

                string newTablePath = Path.Combine(directory, name+"_indexer" + ".asset");

                MetaIndexDic dic = AssetDatabase.LoadAssetAtPath<MetaIndexDic>(newTablePath);
                if (dic == null)
                {
                    dic = new MetaIndexDic();
                    AssetDatabase.CreateAsset(dic, newTablePath);
                    Debug.Log("已创建索引表: " + newTablePath);
                }
                dic.indexes = new MetaIndexDic.Row[table.rows.Count];
                for (int i = 0; i < dic.indexes.Length; i++)
                {
                    dic.indexes[i] = new MetaIndexDic.Row()
                    {
                        regName = table.rows[i].regName,
                        loadPath = table.rows[i].loadPath,
                    };
                }

                AssetDatabase.SaveAssets();
                Debug.Log("已从 " + table.name + " 提取索引到 " + name + ".spriteatlas");
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