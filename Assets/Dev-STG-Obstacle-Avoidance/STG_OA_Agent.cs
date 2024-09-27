using ER.ForEditor;
using UnityEngine;

namespace Dev2
{
    /// <summary>
    /// 障碍代理
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class STG_OA_Agent : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("如果无法计算碰撞体半径, 将会使用这个值")]
        [DisplayLabel("默认半径")]
        private float defaultRadius = 1.0f;

        private Collider2D m_collider;

        private Collider2D collider
        {
            get
            {
                if (m_collider == null)
                    m_collider = GetComponent<Collider2D>();
                return m_collider;
            }
        }

        public Vector2 velocity
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

        public float radius
        {
            get
            {
                if (collider == null)
                    return defaultRadius;
                var circle = collider as CircleCollider2D;
                if (circle != null)
                    return circle.radius;

                var box = collider as BoxCollider2D;
                if (box != null)
                {
                    return Mathf.Abs(box.size.x*velocity.normalized.x)* Mathf.Abs(box.size.y * velocity.normalized.y);
                }

                return defaultRadius;
            }
        }

        private Rigidbody2D m_rigidbody;

        public Rigidbody2D rigidbody
        {
            get
            {
                if (m_rigidbody == null)
                    m_rigidbody = GetComponent<Rigidbody2D>();
                return m_rigidbody;
            }
        }

        private void OnDrawGizmosSelected()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawSphere(transform.position, defaultRadius);
        }
    }
}