namespace ER.Entity2D
{
    /// <summary>
    /// 伤害输出信息
    /// <code>
    /// DamageInfo 用来携带击打中的伤害信息, 攻击者将会通过该结构体向受击者 传递伤害信息
    /// 请把需要交给 受击者 处理的信息在此处定义
    /// </code>
    /// </summary>
    public struct HitInfo
    {
        /// <summary>
        /// 伤害值
        /// </summary>
        public int damage;

        /// <summary>
        /// 伤害标签
        /// </summary>
        public string tag;

        /// <summary>
        /// 造成受击硬直时间
        /// </summary>
        public float hitTime;
    }

    /// <summary>
    /// 伤害反馈信息
    /// <code>
    /// DamageResponseInfo 用来携带 受击者 在受到伤害向攻击者反馈的信息
    /// 请把 伤害事件处理后 需要反馈给 攻击者 的信息在此处定义
    /// </code>
    /// </summary>
    public struct HitedResponseInfo
    {
        /// <summary>
        /// 实际受到的伤害值
        /// </summary>
        public int damage;

        /// <summary>
        /// 处理标志
        /// </summary>
        public HitHandleFlag handleTag;
        /// <summary>
        /// 是否为空的(无效)反馈信息
        /// </summary>
        public bool IsEmpty
        {
            get=> handleTag == HitHandleFlag.None;
        }
        /// <summary>
        /// 无效反馈信息
        /// </summary>
        public static HitedResponseInfo Empty
        {
            get => new HitedResponseInfo { handleTag = HitHandleFlag.None };
        }

    }

    /// <summary>
    /// 伤害事件处理标志
    /// <code>
    /// DamageHandleTag 用来定义游戏中伤害事件的 处理类型
    /// </code>
    /// </summary>
    public enum HitHandleFlag
    {
        /// <summary>
        /// 非有效反馈
        /// </summary>
        None,

        /// <summary>
        /// 正常承受
        /// </summary>
        Geted,

        /// <summary>
        /// 伤害被闪避
        /// </summary>
        Miss,

        /// <summary>
        /// 伤害被格挡
        /// </summary>
        Block,

        /// <summary>
        /// 伤害被无效化
        /// </summary>
        Invalid
    }
}