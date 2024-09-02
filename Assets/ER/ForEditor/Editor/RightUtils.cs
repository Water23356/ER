using ER.Resource;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

namespace ER.ForEditor
{
    [CustomEditor(typeof(Object))]  // 将目标类型设置为 Object 以适用于所有资产类型
    public class RightUtils : Editor
    {

        [MenuItem("Assets/操作/复制注册名到剪贴板", false, 20)]  // 为所有资产添加右键菜单项
        private static void CopyAssetPath(MenuCommand command)
        {
            var asset = Selection.activeObject;
            if (asset != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(asset);
                string registryName = GR.ADToRegistryName(assetPath);
                EditorGUIUtility.systemCopyBuffer = registryName;
                Debug.Log($"已复制到剪贴板: {registryName}");
            }
        }

        private void CopyAssetPathToClipboard(Object target)
        {
            string assetPath = AssetDatabase.GetAssetPath(target);
            string registryName = GR.ADToRegistryName(assetPath);
            EditorGUIUtility.systemCopyBuffer = registryName;
            Debug.Log($"已复制到剪贴板: {registryName}");
        }
    }
}