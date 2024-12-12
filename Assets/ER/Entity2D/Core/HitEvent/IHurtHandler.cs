namespace ER.Entity2D
{
    public interface IHurtHandler
    {
        public HitedResponseInfo TakeDamage(HitInfo info);
    }

}