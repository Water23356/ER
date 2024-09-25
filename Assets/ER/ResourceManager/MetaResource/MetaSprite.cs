using ER.ForEditor;
using UnityEngine;

namespace ER.ResourceManager
{
    /// <summary>
    /// 精灵图元数据
    /// </summary>
    [CreateAssetMenu(fileName = "MetaSprite", menuName = "ER/元/精灵图")]
    [MetaTable("sprite")]
    public class MetaSprite : AssetModifyConfigure
    {
        public override string metaHead => "sprite";

        [DisplayLabel("颜色")]
        public Color color = new Color(1, 1, 1, 1);

        [Tooltip("为空表示使用默认")]
        [DisplayLabel("替换材质")]
        public Material material;

        [DisplayLabel("绘制模式")]
        public SpriteDrawMode drawMode;

        [DisplayLabel("水平翻转")]
        public bool flipX;

        [DisplayLabel("垂直翻转")]
        public bool flipY;

        public override T Get<T>()
        {
            return default(T);
        }
    }
}