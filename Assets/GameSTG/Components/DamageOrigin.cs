using UnityEngine;

namespace STG
{
    public class DamageOrigin:MonoBehaviour,IEntityCompenont
    {
        private int m_damage;
        
        public int damage
        {
            get=>m_damage; set => m_damage = value;
        }

        public LuaHandler GetAdpator()
        {
            return new LuaHandler(this);
        }

        public struct LuaHandler
        {
            private DamageOrigin origin;
            public LuaHandler(DamageOrigin origin)
            {
                this.origin = origin;
            }
            public int damage
            {
                get => origin.damage; set => origin.damage = value;
            }
            public bool enabled
            {
                get=>origin.enabled; set => origin.enabled = value;
            }
        }
    }
}