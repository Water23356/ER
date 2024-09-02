using ER.Dynamic;

namespace ER.TurnBox
{
    public class Role:WithDynamicProperties
    {
        private BuffGroup m_buffGroup;

        public BuffGroup buffGroup
        {
            get
            {
                if (m_buffGroup == null)
                {
                    m_buffGroup = new BuffGroup(this);
                }
                return m_buffGroup;
            }
            protected set
            {
                m_buffGroup = value;
            }
        }
        public virtual IRoleVisual Visual { get; set; }

        /// <summary>
        /// 进入该角色的回合
        /// </summary>
        public virtual void EnterTurn()
        {
            buffGroup.Apply(TriggerTime.OnTurnStart);
        }

        /// <summary>
        /// 离开该角色的回合
        /// </summary>
        public virtual void ExitTurn()
        {
            buffGroup.Apply(TriggerTime.OnTurnEnd);
        }

        /// <summary>
        /// 强制刷新显示
        /// </summary>
        public virtual void UpdateVisual()
        {
            Visual?.UpdateVisual(this);
        }
    }
}