namespace ER.Entity2D.Components
{
    public class SHealthHitReplyer : HitReplySource
    {
        private SHealth m_health;
        private SHealth health
        {
            get
            {
                if(m_health == null)
                {
                    m_health = handler.entity.staAgent.Get<SHealth>();
                }
                return m_health;
            }
        }
        public override void ResponseHit(HitInfo hitInfo, ref HitedResponseInfo originInfo)
        {
             health.health -= hitInfo.damage;
        }
    }
}