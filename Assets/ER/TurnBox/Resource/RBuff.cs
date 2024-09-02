using ER.ForEditor;
using ER.Resource;
using UnityEngine;

namespace ER.TurnBox
{
    [CreateAssetMenu(fileName = "RBuff", menuName = "资源配置表/Buff效果")]
    public class RBuff : BaseAssetConfigure
    {
        [DisplayLabel("文本资源")]
        [GetFromRegistryName]
        public string originText;

        [DisplayLabel("标签")]
        public BuffTag Tag;

        [DisplayLabel("是否可见")]
        public bool isVisible;
        [DisplayLabel("是否为debuff")]
        public bool isDebuff;

        public string[] list;

        public string originSprite
        {
            get{
                string[] parts = registryName.Split(':');
                string path = $"img:origin:{parts[0]}/{parts[2]}";
                return path;
            }
        }

        private SpriteResource m_sprite;
        public SpriteResource Texture
        {
            get
            {
                if (m_sprite == null)
                {
                    m_sprite = GR.Get<SpriteResource>(originSprite);
                }
                return m_sprite;
            }
        }

        public Buff CreateInstance()
        {
            return BuffBuilder.Build(this);
        }

        public string GetLangText(string key,string defaultValue = "Unknwon")
        {
            return LangText.GetLangText(originText, registryName, key, defaultValue);
        }
    }
}