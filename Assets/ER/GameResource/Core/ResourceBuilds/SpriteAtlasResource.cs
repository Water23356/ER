using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 精灵图集
    /// </summary>
    [CreateAssetMenu(fileName = "SpriteAtlasResource", menuName = "创建可配置资产/精灵图集资产", order = 2)]
    public class SpriteAtlasResource : BaseAssetConfigure
    {
        [SerializeField]
        private Sprite[] sprites;

        public int Counr => sprites.Length;

        public Sprite GetValue(int index)
        {
            if (index < 0 || index >= sprites.Length) return null;
            return sprites[index];
        }
    }
}