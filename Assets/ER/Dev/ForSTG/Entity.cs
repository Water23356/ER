using UnityEngine;

namespace ER.STG
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Entity : MonoBehaviour
    {

        [SerializeField]
        private float m_speed = 10;

        [SerializeField]
        private float m_speedSlowMode = 10;

        private bool m_moving = false;

        private Rigidbody2D m_rigidbody;
        private Animator m_animator;
        private SpriteRenderer m_renderer;

        
        public float Speed { get => m_speed; set => m_speed = value; }

        public bool IsMoving => m_moving;

        public float SpeedSlowMode { get => m_speedSlowMode; set => m_speedSlowMode = value; }

        public Rigidbody2D rigidbody
        {
            get
            {
                if (m_rigidbody == null)
                    m_rigidbody = GetComponent<Rigidbody2D>();
                return m_rigidbody;
            }
        }

        public Animator Animator
        {
            get
            {
                if (m_animator == null)
                    m_animator = GetComponent<Animator>();
                return m_animator;
            }
        }

        public SpriteRenderer Renderer
        {
            get
            {
                if (m_renderer == null)
                    m_renderer = GetComponent<SpriteRenderer>();
                return m_renderer;
            }
        }

        public Vector2 velocity
        {
            get=>rigidbody.velocity;
            set => rigidbody.velocity = value;
        }

        public void StopMove()
        {
            m_moving = false;
            Animator.SetBool("move", false);
            velocity = Vector2.zero;
        }

        public void Move(Vector2 dir, bool slowMode = false)
        {
            m_moving = true;
            if (Mathf.Abs(dir.x) < 0.1)
            {
                Animator.SetBool("move", false);
            }
            else if (dir.x > 0)
            {
                Renderer.flipX = true;
                Animator.SetBool("move", true);
            }
            else
            {
                Renderer.flipX = false;
                Animator.SetBool("move", true);
            }
            velocity = (slowMode ? SpeedSlowMode : Speed) * dir;
        }

    }
}