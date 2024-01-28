using UnityEditor.UIElements;

namespace ER.TurnBase
{
    /// <summary>
    /// ai 基类
    /// </summary>
    public interface AI
    {
        /// <summary>
        /// 操控的玩家槽位号
        /// </summary>
        public int Index
        {
            get;set;
        }
        /// <summary>
        /// 所属沙盒对象
        /// </summary>
        public TurnSandbox sandbox { get; set; }
        /// <summary>
        /// 回合开始
        /// </summary>
        public void RoundStart();
        /// <summary>
        /// 回合结束
        /// </summary>
        public void RoundEnd();
    }
}