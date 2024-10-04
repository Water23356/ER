using ER.ResourceManager;
using UnityEngine;

namespace STG
{
    public class SpriteAtlasAnimator:MonoBehaviour,IEntityCompenont
    {

    }



    public struct AtlasAnimationClip
    {
        private SpriteAtlasResource m_atlas;
        private float m_space;
        private bool m_loop;

        public SpriteAtlasResource atlas { get => m_atlas; set => m_atlas = value; }
        public float space { get => m_space; set => m_space = value; }
        public bool loop { get => m_loop; set => m_loop = value; }
    }
}