namespace ER.Entity2D
{
    /// <summary>
    /// 攻击者, 可向 AttackRegion 传递伤害信息
    /// </summary>
    public interface IAttacker
    {
        /// <summary>
        /// 攻击来源实体
        /// </summary>
        public Entity Owner { get; }
        /// <summary>
        /// 获取伤害信息, 在 AttackRegion 被激活时(OnEnable) 会通过该函数自动同步伤害信息
        /// </summary>
        /// <returns></returns>
        public AttackInfo GetAttackInfo();

        /// <summary>
        /// 接收攻击反馈
        /// </summary>
        public void GetAttackBack(AttackBackInfo attackBackInfo);
    }
}