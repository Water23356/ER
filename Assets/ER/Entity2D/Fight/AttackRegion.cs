
using UnityEngine;

namespace ER.Entity2D
{
    public sealed class AttackRegion:MonoAttribute
    {
        [SerializeField]
        private GameObject regionObject;

        private IRegion region;
        private AttackInfo attackInfo;
        private AttackBackInfo backInfo;
        private IAttacker attacker;//伤害源

        public AttackInfo AttackInfo { get => attackInfo; }
        public AttackBackInfo BackInfo { get => backInfo; }
        public IAttacker Attacker { get => attacker; set => attacker = value; }


        public override void Initialize()
        {
            region = regionObject.GetComponent<IRegion>();
            region.EnterEvent += CauseAttack;
        }
        private void CauseAttack(IRegion r, Collider2D collider)
        {
            IRegion region = collider.gameObject.GetComponent<IRegion>();
            IRegionOwner ro =  region.Owner;
            while(ro is IRegion)//一层一层直到获取顶层所有者
            {
                ro = region.Owner;
            }
            if (ro == null) return;
            if(ro is IVictim)
            {
                IVictim victim = (IVictim)ro;
                AttackEventOccurInfo occurInfo = new AttackEventOccurInfo
                {
                     origin = r.Position,
                     hit = collider.transform.position
                };
                backInfo = victim.GetAttack(attackInfo,occurInfo);
                attacker.GetAttackBack(backInfo);
            }
        }


        private void OnEnable()
        {
            if(attacker==null)
            {
                Debug.LogWarning("攻击体未设置攻击来源");
                return;
            }
            attackInfo = attacker.GetAttackInfo();
        }
    }
}