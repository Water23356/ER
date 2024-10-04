using UnityEngine;

namespace STG
{
    public class Killable : MonoBehaviour, IEntityCompenont
    {
        private int m_health;
        private int m_health_max;
        private int m_defence;
        private int m_block;
        private int m_wastage;

        public int health
        {
            get => m_health; set => m_health = value;
        }

        public int health_max { get => m_health_max; set => m_health_max = value; }
        public int defence { get => m_defence; set => m_defence = value; }
        public int block { get => m_block; set => m_block = value; }
        public int wastage { get => m_wastage; set => m_wastage = value; }

        public LuaHandler GetAdpator()
        {
            return new LuaHandler(this);
        }

        public struct LuaHandler
        {
            private Killable origin;

            public LuaHandler(Killable origin)
            {
                this.origin = origin;
            }

            public int health { get => origin.health; set => origin.health = value; }

            public int health_max { get => origin.health_max; set => origin.health_max = value; }
            public int defence { get => origin.defence; set => origin.defence = value; }
            public int block { get => origin.block; set => origin.block = value; }
            public int wastage { get => origin.wastage; set => origin.wastage = value; }

            public bool enabled { get => origin.enabled; set => origin.enabled = value; }
        }
    }
}