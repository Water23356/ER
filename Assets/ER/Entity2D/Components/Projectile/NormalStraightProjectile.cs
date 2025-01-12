using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D.Components
{
    public class NormalStraightProjectile : Projectile, IVelocityObject
    {
        [SerializeField]
        [DisplayLabel("速度设定")]
        private float m_speed;

        [SerializeField]
        [DisplayLabel("速度")]
        private Vector2 m_velocity;

        [Header("限制器")]
        [SerializeField]
        [DisplayLabel("时间限制有效")]
        private bool m_timeLimitEnable = false;
        [SerializeField]
        [DisplayLabel("时间限制")]
        private float m_timeLimit;
        [SerializeField]
        [DisplayLabel("距离限制有效")]
        private bool m_distanceEnable = false;
        [SerializeField]
        [DisplayLabel("距离限制")]
        private float m_distanceLimit;


        private float timer;
        private Vector2 startPos;

        public float speed
        {
            get => m_speed;
            set
            {
                m_speed = value;
                velocity = velocity.normalized * m_speed;
            }
        }

        public bool timeLimitEnable
        {
            get => m_timeLimitEnable;
            set => m_timeLimitEnable = value;
        }
        public bool distanceEnable
        {
            get => m_distanceEnable;
            set => m_distanceEnable = value;
        }
        public float timeLimit
        {
            get => m_timeLimit;
            set => m_timeLimit = value;
        }
        public float distanceLimit
        {
            get => m_distanceLimit;
            set => m_distanceLimit = value;
        }
        public Vector2 velocity { get => m_velocity; set => m_velocity = value; }

        protected override void Init()
        {
            speed = 0;
        }

        public override void Shoot(Vector2 startPos, Vector2 dir = default, Dictionary<string, object> parameters = null)
        {
            if (parameters != null)
            {
                if (parameters.TryGetValue("speed", out object value))
                {
                    if (value is float)
                    {
                        speed = (float)value;
                    }
                }
            }

            transform.position = startPos;
            velocity = dir.normalized * speed;

            timer = timeLimit;
            this.startPos = startPos;
        }

        private void FixedUpdate()
        {
            transform.position += (Vector3)velocity * Time.fixedDeltaTime;
            if(timeLimitEnable)
            {
                timer -= Time.deltaTime;
                if (timer < 0) Destroy();
            }
            if(distanceEnable)
            {
                if (Vector2.Distance(transform.position, startPos) > distanceLimit)
                    Destroy();
            }
        }
    }
}