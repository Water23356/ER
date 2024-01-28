
using System;
using System.Collections.Generic;

namespace ER.TurnBase
{
    /// <summary>
    /// 效果基类
    /// </summary>
    public abstract class Buff
    {
        #region 属性

        /// <summary>
        /// 效果宿主
        /// </summary>
        public BuffManager owner;

        /// <summary>
        /// 效果名称
        /// </summary>
        public string buffName;

        /// <summary>
        /// 效果标签
        /// </summary>
        public List<string> buffTag = new();

        /// <summary>
        /// 名称文本
        /// </summary>
        public string nameText;

        /// <summary>
        /// 描述文本
        /// </summary>
        public string descriptionText;
        /// <summary>
        /// 效果持续回合数
        /// </summary>
        public int rounds;

        /// <summary>
        /// 效果等级
        /// </summary>
        public int level;

        /// <summary>
        /// 最大效果等级
        /// </summary>
        public int levelMax;

        /// <summary>
        /// 结算方式(持续回合数结算方式)
        /// </summary>
        public enum SettleType
        {
            /// <summary>
            /// 不自动结算(存在其他结算方式,例如触发结算等)
            /// </summary>
            None,
            /// <summary>
            /// 自身回合开始时结算
            /// </summary>
            SelfRoundStart,
            /// <summary>
            /// 自身回合结束时结算
            /// </summary>
            SelfRoundEnd,
            /// <summary>
            /// 世界回合开始时结算
            /// </summary>
            WorldRoundStart,
            /// <summary>
            /// 世界回合结束时结算
            /// </summary>
            WorldRoundEnd,
        }
        /// <summary>
        /// 持续回合结算方式
        /// </summary>
        public SettleType settleType;

        /// <summary>
        /// 优先级
        /// </summary>
        public int priority;
        #endregion
        #region 事件

        /// <summary>
        /// 加入此效果触发的事件
        /// </summary>
        public event Action<BuffManager> EnterEvent;

        /// <summary>
        /// 效果生效时触发的事件
        /// </summary>
        public event Action<BuffManager> TakeEffectEvent;

        /// <summary>
        /// 移除此效果触发的事件
        /// </summary>
        public event Action<BuffManager> ExitEvent;

        #endregion 事件


        public void Enter()
        {
            if (EnterEvent != null) EnterEvent(owner);
            EffectOnEnter();
        }

        public void Exit()
        {
            if (ExitEvent != null) ExitEvent(owner);
            EffectOnExit();
        }
        /// <summary>
        /// 当该效果被重复添加时触发的函数
        /// </summary>
        /// <param name="add">后面添加的 与自身同类型的效果对象</param>
        public virtual void Repeat(Buff add)
        {

        }

        /// <summary>
        /// 当加入此效果时触发的函数(效果初始化函数)
        /// </summary>
        /// <param name="owner"></param>
        public abstract void EffectOnEnter();


        /// <summary>
        /// 当移除此效果时触发的函数(效果析构函数)
        /// </summary>
        /// <param name="owner"></param>
        public abstract void EffectOnExit();
        /// <summary>
        /// 当自身回合开始时触发的函数
        /// </summary>
        public abstract void EffectOnSelfRoundStart();
        /// <summary>
        /// 当自身回合结束时触发的函数
        /// </summary>
        public abstract void EffectOnSelfRoundEnd();
        /// <summary>
        /// 当世界回合开始时触发的函数
        /// </summary>
        public abstract void EffectOnWorldRoundStart();
        /// <summary>
        /// 当世界回合结束时触发的函数
        /// </summary>
        public abstract void EffectOnWorldRoundEnd();

    }
}