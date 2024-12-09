using System;
using UnityEngine;

namespace ER.STG
{
    public class DamageAgent : MonoBehaviour, ITakeDamagable
    {

        [SerializeField]
        private float m_health = 10;//生命值

        [SerializeField]
        private int m_resist = 1;//抵抗(对伤害域的损伤)
        public event Action onDead;
        public float health { get => m_health; set => m_health = value; }
        public int resist { get => m_resist; set => m_resist = value; }


        public DamageBackInfo TakeDamage(DamageInfo info)
        {
            health -= info.damage;
            return new DamageBackInfo 
            {
                durableNeed = resist 
            };
        }

        public void Dead()
        {
            onDead?.Invoke();
        }
    }
}