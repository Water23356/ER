using Dev;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

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
                string registryName = GR.ADToRegName(assetPath);
                EditorGUIUtility.systemCopyBuffer = registryName;
                Debug.Log($"已复制到剪贴板: {registryName}");
            }
        }

        private void CopyAssetPathToClipboard(Object target)
        {
            string assetPath = AssetDatabase.GetAssetPath(target);
            string registryName = GR.ADToRegName(assetPath);
            EditorGUIUtility.systemCopyBuffer = registryName;
            Debug.Log($"已复制到剪贴板: {registryName}");
        }

        

        // 只在选择 Texture2D 资源时显示右键菜单
        [MenuItem("Assets/创建为精灵图集", true)]
        private static bool ValidateCreateSpriteAtlas()
        {
            // 只允许当选中的是 Texture2D 时右键菜单可见
            return Selection.activeObject is Texture2D;
        }


        // 右键菜单项：为选中的 Texture2D 创建同名的 SpriteAtlas
        [MenuItem("Assets/创建为精灵图集")]
        private static void CreateSpriteAtlasFromTexture()
        {
            // 获取当前选中的 Texture2D 资源
            Texture2D selectedTexture = Selection.activeObject as Texture2D;
            if (selectedTexture == null)
            {
                Debug.LogWarning("No Texture2D selected.");
                return;
            }

            // 获取选中的 Texture2D 的路径
            string texturePath = AssetDatabase.GetAssetPath(selectedTexture);
            string directory = Path.GetDirectoryName(texturePath);
            string textureName = Path.GetFileNameWithoutExtension(texturePath);

            // 创建 SpriteAtlas 资源路径
            string atlasPath = Path.Combine(directory, textureName + ".spriteatlas");

            // 检查 SpriteAtlas 是否已经存在
            SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
            if (spriteAtlas == null)
            {
                // 如果不存在，则创建一个新的 SpriteAtlas
                spriteAtlas = new SpriteAtlas();
                AssetDatabase.CreateAsset(spriteAtlas, atlasPath);
                Debug.Log("已创建精灵图集: " + atlasPath);
            }

            // 获取选中 Texture2D 的所有精灵对象
            Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath);
            foreach (Object obj in sprites)
            {
                if (obj is Sprite)
                {
                    // 将精灵添加到 SpriteAtlas
                    spriteAtlas.Add(new[] { obj });
                }
            }

            // 可选设置：配置 SpriteAtlas 的打包设置
            SpriteAtlasPackingSettings packingSettings = spriteAtlas.GetPackingSettings();
            packingSettings.enableTightPacking = true;  // 启用紧凑打包
            spriteAtlas.SetPackingSettings(packingSettings);

            // 保存更改
            AssetDatabase.SaveAssets();
            Debug.Log("已从 " + selectedTexture.name + " 提取精灵图到 " + textureName + ".spriteatlas");
        }
    }
}