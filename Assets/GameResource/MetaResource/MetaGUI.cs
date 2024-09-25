using ER.ForEditor;
using ER.GUI;
using UnityEngine;

namespace Dev
{
    /// <summary>
    /// 精灵图元数据
    /// </summary>
    [CreateAssetMenu(fileName = "MetaGUI", menuName = "ER/元/GUI")]
    [MetaTable("gui")]
    public class MetaGUI : AssetModifyConfigure
    {
        public override string metaHead => "gui";

        [DisplayLabel("排序层级")]
        public GUIPanel.GUILayer layer;


        public override T Get<T>()
        {
            return default(T);
        }
    }
}