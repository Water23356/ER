using System;

namespace ER.TurnBase
{
    /// <summary>
    /// 回合制玩家
    /// </summary>
    public class TurnPlayer
    {
        protected TurnSandbox sandbox;

        /// <summary>
        /// 效果栏
        /// </summary>
        protected BuffManager buffManager;

        public BuffManager BuffManager
        {
            get => buffManager;
            set
            {
                buffManager = value;
                buffManager.Owner = this;
            }
        }

        /// <summary>
        /// 所属沙盒对象
        /// </summary>
        public TurnSandbox SandBox { get => sandbox; set => sandbox = value; }

        /// <summary>
        /// 当自身回合开始时触发的事件
        /// </summary>
        public event Action OnRoundStartEvent;

        /// <summary>
        /// 当自身回合结束时触发的事件
        /// </summary>
        public event Action OnRoundEndEvent;

        /// <summary>
        /// 自身实际经历的回合数
        /// </summary>
        protected int rounds;

        /// <summary>
        /// 自身回合开始
        /// </summary>
        public void RoundStart()
        {
            rounds++;
            OnRoundStartEvent?.Invoke();
        }

        /// <summary>
        /// 自身回合结束
        /// </summary>
        public void RoundEnd()
        {
            OnRoundEndEvent?.Invoke();
            SandBox.NextPlayerRound();
        }
    }
}