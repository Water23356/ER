using ER.Entity2D.Agents;
using ER.ForEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 击打事件发生源(组): 对事件源的成组封装, 元素不可重复
    /// </summary>
    [Serializable]
    public class HitSourceGroup: IHitSource
    {
        private List<HitSource> handlers = new List<HitSource>();

        [DisplayLabel("事件源")]
        [SerializeField]
        private HitSource[] requires;

        /// <summary>
        /// 当触发攻击时(成功击中实体)触发的事件;
        /// <code>
        /// 使用该事件接受信息, 不应向受击者发送伤害信息, 如需要发送信息, 应当使用 HitEventHandler 接口
        /// </code>
        /// </summary>
        public event Action<EntityAgent> onHit;

        public void Init()
        {
            foreach (var handler in requires)
            {
                AddSource(handler);
            }
        }

        public void AddSource(HitSource handler)
        {
            handlers.Remove(handler);
            handlers.Add(handler);
        }

        public void RemoveSource(HitSource handler)
        {
            handlers.Remove(handler);
        }

        public void SendHitEvent(EntityAgent hitedEntity)
        {
            onHit?.Invoke(hitedEntity);
            foreach (var handler in handlers)
            {
                if (handler.enabled)
                    handler.SendHitEvent(hitedEntity);
            }
        }

        public void HandleResponse(HitedResponseInfo info)
        {
            foreach (var handler in handlers)
            {
                if (handler.enabled)
                    handler.HandleResponse(info);
            }
        }
    }
}