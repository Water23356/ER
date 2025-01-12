using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D.Components
{
    /// <summary>
    /// 弹幕
    /// </summary>
    public abstract class Projectile : Water, IProjectile
    {
        private HitBox m_hitbox;

        public HitBox hitbox { get => m_hitbox; private set => m_hitbox = value; }

        /// <summary>
        /// 初始化事件(如果实体存在其他初始化组件使用该事件接口)
        /// </summary>
        public event Action OnInit;

        private void Awake()
        {
            hitbox = GetComponent<HitBox>();
        }

        /// <summary>
        /// 释放该对象
        /// </summary>
        protected void Release()
        {
            Destroy();
        }

        /// <summary>
        /// 发射
        /// </summary>
        /// <param name="startPos">初始位置</param>
        /// <param name="dir">初始方向向量</param>
        /// <param name="parameters">其他参数</param>
        public abstract void Shoot(Vector2 startPos, Vector2 dir = default, Dictionary<string, object> parameters = null);

        public override void OnGetFormPool()
        {
            ResetState();
        }

        public override void ResetState()
        {
            Init();
            OnInit?.Invoke();
        }

        /// <summary>
        /// 初始化: 参数重置
        /// </summary>
        protected abstract void Init();
    }

    public interface IProjectile
    {
        public void Shoot(Vector2 startPos, Vector2 dir = default, Dictionary<string, object> parameters = null);
    }
}