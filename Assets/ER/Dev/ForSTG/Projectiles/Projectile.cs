
using UnityEngine;

namespace ER.STG
{
    /// <summary>
    /// 射弹:
    /// Rigidbody 配置: 动力学(Kinematic)
    /// Collider2D 配置: 触发器(isTrigger)
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Projectile : Water
    {
        /// <summary>
        /// 伤害值
        /// </summary>
        private float damage;

        /// <summary>
        /// 穿透(耐久)
        /// </summary>
        private float durable;

        /// <summary>
        /// 伤害计时器标签(用于做伤害无敌帧, 同标签的伤害共用一个计时器)
        /// </summary>
        private string damageTimerTag;

        private SpriteRenderer m_spriteRenderer;

        private Rigidbody2D m_rigidbody;

        public float Durable { get => durable; set => durable = value; }
        public string DamageTimerTag { get => damageTimerTag; set => damageTimerTag = value; }
        public float Damage { get => damage; set => damage = value; }
        public Color Color { get => Renderer.color; set => Renderer.color = value; }

        public Vector2 Speed
        {
            get
            {
                return Rigidbody.velocity;
            }
            set
            {
                Rigidbody.velocity = value;
            }
        }

        public SpriteRenderer Renderer
        { get { if (m_spriteRenderer == null) m_spriteRenderer = GetComponent<SpriteRenderer>(); return m_spriteRenderer; } }

        public Rigidbody2D Rigidbody
        {
            get
            {
                if (m_rigidbody == null)
                    m_rigidbody = GetComponent<Rigidbody2D>();
                return m_rigidbody;
            }
        }

        private void Start()
        {
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log($"子弹接触: {collision.tag}");
            if (collision.CompareTag("Edge"))
            {
                Destroy();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {

        }

    }
}