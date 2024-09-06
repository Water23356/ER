using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.STG
{
    [Serializable]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Role : MonoBehaviour, IRole, ITakeDamagable
    {
        [SerializeField]
        private RoleAction[] actionLoads;

        [SerializeField]
        private float m_health = 10;//生命值

        [SerializeField]
        private int m_resist = 1;//抵抗(对伤害域的损伤)

        [SerializeField]
        private float m_speed = 10;

        [SerializeField]
        private float m_speedSlowMode = 10;

        private bool m_moving = false;

        private Dictionary<string, RoleAction> m_actions;

        private Rigidbody2D m_rigidbody;
        private Animator m_animator;
        private SpriteRenderer m_renderer;

        public bool IsMoving => m_moving;

        public Rigidbody2D Rigidbody
        {
            get
            {
                if (m_rigidbody == null)
                    m_rigidbody = GetComponent<Rigidbody2D>();
                return m_rigidbody;
            }
        }

        public float Health { get => m_health; set => m_health = value; }
        public float Speed { get => m_speed; set => m_speed = value; }
        public float SpeedSlowMode { get => m_speedSlowMode; set => m_speedSlowMode = value; }
        public int Resist { get => m_resist; set => m_resist = value; }

        public Dictionary<string, RoleAction> Actions
        {
            get
            {
                if (m_actions == null)
                    m_actions = new Dictionary<string, RoleAction>();
                return m_actions;
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

        private void Awake()
        {
            foreach (var act in actionLoads)
            {
                act.Owner = this;
                Actions[act.ActionName] = act;
            }
            actionLoads = null;
        }

        public void StopMove()
        {
            m_moving = false;
            Animator.SetBool("move", false);
            Rigidbody.velocity = Vector2.zero;
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
            Rigidbody.velocity = (slowMode ? SpeedSlowMode : Speed) * dir;
        }

        public virtual void Shoot()
        {
            Debug.Log("射击");
            ExecuteAction("shoot");
        }

        public virtual void Bomb()
        {
            ExecuteAction("bomb");
        }

        public virtual void Action(int index)
        {
            ExecuteAction("action_" + index);
        }

        private void ExecuteAction(string key)
        {
            if (Actions.TryGetValue(key, out var act))
            {
                act?.Execute();
            }
        }

        public DamageBackInfo TakeDamage(DamageInfo info)
        {
            Health -= info.damage;
            return new DamageBackInfo() { durableNeed = Resist };
        }

        public virtual void Dead()
        { }
    }
}