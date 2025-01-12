using ER.ForEditor;
using System;

namespace ER.Entity2D.Agents
{
    [Serializable]
    public class StateCell
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        [DisplayLabel("状态名称")]
        [ReadOnly]
        public string stateName;

        /// <summary>
        /// 用来检查过渡是否有效
        /// <code>
        /// * 一般用作过渡的二级检查, 一级检查一般写在 onCheckTransition,
        /// * 二级检查用来写通用的检查逻辑, 因为所有状态过渡到该状态都会 经历该检查逻辑
        /// * 建议放 和本状态直接相关的 条件检测, 不要涉及其他状态的判断
        /// </code>
        /// </summary>
        public Func<bool> onCheckEntry;

        /// <summary>
        /// 当进入该状态时触发的逻辑
        /// <code>
        /// 同时 step 会被设置为 1 ,不会触发 onChanged
        /// 参数:
        ///     string: 上一个状态名称
        /// </code>
        /// </summary>
        public Action<string> onEntry;

        /// <summary>
        /// 当离开该状态时触发的逻辑
        /// <code>
        /// 同时 step 会被重置为 0 , 不会触发 onChanged
        /// 参数:
        ///     string: 下一个状态名称
        /// </code>
        /// </summary>
        public Action<string> onExit;

        /// <summary>
        /// 过渡检查函数
        /// <code>
        /// 参数:
        ///     string: 触发器标签(无触发器则应传入 string.Empty)
        ///     return string: 目标状态名称(无过渡, 则应返回 string.Empty)
        ///     
        /// state.onCheckTransition = trigger =>{
        ///     //只有满足一定条件时返回 "dead" , 表示需要过渡到 "dead" 状态
        ///     if(GetParamState("dead")
        ///         return "dead"
        ///     //默认返回 string.Empty 表示无过渡
        ///     return string.Empty;
        /// };
        /// </code>
        /// </summary>
        public Func<string, string> onCheckTransition;

        /// <summary>
        /// 当外界(一般是动画机)修改本状态的 step 值后, 触发的逻辑
        /// <code>
        /// 参数:
        ///     int: 修改后的步骤值
        /// </code>
        /// </summary>
        public Action<int> onChanged;

        private int m_step;

        /// <summary>
        /// 状态的步骤:
        /// <code>
        /// 0: 失活
        /// 1: 激活
        /// 其他: 激活(额外标记)
        /// </code>
        /// </summary>
        public int step
        {
            get => m_step;
            private set => m_step = value;
        }

        public bool CanEntry()
        {
            return onCheckEntry?.Invoke() ?? false;
        }

        public string CheckTransition(string triggerTag)
        {
            if (onCheckTransition == null) return string.Empty;
            return onCheckTransition.Invoke(triggerTag);
        }

        public void Entry(string lastStateName)
        {
            m_step = 1;
            onEntry?.Invoke(lastStateName);
        }

        public void Exit(string nextStateName)
        {
            m_step = 0;
            onExit?.Invoke(nextStateName);
        }

        public void NextStep()
        {
            step += 1;
            onChanged?.Invoke(step);
        }
    }
}