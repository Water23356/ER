using System;

namespace ER
{
    /// <summary>
    /// 计时器
    /// </summary>
    public sealed class BaseTimer
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
        /// 计时限制
        /// </summary>
        public float limitTime;

        /// <summary>
        /// 循环模式下循环次数,-1表示无限循环
        /// </summary>
        public int limitCount;

        /// <summary>
        /// 计数器
        /// </summary>
        public int counter;

        /// <summary>
        /// 当前计时数
        /// </summary>
        public float timer;

        public bool IsOver()
        {
            return timer <= 0;
        }

        /// <summary>
        /// 获取一个短暂的(一次性)计时器
        /// </summary>
        /// <returns></returns>
        public static BaseTimer GetBriefTimer(
            string _tag,
            Action _callback,
            float limit,
            TimerManager.UpdateMode updateMode = TimerManager.UpdateMode.ScaledTime
        )
        {
            return new BaseTimer()
            {
                tag = _tag,
                updateMode = updateMode,
                destroyMode = TimerManager.DestroyMode.Single,
                callback = _callback,
                limitTime = limit,
                timer = 0,
            };
        }

        /// <summary>
        /// 获取一个循环的(重复性)计时器
        /// </summary>
        /// <returns></returns>
        public static BaseTimer GetLoopTimer(
            string _tag,
            Action _callback,
            float limit,
            TimerManager.UpdateMode updateMode = TimerManager.UpdateMode.ScaledTime
        )
        {
            return new BaseTimer()
            {
                tag = _tag,
                updateMode = updateMode,
                destroyMode = TimerManager.DestroyMode.Loop,
                callback = _callback,
                limitTime = limit,
                timer = 0,
            };
        }
    }
}