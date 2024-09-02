using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 受击者接口
    /// </summary>
    public interface IVictim
    {

        /// <summary>
        /// 接收攻击事件, 同时返回一个 攻击反馈 信息
        /// </summary>
        /// <param name="attackInfo"></param>
        public AttackBackInfo GetAttack(AttackInfo attackInfo, AttackEventOccurInfo occurInfo);

    }
}