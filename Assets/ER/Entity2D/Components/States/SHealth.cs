using ER.ForEditor;
using System;
using UnityEngine;

namespace ER.Entity2D.Components
{
    public class SHealth:StateBase
    {
        [SerializeField]
        [DisplayLabel("当前生命值")]
        private float m_health;
        [SerializeField]
        [DisplayLabel("最大生命值")]
        private float m_maxHealth;

        public float health { get => m_health; set => m_health = value; }
        public float maxHealth { get => m_maxHealth; set => m_maxHealth = value; }

        public bool SetHealth(float health)
        {
            this.health = health;
            return health <= 0;
                
        }
        public bool ModifyHealth(float changeValue)
        {
            health += changeValue;
            return health <= 0;
        }
    }

}