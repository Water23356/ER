using ER.GUI;

namespace ER.Entity2D.Components
{
    public class DamageRenderer : HitReplySource
    {
        public override void ResponseHit(HitInfo hitInfo, ref HitedResponseInfo originInfo)
        {
            DamageNumberManager.Instance.Render(hitInfo.damage, handler.entity.position);
        }
    }
}