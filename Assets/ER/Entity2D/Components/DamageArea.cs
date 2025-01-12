using ER.ForEditor;
using UnityEngine;

namespace ER.Entity2D.Components
{
    public class DamageArea : MonoBehaviour, IHitSource
    {
        [SerializeField]
        [DisplayLabel("击中区域")]
        private HitBox hitBox;

        [SerializeField]
        [DisplayLabel("伤害值")]
        private float m_damage;

        public float damage { get => m_damage; set => m_damage = value; }

        private void Start()
        {
            hitBox.onHit = SendHitEvent;
            hitBox.hitTag = HitRecorder.defaultTag;
        }

        public void HandleResponse(HitedResponseInfo info)
        {
            //无处理
#if UNITY_EDITOR
            //Debug.Log($"造成伤害: {info.damage}");
#endif
        }

        public void SendHitEvent(HurtHandler hitedEntity)
        {
            var response = hitedEntity.TakeDamage(new HitInfo
            {
                damage = damage,
                hitingTime = 0,
                tag = HitRecorder.defaultTag
            });
            HandleResponse(response);
        }
    }
}