// Ignore Spelling: Tiker Unregister Tikers

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public sealed class TimerManager : MonoSingletonAutoCreate<TimerManager>
    {
        #region 枚举定义

        /// <summary>
        /// 状态更新模式
        /// </summary>
        public enum UpdateMode
        {
            /// <summary>
            /// 不更新
            /// </summary>
            None,

            /// <summary>
            /// 受到时间系数影响
            /// </summary>
            ScaledTime,

            /// <summary>
            /// 不受到时间系数影响
            /// </summary>
            UnscaledTime,
        }

        /// <summary>
        /// 销毁模式
        /// </summary>
        public enum DestroyMode
        {
            /// <summary>
            /// 不自动销毁
            /// </summary>
            None,

            /// <summary>
            /// 运行一次后自动销毁
            /// </summary>
            Single,

            /// <summary>
            /// 不销毁且循环运行
            /// </summary>
            Loop
        }

        #endregion 枚举定义

        /// <summary>
        /// 在 delay 秒后触发 action 委托, 循环 times 次
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static BaseTimer Invoke(Action action, float delay, int times = 1)
        {
            return Instance.AddAction(action, delay, times);
        }

        /// <summary>
        /// 在 delay 秒后触发 action 委托, 循环 times 次(不受timeScale影响)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static BaseTimer UnscaledInvoke(Action action, float delay, int times = 1)
        {
            return Instance.AddUnscaledAction(action, delay, times);
        }

        public int poolCount = 20;
        private List<string> timerNeedRemoveds = new List<string>(); //等待移除的计时器
        private LinkedList<BaseTimer> timers = new LinkedList<BaseTimer>(); //所有计时器列表
        private Queue<BaseTimer> pool = new Queue<BaseTimer>(20);

        private List<string> tickerNeedRemovedss = new List<string>(); //等待移除的计时器
        private LinkedList<TickTimer> tickers = new LinkedList<TickTimer>(); //所有计时器列表(tick)
        private Queue<TickTimer> pool_ticker = new Queue<TickTimer>(20);

        private float realTimeRecord;
        private float realDeltaTime;
        public static float RealDeltaTime
        {
            get
            {
                return Instance.realDeltaTime;
            }
        }

        /// <summary>
        /// 注册一个计时器
        /// </summary>
        /// <param name="timer"></param>
        public void RegisterTimer(BaseTimer timer)
        {
            timer.timer = timer.limitTime;
            timers.AddLast(timer);
            //Debug.Log($"添加计时器: {timer.tag} : {timer.limitTime}");
        }

        /// <summary>
        /// 注销指定标签的所有计时器
        /// </summary>
        /// <param name="tag">计时器标签</param>
        public void UnregisterTimer(string tag)
        {
            timerNeedRemoveds.Add(tag);
        }

        /// <summary>
        /// 注册一个计时器(tick)
        /// </summary>
        /// <param name="timer"></param>
        public void RegisterTicker(TickTimer ticker)
        {
            ticker.ticks = ticker.limitTick;
            tickers.AddLast(ticker);
            //Debug.Log($"添加计时器: {timer.tag} : {timer.limitTime}");
        }

        /// <summary>
        /// 注销指定标签的所有计时器(tick)
        /// </summary>
        /// <param name="tag">计时器标签</param>

        public void UnregisterTicker(string tag)
        {
            tickerNeedRemovedss.Add(tag);
        }

        /// <summary>
        /// 取得一个已注册的计时器
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public BaseTimer GetTimer(string tag)
        {
            foreach (var timer in timers)
            {
                if (timer.tag == tag)
                {
                    return timer;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取所有该标签的计时器
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public BaseTimer[] GetTimers(string tag)
        {
            List<BaseTimer> ot = new List<BaseTimer>();
            foreach (var timer in timers)
            {
                if (timer.tag == tag)
                {
                    ot.Add(timer);
                }
            }
            return ot.ToArray();
        }

        /// <summary>
        /// 取得一个已注册的计时器
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public TickTimer GetTicker(string tag)
        {
            foreach (var timer in tickers)
            {
                if (timer.tag == tag)
                {
                    return timer;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取所有该标签的计时器
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public TickTimer[] GetTickers(string tag)
        {
            List<TickTimer> ot = new List<TickTimer>();
            foreach (var timer in tickers)
            {
                if (timer.tag == tag)
                {
                    ot.Add(timer);
                }
            }
            return ot.ToArray();
        }

        public TickTimer AddActionWithTick(Action action, int tickCount, int times)
        {
            if (tickCount < 0) return null;

            TickTimer ticker = NewTicker();
            ticker.callback = action;
            ticker.destroyMode = DestroyMode.Loop;
            ticker.limitCount = times;
            ticker.counter = 0;
            ticker.limitTick = tickCount;
            ticker.ticks = 0;
            ticker.updateMode = UpdateMode.ScaledTime;

            RegisterTicker(ticker);
            return ticker;
        }

        /// <summary>
        /// 在 delay 秒后触发 action 委托, 循环 times 次
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <param name="times"></param>
        /// <returns></returns>

        public BaseTimer AddAction(Action action, float delay, int times = 1)
        {
            if (times <= 0) return null;

            BaseTimer timer = NewTimer();
            timer.callback = action;
            timer.destroyMode = DestroyMode.Loop;
            timer.limitCount = times;
            timer.counter = 0;
            timer.limitTime = delay;
            timer.timer = delay;
            timer.updateMode = UpdateMode.ScaledTime;

            RegisterTimer(timer);
            return timer;
        }

        /// <summary>
        /// 在 delay 秒后触发 action 委托, 循环 times 次(不受timeScale影响)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <param name="times"></param>
        /// <returns></returns>

        public BaseTimer AddUnscaledAction(Action action, float delay, int times = 1)
        {
            if (times <= 0)
                return null;
            BaseTimer timer = NewTimer();
            timer.callback = action;
            timer.destroyMode = DestroyMode.Loop;
            timer.limitCount = times;
            timer.counter = 0;
            timer.limitTime = delay;
            timer.timer = delay;
            timer.updateMode = UpdateMode.UnscaledTime;

            RegisterTimer(timer);
            return timer;
        }

        /// <summary>
        /// 获取一个新的计时器,如果对象池为空则创建新的计时器
        /// </summary>
        /// <returns></returns>
        private BaseTimer NewTimer()
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            BaseTimer timer = new BaseTimer();
            timer.tag = timer.GetHashCode() + UnityEngine.Random.value.ToString();
            return timer;
        }

        /// <summary>
        /// 获取一个新的计时器,如果对象池为空则创建新的计时器
        /// </summary>
        /// <returns></returns>
        private TickTimer NewTicker()
        {
            if (pool.Count > 0)
            {
                return pool_ticker.Dequeue();
            }
            TickTimer timer = new TickTimer();
            timer.tag = timer.GetHashCode() + UnityEngine.Random.value.ToString();
            return timer;
        }

        private void UpdateTimer()
        {
            List<BaseTimer> removes = new List<BaseTimer>(timerNeedRemoveds.Count);
            foreach (var timer in timers)
            {
                if (timerNeedRemoveds.Contains(timer.tag))
                {
                    removes.Add(timer);
                    continue;
                }
                if (!timer.IsOver())
                {
                    switch (timer.updateMode)
                    {
                        case UpdateMode.None:
                            break;

                        case UpdateMode.ScaledTime:
                            timer.timer -= Time.deltaTime;
                            break;

                        case UpdateMode.UnscaledTime:
                            timer.timer -= Time.unscaledDeltaTime;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    timer.callback?.Invoke();
                    switch (timer.destroyMode)
                    {
                        case DestroyMode.None:
                            break;

                        case DestroyMode.Single:
                            removes.Add(timer);
                            break;

                        case DestroyMode.Loop:
                            timer.counter++;
                            if (timer.limitCount > 0 && timer.counter >= timer.limitCount)
                            {
                                removes.Add(timer);
                            }
                            timer.timer = timer.limitTime;
                            break;
                    }
                }
            }
            timerNeedRemoveds.Clear();
            for (int i = 0; i < removes.Count; i++)
            {
                timers.Remove(removes[i]);
                if (pool.Count < poolCount)
                {
                    pool.Enqueue(removes[i]);
                }
            }
        }

        private void UpdateTicker()
        {
            List<TickTimer> removes = new List<TickTimer>(tickerNeedRemovedss.Count);
            foreach (var tickTimer in tickers)
            {
                if (tickerNeedRemovedss.Contains(tickTimer.tag))
                {
                    removes.Add(tickTimer);
                    continue;
                }
                if (!tickTimer.IsOver())
                {
                    switch (tickTimer.updateMode)
                    {
                        case UpdateMode.None:
                            break;

                        default:
                            tickTimer.ticks--;
                            break;
                    }
                }
                else
                {
                    tickTimer.callback?.Invoke();
                    switch (tickTimer.destroyMode)
                    {
                        case DestroyMode.None:
                            break;

                        case DestroyMode.Single:
                            removes.Add(tickTimer);
                            break;

                        case DestroyMode.Loop:
                            tickTimer.counter++;
                            if (tickTimer.limitCount > 0 && tickTimer.counter >= tickTimer.limitCount)
                            {
                                removes.Add(tickTimer);
                            }
                            tickTimer.ticks = tickTimer.limitTick;
                            break;
                    }
                }
            }
            tickerNeedRemovedss.Clear();
            for (int i = 0; i < removes.Count; i++)
            {
                tickers.Remove(removes[i]);
                if (pool.Count < poolCount)
                {
                    pool_ticker.Enqueue(removes[i]);
                }
            }
        }

        private void Update()
        {
            realDeltaTime = Time.realtimeSinceStartup - realTimeRecord;
            realTimeRecord = Time.realtimeSinceStartup;

            UpdateTimer();
        }

        private void FixedUpdate()
        {
            UpdateTicker();
        }
    }
}