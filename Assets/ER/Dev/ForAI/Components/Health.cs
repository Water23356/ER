using ER.ForEditor;
using System;
using UnityEngine;

namespace Dev.AI
{
    public class Health : InputCell
    {
        [SerializeField]
        [DisplayLabel("生命值")]
        private float m_health = 10f;

        [SerializeField]
        [DisplayLabel("生命上限")]
        private float m_healthMax = 15f;

        [SerializeField]
        [DisplayLabel("饥饿倍率")]
        private float m_hungery = 0.5f;

        public Action onDead;

        public float health { get => m_health; set => m_health = value; }
        public float healthMax { get => m_healthMax; set => m_healthMax = value; }
        public float hungery { get => m_hungery; set => m_hungery = value; }

        public override double UpdateState()
        {
            health -= Time.deltaTime * hungery;
            if (health < 0)
            {
                onDead?.Invoke();
            }
            state = health / healthMax;
            return state;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("接触");
            var food = collision.gameObject.GetComponent<Food>();
            if (food == null) return;
            health += food.value;
            food.Eated();
        }
    }
}