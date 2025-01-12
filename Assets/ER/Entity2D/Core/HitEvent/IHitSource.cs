using ER.Entity2D.Agents;

namespace ER.Entity2D
{
    public interface IHitSource
    {
        /// <summary>
        /// 发送击打事件
        /// </summary>
        /// <param name="hitedEntity"></param>
        public void SendHitEvent(HurtHandler hitedEntity);
        /// <summary>
        /// 处理事件反馈信息
        /// </summary>
        /// <param name="info"></param>
        public void HandleResponse(HitedResponseInfo info);
        /// <summary>
        /// 源是否有效
        /// </summary>
        public bool enabled { get; set; }
    }

}