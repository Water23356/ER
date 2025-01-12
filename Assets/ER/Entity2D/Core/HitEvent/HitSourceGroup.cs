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
        private List<IHitSource> handlers = new List<IHitSource>();

        [DisplayLabel("事件源")]
        [SerializeField]
        private HitSource[] requires;

        private bool m_enabled;
        public bool enabled { get => m_enabled; set => m_enabled=value; }

        /// <summary>
        /// 当触发攻击时(成功击中实体)触发的事件;
        /// <code>
        /// 使用该事件接受信息, 不应向受击者发送伤害信息, 如需要发送信息, 应当使用 HitEventHandler 接口
        /// </code>
        /// </summary>
        public event Action<HurtHandler> onHit;

        

        public void Init()
        {
            foreach (var handler in requires)
            {
                AddSource(handler);
            }
        }

        public void AddSource(IHitSource handler)
        {
            handlers.Remove(handler);
            handlers.Add(handler);
        }

        public void RemoveSource(IHitSource handler)
        {
            handlers.Remove(handler);
        }

        public void SendHitEvent(HurtHandler hitedEntity)
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