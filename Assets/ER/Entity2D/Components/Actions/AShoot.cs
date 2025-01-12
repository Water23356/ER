using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D.Components
{
    public class AShoot : ActionBase
    {
        [SerializeField]
        [DisplayLabel("预制体池名称")]
        private string m_poolName;

        private WaterPool pool;

        [SerializeField]
        [DisplayLabel("射击冷却")]
        private float m_shootCD = 0.5f;

        [SerializeField]
        [DisplayLabel("射击状态")]
        private bool m_shootSatte = false;

        [SerializeField]
        [DisplayLabel("子弹速度")]
        private float speed = 10f;

        [SerializeField]
        [DisplayLabel("射击方向")]
        private Vector2 m_shootDir;

        private float shootTimer = 0f;

        /// <summary>
        /// 射击冷却时间
        /// </summary>
        public float shootCD
        {
            get => m_shootCD;
            set => m_shootCD = value;
        }

        /// <summary>
        /// 射击状态
        /// </summary>
        public bool shootSatte
        {
            get => m_shootSatte;
            set => m_shootSatte = value;
        }

        /// <summary>
        /// 射击方向
        /// </summary>
        public Vector2 shootDir
        {
            get => m_shootDir;
            set => m_shootDir = value;
        }

        public string poolName
        {
            get => m_poolName;
            set => m_poolName = value;
        }

        public override void Init()
        {
            pool = WaterPoolManager.Instance.GetPool(poolName);
        }

        public override void Entry()
        {
            shootSatte = true;
        }

        public override void Exit()
        {
            shootSatte = false;
        }

        private void Update()
        {
            if (shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            else
            {
                if (shootSatte) Shoot();
            }
        }

        private void Shoot()
        {
            shootTimer = shootCD;
            var bullet = (Projectile)pool.GetObject();
            if (bullet == null) return;
            bullet.Shoot(transform.position, shootDir.normalized, new Dictionary<string, object>()
            {
                {"speed",speed }
            });
        }
    }
}