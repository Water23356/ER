namespace ER.TurnBase.Template.DoubleCombat
{
    /// <summary>
    /// 双人对战型玩家
    /// </summary>
    public class DCPlayer : TurnPlayer
    {
        protected float health;
        protected float healthMax;
        protected float armor;
        public float HP => health;
        public float HPMax
        {
            get => healthMax;
            set => healthMax = value;
        }

    }
}