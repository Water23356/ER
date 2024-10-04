using System.Collections.Generic;
using UnityEngine;

namespace STG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Entity : MonoBehaviour
    {
        private bool m_moving;
        private Rigidbody2D m_rigidbody;
        private float slowedSpeed;
        private float maxSpeed;
        private Dictionary<string, EntityAction> actions = new();

        public Rigidbody2D rigidbody
        {
            get
            {
                if (m_rigidbody == null)
                    m_rigidbody = GetComponent<Rigidbody2D>();
                return m_rigidbody;
            }
        }

        public void StopMove()
        {
            m_moving = false;
            //Animator.SetBool("move", false);
            rigidbody.velocity = Vector2.zero;
        }

        public void Move(Vector2 dir, bool slowMode = false)
        {
            m_moving = true;
            rigidbody.velocity = (slowMode ? slowedSpeed : maxSpeed) * dir;
        }

        public virtual void Shoot()
        {
            //Debug.Log("射击");
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
            if (actions.TryGetValue(key, out var act))
            {
                act?.Execute();
            }
        }
    }
}