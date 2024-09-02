using System;
using UnityEngine;

namespace ER.Entity2D
{
    public enum SpaceState
    {
        Land,
        Air,
        Water
    }

    public class VictimAttribute : MonoAttribute
    {
        [Header("基础")]
        [SerializeField]
        protected float health;
        [SerializeField]
        protected float healthMax;

        [SerializeField]
        protected float weight;//影响击退重量

        [Header("状态")]
        [SerializeField]
        protected float inactionTime;//剩余不可行动时间
        [SerializeField]
        protected float hoverTime;//是否启用硬直悬浮?
        [SerializeField]
        protected SpaceState spaceState;//空间状态
        [SerializeField]
        protected bool rightSpace;//右前进接触空间状态
        [SerializeField]
        protected bool leftSpace;//左前进接触空间状态


        /// <summary>
        /// 是否启用硬直悬浮?
        /// </summary>
        public virtual float HoverTime { get => hoverTime; set {hoverTime = MathF.Max(value, hoverTime); if (hoverTime > 0) spaceState = SpaceState.Air; } }

        /// <summary>
        /// 硬直时间
        /// </summary>
        public virtual float InactionTime { get => inactionTime; set { inactionTime = MathF.Max(value, inactionTime); } }
        /// <summary>
        /// 空间状态
        /// </summary>
        public virtual SpaceState SpaceState { get => spaceState; set => spaceState = value; }
        /// <summary>
        /// 右前进接触空间状态
        /// </summary>
        public virtual bool RightSpace { get => rightSpace; set => rightSpace = value; }
        /// <summary>
        /// 左前进接触空间状态
        /// </summary>
        public virtual bool LeftSpace { get => leftSpace; set => leftSpace = value; }


        public virtual float Health { get => health; set => health = value; }
        public virtual float HealthMax { get => healthMax; set => healthMax = value; }
        public virtual float Weight { get => weight; set => weight = value; }

        public override void Initialize()
        {
        }

        /// <summary>
        /// 判断当前是否可行动
        /// </summary>
        /// <returns></returns>
        public bool IsActionable()
        {
            return inactionTime <= 0;
        }

        protected virtual void Update()
        {
            if (inactionTime > 0)
            {
                inactionTime -= Time.deltaTime;
            }
            if(hoverTime>0)
            {
                hoverTime-= Time.deltaTime;
                if (owner.Rigidbody != null)
                    owner.Rigidbody.AddForce(Vector2.up * owner.Rigidbody.gravityScale);
            }
        }
    }
}