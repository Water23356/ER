using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 攻击事件发生点信息, 可根据项目需要进行魔改
    /// </summary>
    public struct AttackEventOccurInfo
    {
        public Vector2 origin;
        public Vector2 hit;
    }


    /// <summary>
    /// 攻击事件信息, 一般包括 伤害源, 伤害值, 伤害类型, 具体可根据项目需求更改
    /// </summary>
    public class AttackInfo
    {
        public IAttacker origin;
        public float damage;
        public AttackEffectInfo attackEffect;
    }

    /// <summary>
    /// 攻击反馈信息, 攻击命中目标后, 会由受击者生成该 信息, 并反馈给伤害源
    /// </summary>
    public class AttackBackInfo
    {
        public IVictim victim;
        public float damage;//实际伤害
    }

    /// <summary>
    /// 攻击导致的额外控制效果
    /// </summary>
    public class AttackEffectInfo
    {
        /// <summary>
        /// 击退方向模式
        /// </summary>
        public enum RepelDirMode
        {
            /// <summary>
            /// 自定义(固定方向)
            /// </summary>
            CustomVector,
            /// <summary>
            /// 自定义(相对角度)
            /// </summary>
            CustomAngle,
            /// <summary>
            /// 自动模式(基于攻击者)
            /// </summary>
            AutoOwner,
            /// <summary>
            /// 自动模式(基于判定区域)
            /// </summary>
            AutoSelf
        }
        /// <summary>
        /// 额外模式
        /// </summary>
        public enum EffectMode
        {
            /// <summary>
            /// 仅击退
            /// </summary>
            Repel,
            /// <summary>
            /// 落地反弹
            /// </summary>
            FallRebound,
            /// <summary>
            /// 滞空
            /// </summary>
            Hover,
        }

        public float force;//击退力度
        public float angle;//击退角度(仅CustomAngle时有效)
        public Vector2 vector;//击退方向(仅CustomVector时有效)
        public RepelDirMode dirMode;//方向模式
        public EffectMode effectMode;//效果模式
        public float inactionTime;//不可行动的时间(硬直时间)

    }
}