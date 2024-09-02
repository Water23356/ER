using ER;
using System;
using UnityEngine;

namespace ER.Entity2D
{
    public class VictimRegion : MonoAttribute, IVictim, IRegionOwner
    {
        [SerializeField]
        private GameObject RegionObject;

        private IRegion region;

        private VictimAttribute victimAttribute;

        public event Action<AttackInfo, AttackBackInfo> OnGetAttackEvent;

        public AttackBackInfo GetAttack(AttackInfo attackInfo, AttackEventOccurInfo occurInfo)
        {
            AttackBackInfo attackBackInfo = null;
            if (victimAttribute != null)
            {
                victimAttribute.Health -= attackInfo.damage;
            }
            GetAttackEffect(attackInfo.attackEffect, occurInfo);

            OnGetAttackEvent?.Invoke(attackInfo, attackBackInfo);
            return attackBackInfo;
        }

        private void GetAttackEffect(AttackEffectInfo effectInfo, AttackEventOccurInfo occurInfo)
        {
            if (effectInfo == null) return;
            Rigidbody2D bd = owner.GetComponent<Rigidbody2D>();
            if (bd == null) return;

            victimAttribute.InactionTime = effectInfo.inactionTime;//硬直时间

            switch (effectInfo.effectMode)
            {
                case AttackEffectInfo.EffectMode.Repel:
                    Vector2 dir = Vector2.zero;
                    switch (effectInfo.dirMode)
                    {
                        case AttackEffectInfo.RepelDirMode.AutoSelf:
                            dir = (occurInfo.hit - occurInfo.origin).normalized;
                            break;

                        case AttackEffectInfo.RepelDirMode.AutoOwner:
                            dir = (occurInfo.hit - (Vector2)owner.transform.position).normalized;
                            break;

                        case AttackEffectInfo.RepelDirMode.CustomVector:
                            dir = effectInfo.vector.normalized;
                            break;

                        case AttackEffectInfo.RepelDirMode.CustomAngle:
                            if (owner.Direction == Entity.FaceDir.Right)
                            {
                                dir = new Vector2(1, 0).Rotate(effectInfo.angle);
                            }
                            else
                            {
                                dir = new Vector2(-1, 0).Rotate(-effectInfo.angle);
                            }
                            break;
                    }
                    bd.velocity += dir * effectInfo.force / victimAttribute.Weight;
                    break;

                case AttackEffectInfo.EffectMode.FallRebound:
                    Debug.LogError("该效果未实现");
                    break;

                case AttackEffectInfo.EffectMode.Hover:
                    victimAttribute.HoverTime = effectInfo.inactionTime;
                    break;
            }
        }

        public override void Initialize()
        {
            region = RegionObject.GetComponent<IRegion>();
            region.Owner = this;
            victimAttribute = owner.GetAttribute<VictimAttribute>();
        }
    }
}