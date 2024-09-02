using System;

namespace ER.TurnBox
{
    public enum TriggerTime
    {
        /// <summary>
        /// 角色回合开始
        /// </summary>
        OnTurnStart,
        /// <summary>
        /// 角色回合结束
        /// </summary>
        OnTurnEnd,
        /// <summary>
        /// 受到伤害时
        /// </summary>
        OnHit,
        /// <summary>
        /// 死亡时
        /// </summary>
        OnDead,
        /// <summary>
        /// 获得格挡时
        /// </summary>
        OnAddBlock,
        /// <summary>
        /// 发生卡牌摧毁时
        /// </summary>
        OnDestroyCard,
        /// <summary>
        /// 损失生命值时
        /// </summary>
        OnLoseHealth,
        /// <summary>
        /// 时机标记
        /// </summary>
        A1 = -1,
        /// <summary>
        /// 时机标记
        /// </summary>
        A2 = -2,
        /// <summary>
        /// 时机标记
        /// </summary>
        A3 = -3,
        /// <summary>
        /// 时机标记
        /// </summary>
        A4 = -4,
        // 其他时机
    }

    [Flags]
    public enum BuffTag
    {
        /// <summary>
        /// 无标签
        /// </summary>
        None = 0,
        /// <summary>
        /// 这是一个状态, 不会因回合衰减
        /// </summary>
        Status,
        /// <summary>
        /// Level 不允许为0, 为0时自动移除
        /// </summary>
        NotZero,
        /*
        Strengthen = 1 << 0,
        Ice = 1 << 1,
        Shield = 1 << 2,
        DoT = 1 << 3,
        Curse = 1 << 4,
        // 其他标签
        使用 | 可以表示标签集, 例如 Ice | Dot 就表示既含有 Ice 也含有 Dot 标签
        */
    }
}