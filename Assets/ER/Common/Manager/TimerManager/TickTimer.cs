using System;

namespace ER
{
    /// <summary>
    /// 刻计时器
    /// </summary>
    public sealed class TickTimer
    {
        /// <summary>
        /// 计时器标志
        /// </summary>
        public string tag;

        /// <summary>
        /// 状态更新模式
        /// </summary>
        public TimerManager.UpdateMode updateMode;

        /// <summary>
        /// 销毁模式
        /// </summary>
        public TimerManager.DestroyMode destroyMode;

        /// <summary>
        /// 回调函数
        /// </summary>
        public Action callback;

        /// <summary>
        /// 最大刻
        /// </summary>
        public float limitTick;

        /// <summary>
        /// 循环模式下循环次数,-1表示无限循环
        /// </summary>
        public int limitCount;

        /// <summary>
        /// 计数器
        /// </summary>
        public int counter;

        /// <summary>
        /// 当前刻数
        /// </summary>
        public float ticks;

        public bool IsOver()
        {
            return ticks <= 0;
        }
    }
}