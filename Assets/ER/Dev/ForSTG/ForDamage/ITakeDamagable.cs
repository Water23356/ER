namespace ER.STG
{
    public interface ITakeDamagable
    {
        public DamageBackInfo TakeDamage(DamageInfo info);
    }

    public struct DamageInfo
    {
        /// <summary>
        /// 伤害值
        /// </summary>
        public float damage;
    }
    public struct DamageBackInfo
    {
        /// <summary>
        /// 耐久消耗
        /// </summary>
        public int durableNeed;
    }
}