using ER.Entity2D.Agents;
using System;
using UnityEngine;

namespace ER.Entity2D
{
    public abstract class HitSource : MonoBehaviour, IHitSource
    {
        public event Action<HitedResponseInfo> onGetResponse;

        /// <summary>
        /// 响应伤害事件(发送阶段)
        /// </summary>
        /// <param name="hitedEntity">受击实体</param>
        /// <param name="selfEntity">攻击实体</param>
        public abstract void SendHitEvent(EntityAgent hitedEntity);

        public void HandleResponse(HitedResponseInfo info)
        {
            onGetResponse?.Invoke(info);
        }
    }
}