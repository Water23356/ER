using ER.Resource;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ER.ForEditor
{
    public class ForRightItems
    {
        [MenuItem("Assets/以[表编辑器]打开", false, 20)]  // 为所有资产添加右键菜单项
        private static void CopyAssetPath(MenuCommand command)
        {
            var asset = Selection.activeObject;
            if (asset == null) return;
            string path = AssetDatabase.GetAssetPath(asset);
            if (AssetDatabase.IsValidFolder(path))
            {
                ScriptableObjectEditorWindow.ShowWindow(path);
                return;
            }
             
            var direct = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(direct))
            {
                ScriptableObjectEditorWindow.ShowWindow(direct);
            }
        }
    }
}