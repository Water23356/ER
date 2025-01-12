using ER.Entity2D.Agents;
using UnityEngine;

namespace ER.Entity2D.Components
{
    /// <summary>
    /// 攻击动作样例
    /// </summary>
    public class AAttackSample : ActionBase, IHitSource
    {
        [SerializeField]
        private HitBox m_HitBox;

        private HitSourceGroup m_HitSourceGroup;

        public override void Init()
        {
            m_HitBox.onHit = m_HitSourceGroup.SendHitEvent;//绑定 击打箱 和 击打源
            m_HitSourceGroup.AddSource(this);
        }

        public override void Entry()
        {
            m_HitBox.enabled = false;
        }

        public override void Exit()
        {
            
        }
        public override void ResponseStep(int step)
        {
            switch (step)
            {
                case 2:
                    m_HitBox.enabled = true;
                    break;
                case 3:
                    m_HitBox.enabled = false;
                    break;
            }
        }

        private void Update()
        {
            
        }

        public void SendHitEvent(HurtHandler hitedEntity)
        {
            var response = hitedEntity.TakeDamage(new HitInfo
            {
                damage = 10,
                hitingTime = 0.5f,
                tag = HitRecorder.defaultTag
            });
            HandleResponse(response);
        }

        public void HandleResponse(HitedResponseInfo info)
        {
            
        }
    }
}