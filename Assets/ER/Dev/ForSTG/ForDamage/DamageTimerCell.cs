namespace ER.STG
{
    /// <summary>
    /// 伤害计时器
    /// </summary>
    public class DamageTimerCell
    {
        private string damageTimerTag;//伤害计时器标签
        private int ticks;//计时器(时间刻)
        private int immunityTime;//伤害免疫时间

        public string DamageTimerTag { get => damageTimerTag; set => damageTimerTag = value; }
        public int Ticks { get => ticks; set => ticks = value; }
        public int ImmunityTime { get => immunityTime; set => immunityTime = value; }

        /// <summary>
        /// 重置计时器
        /// </summary>
        public void Reset()
        {
            Ticks = -1;
        }
        /// <summary>
        /// 更新计时
        /// </summary>
        /// <param name="amount"></param>
        public void Update(int amount=1)
        {
            if (Ticks < 0) return;
            Ticks += amount;
            if (Ticks >= ImmunityTime)
                Reset();
        }
        /// <summary>
        /// 激活计时器
        /// </summary>
        public void Active()
        {
            Ticks = 0;
        }
        public bool canDamage
        {
            get
            {
                return Ticks < 0;
            }
        }


    }
}