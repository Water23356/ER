using ER.STG.Tracks;
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
        public Color Color { get => renderer.color; set => renderer.color = value; }
        public Vector2 velocity { get => rigidbody.velocity; set => rigidbody.velocity = value; }

        public Vector2 Speed
        {
            get
            {
                return rigidbody.velocity;
            }
            set
            {
                rigidbody.velocity = value;
            }
        }

        public SpriteRenderer renderer
        { get { if (m_spriteRenderer == null) m_spriteRenderer = GetComponent<SpriteRenderer>(); return m_spriteRenderer; } }

        public Rigidbody2D rigidbody
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
            if (collision.CompareTag("ProjectileEdge"))
            {
                Destroy();
            }
            else if(collision.CompareTag(tag))
            {
                //和自身标签相同则不发生检测
            }
            else//造成伤害
            {

            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
        }
        /// <summary>
        /// 获取或者创建轨迹组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetOrCreateTrack<T>() where T: ProjectileTrack
        {
            return transform.GetOrAddComponent<T>();
        }
        /// <summary>
        /// 禁用所有轨迹组件
        /// </summary>
        public void DisableAllTack()
        {
            ProjectileTrack[] tracks = GetComponents<ProjectileTrack>();
            foreach (ProjectileTrack track in tracks)
            {
                track.enabled = false;
            }
        }
    }
}