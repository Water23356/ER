using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;  

/// <summary>
/// 用于精灵图集打包
/// </summary>
public class SpriteAtlasTool : EditorWindow
{
    private string atlasName = "NewSpriteAtlas";  // SpriteAtlas 的名称
    private SpriteAtlas spriteAtlas;              // 存储 SpriteAtlas 实例
    private List<Sprite> selectedSprites = new List<Sprite>();  // 存储选中的 Sprites

    // 创建菜单项
    [MenuItem("Tools/精灵图集封包器")]
    public static void ShowWindow()
    {
        // 创建窗口
        GetWindow<SpriteAtlasTool>("精灵图集封包器");
    }

    // 窗口绘制
    private void OnGUI()
    {
        GUILayout.Label("精灵图集封包器", EditorStyles.boldLabel);

        // SpriteAtlas 名称输入框
        atlasName = EditorGUILayout.TextField("Atlas Name", atlasName);

        // 显示选中的精灵
        if (selectedSprites.Count > 0)
        {
            GUILayout.Label("Selected Sprites:");
            foreach (var sprite in selectedSprites)
            {
                GUILayout.Label(sprite.name);
            }
        }

        // 按钮：选择精灵资源
        if (GUILayout.Button("Select Sprites"))
        {
            SelectSprites();
        }

        // 按钮：打包精灵到 SpriteAtlas
        if (GUILayout.Button("Pack into Sprite Atlas"))
        {
            PackSelectedSprites();
        }
    }

    // 从 Project 视图中选择精灵资源
    private void SelectSprites()
    {
        selectedSprites.Clear();
        Object[] selectedObjects = Selection.objects;

        // 遍历选中的对象，筛选出 Sprite 类型的资源
        foreach (Object obj in selectedObjects)
        {
            if (obj is Sprite)
            {
                selectedSprites.Add(obj as Sprite);
            }
        }

        // 刷新窗口
        Repaint();
    }

    // 将选中的精灵打包进 SpriteAtlas
    private void PackSelectedSprites()
    {
        if (selectedSprites.Count == 0)
        {
            Debug.LogWarning("No sprites selected for packing.");
            return;
        }

        // 检查是否已经有这个名称的 SpriteAtlas
        string atlasPath = "Assets/" + atlasName + ".spriteatlas";
        spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);

        if (spriteAtlas == null)
        {
            // 如果没有现成的 Atlas，创建一个新的
            spriteAtlas = new SpriteAtlas();
            AssetDatabase.CreateAsset(spriteAtlas, atlasPath);
        }

        // 将选中的精灵添加到 SpriteAtlas 中
        Object[] packables = new Object[selectedSprites.Count];
        for (int i = 0; i < selectedSprites.Count; i++)
        {
            packables[i] = selectedSprites[i];
        }

        // 添加精灵到打包对象中
        spriteAtlas.Add(packables);

        // 设置图集的打包设置（可选）
        SpriteAtlasPackingSettings packingSettings = spriteAtlas.GetPackingSettings();
        packingSettings.enableTightPacking = true;  // 紧凑打包
        spriteAtlas.SetPackingSettings(packingSettings);

        // 设置图集的压缩设置（可选）
        TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings();
        platformSettings.overridden = true;
        platformSettings.format = TextureImporterFormat.Automatic;
        spriteAtlas.SetPlatformSettings(platformSettings);

        // 保存更改
        AssetDatabase.SaveAssets();
        Debug.Log("Selected sprites have been packed into " + atlasName);
    }
}