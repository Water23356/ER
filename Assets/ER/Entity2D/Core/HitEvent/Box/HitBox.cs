using ER.ForEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 打击区域
    /// <code>
    /// - 在 hitbox 和 hurtbox 碰撞时触发 onHit 事件;
    /// - 过滤:
    ///     - 处在受击cd的对象
    ///     - 超过受击次数的对象
    ///     - 超出击打对象个数
    /// - 配合 HitSender 使用: 将 HitSender 的 SendHit 订阅在 HitBox 的 onHit 事件
    /// - 由 HitSender 负责传递击打事件的具体信息, HitBox 仅起到触发和对象过滤的作用
    /// </code>
    /// </summary>
    public class HitBox : MonoBehaviour
    {
        [SerializeField]
        [DisplayLabel("区域")]
        private Area area;

        [SerializeField]
        [Tooltip("最多允许对 x 个不同实体造成伤害")]
        [DisplayLabel("穿透")]
        private int pierce = 1;

        [Tooltip("对同一个实体重复造成 x 次伤害")]
        [DisplayLabel("多次击打")]
        private int multipleHit = 1;

        public Action<HurtHandler> onHit;
        private Dictionary<HurtHandler, int> counter = new();

        public void ResetHit(int pierce = 1, int multipleHit = 1)
        {
            this.pierce = pierce;
            this.multipleHit = multipleHit;
        }

        private void Start()
        {
            area.onEnter += (other) =>
            {
                var box = other.GetComponent<HurtBox>();
                if (box == null) { return; }
                if (box.handler.CanHit(tag))
                {
                    HitEntity(box.handler);
                };
            };
        }

        private void HitEntity(HurtHandler handler)
        {
            if (!counter.ContainsKey(handler))
            {
                if (pierce > 0)
                {
                    counter[handler] = multipleHit;
                    pierce--;
                }
            }
            else if (counter[handler] > 0)
            {
                counter[handler]--;
                onHit?.Invoke(handler);
            }
        }
    }
}