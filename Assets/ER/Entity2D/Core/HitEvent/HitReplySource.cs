using UnityEngine;

namespace ER.Entity2D
{
    public abstract class HitReplySource : MonoBehaviour
    {
        public HurtHandler handler { get; set; }

        /// <summary>
        /// 响应伤害事件(反馈阶段)
        /// </summary>
        /// <param name="hitInfo">本次伤害事件中的伤害信息</param>
        /// <param name="originInfo">本次伤害事件中 已有伤害反馈信息集</param>
        /// <returns>本次处理后应当反馈的信息集</returns>
        public abstract void ResponseHit(HitInfo hitInfo, ref HitedResponseInfo originInfo);
    }
}