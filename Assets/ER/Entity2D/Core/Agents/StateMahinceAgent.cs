using ER.ForEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ER.Entity2D.Agents
{
    /// <summary>
    /// 动画-状态机代理器:
    /// 负责状态间的跳转管控
    /// </summary>
    public abstract class StateMahinceAgent : MonoBehaviour
    {
        private EntityAgent m_entity;

        public EntityAgent entity
        {
            get
            {
                if (m_entity == null)
                    m_entity = GetComponent<EntityAgent>();
                return m_entity;
            }
            set
            {
                m_entity = value;
            }
        }

        [SerializeField]
        private Animator m_animator;

        public Animator animator
        { get { return m_animator; } }

        protected Dictionary<string, bool> stateParams = new Dictionary<string, bool>();
        protected Dictionary<string, ActionLayer> actionLayers = new Dictionary<string, ActionLayer>();
        protected Dictionary<string, State> states = new Dictionary<string, State>();

        [DisplayLabel("当前状态")]
        [SerializeField]
        private State m_current;

        /// <summary>
        /// 获取当前状态
        /// </summary>
        public State current
        {
            get => m_current; private set => m_current = value;
        }

#if UNITY_EDITOR

        [Space]
        [SerializeField]
        [DisplayLabel("测试监听是否有效")]
        private bool listenEnable = false;

        [SerializeField]
        [DisplayLabel("状态监听窗口 - 测试用")]
        private List<StateLisenCell> stateListener = new List<StateLisenCell>();

#endif
        /// <summary>
        /// 改变状态时触发的事件
        /// </summary>
        public event Action<string> onChangedState;

        /// <summary>
        /// 设置状态机控制参数
        /// </summary>
        /// <param name="stateParamName"></param>
        /// <param name="status"></param>
        public void SetParam(string stateParamName, bool status)
        {
            //Debug.Log($"设置状态: {stateParamName} : {status}");
            if (stateParams.TryGetValue(stateParamName, out var old))
            {
                if (old != status)
                {
                    stateParams[stateParamName] = status;
                    CheckTransition();
                }
            }
            else
            {
                stateParams[stateParamName] = status;
                CheckTransition();
            }
        }

        /// <summary>
        /// 设置状态机状态(无过渡检测), 用作动作过渡时强制状态复位
        /// </summary>
        /// <param name="stateParamName"></param>
        /// <param name="status"></param>
        protected void SetParamNoCheck(string stateParamName, bool status)
        {
            stateParams[stateParamName] = status;
        }

        /// <summary>
        /// 获取指定状态
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public State GetState(string stateName)
        {
            if (states.TryGetValue(stateName, out var state))
                return state;
            return null;
        }

        /// <summary>
        /// 获取指定状态层
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public ActionLayer GetLayer(string layerName)
        {
            if (actionLayers.TryGetValue(layerName, out var state))
                return state;
            return null;
        }

        //检查当前状态的过渡, 参数填的是触发器参数, 如果无触发器则为 string.empty
        private void CheckTransition(string triggerTag = "")
        {
            //更新主状态机状态
            if (current != null)
            {
                var aim = current.CheckTransition(triggerTag);
                var aimState = GetState(aim);
                if (aimState != null && aimState.CanEntry())
                {
                    var old = current;
                    current = aimState;

                    old?.Exit(current.stateName);
                    current.Entry(old.stateName);

                    //Debug.Log($"改变状态: {aim}");
                    onChangedState?.Invoke(aim);
                }
            }

            //更新动作层状态
            CheckActionLayerTransition();
        }

        //检查所有状态层的过渡
        private void CheckActionLayerTransition()
        {
            foreach (var layer in actionLayers.Values)
            {
                layer.CheckTransition();
            }
        }

        /// <summary>
        /// 触发型过渡, 例如: 攻击, 跳跃, 单次点击的动作, 则需要传入对应的触发标签;
        /// 如果成功过渡到目标状态, 应当在 stateParams 中设置对应参数状态为 true, 离开时重设为 null
        /// </summary>
        /// <param name="triggerTag"></param>
        public void TriggerTransition(string triggerTag)
        {
            //Debug.Log($"触发参数: {triggerTag}");
            CheckTransition(triggerTag);
        }

        /// <summary>
        /// 使指定状态阶段+1(一般由动画机调用)
        /// </summary>
        /// <param name="stateName"></param>
        public void NextStep(string stateName)
        {
            var state = GetState(stateName);
            if (state != null)
            {
                state.NextStep();
                CheckTransition();
                CheckActionLayerTransition();
            }
        }

        private void Start()
        {
            current = InitStateMahcine();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (listenEnable)
            {
                foreach (var cell in stateListener)
                {
                    if (stateParams.TryGetValue(cell.stateName, out var state))
                        cell.state = state;
                }
            }
#endif
        }

        /// <summary>
        /// 子类实现, 状态机初始化, 并返回初始状态
        /// </summary>
        /// <returns></returns>
        protected abstract State InitStateMahcine();
        /// <summary>
        /// 创建一个空的状态
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        protected State CreateState(string stateName)
        {
            State state = new State() { stateName = stateName };
            states[stateName] = state;
            return state;
        }
        /// <summary>
        /// 设置动画参数
        /// </summary>
        /// <param name="triggerName"></param>
        protected void SetAnimatorTrigger(string triggerName)
        {
            foreach (var parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(parameter.name);
                }
            }
            animator.SetTrigger(triggerName);
        }
        /// <summary>
        /// 创建一个空的状态层
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        protected ActionLayer CreateActionLayer(string layerName)
        {
            ActionLayer state = new ActionLayer() { stateName = layerName };
            actionLayers[layerName] = state;
            return state;
        }
        /// <summary>
        /// 依次遍历 stateNames; 找到合适的状态名并返回
        /// <code>
        /// 如果 stateNames[i] 对应的状态参数 为 true
        /// 则返回 stateName[i]
        /// 若列表中均不满足条件, 则默认返回 defaultState
        /// </code>
        /// </summary>
        /// <param name="stateName"></param>
        protected string TransitionIF(string defaultState ,params string[] stateNames)
        {
            for(int i=0;i<stateNames.Length;i++)
            {
                if (GetParamState(stateNames[i]))
                    return stateNames[i];
            }
            return defaultState;
                
        }

        public bool GetParamState(string paramName)
        {
            if (stateParams.TryGetValue(paramName, out var state))
            {
                return state;
            }
            return false;
        }

        /// <summary>
        /// 取指定参数的值的相与结果
        /// </summary>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public bool GetParamStateAnd(params string[] paramNames)
        {
            foreach (var key in paramNames)
            {
                if (!GetParamState(key)) return false;
            }
            return true;
        }

        /// <summary>
        ///  取指定参数的值的相或结果
        /// </summary>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public bool GetParamStateOr(params string[] paramNames)
        {
            foreach (var key in paramNames)
            {
                if (GetParamState(key)) return true;
            }
            return false;
        }

        /// <summary>
        /// 当当前状态为这些状态之一时返回true
        /// </summary>
        /// <param name="stateNames"></param>
        /// <returns></returns>
        public bool IsCurrent(params string[] stateNames)
        {
            return stateNames.Contains(current.stateName);
        }

    }

    [Serializable]
    public class State
    {
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
        /// 过渡检查函数: 参数: 触发器标签(无触发器则应传入 string.Empty), 返回: 目标状态名称(无过渡, 则应返回 string.Empty)
        /// <code>
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
        /// 当进入该状态时触发的逻辑
        /// <code>
        /// 同时 step 会被设置为 1 ,不会触发 onChanged
        /// </code>
        /// </summary>
        public Action<string> onEntry;

        /// <summary>
        /// 当离开该状态时触发的逻辑
        /// <code>
        /// 同时 step 会被重置为 0 , 不会触发 onChanged
        /// </code>
        /// </summary>
        public Action<string> onExit;

        /// <summary>
        /// 当外界(一般是动画机)修改本状态的 step 值后, 触发的逻辑
        /// </summary>
        public Action<int> onChanged;

        private int m_step;

        /// <summary>
        /// 状态的步骤:
        /// 0: 失活
        /// 1: 激活
        /// 其他: 激活(额外标记)
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

    public class ActionLayer
    {
        public string stateName;
        public Action onEnable;
        public Action onDisable;
        public Func<bool> checkEnable;
        public Func<bool> checkDisable;

        private bool m_state = false;

        public bool state
        {
            get => m_state;
            set { m_state = value; }
        }

        public void CheckTransition()
        {
            if (state)
            {
                if (checkDisable?.Invoke() ?? false)
                {
                    Disable();
                }
            }
            else
            {
                if (checkEnable?.Invoke() ?? false)
                {
                    Enable();
                }
            }
        }

        public void Enable()
        {
            state = true;
            onEnable?.Invoke();
        }

        public void Disable()
        {
            state = false;
            onDisable?.Invoke();
        }
    }

#if UNITY_EDITOR

    //状态监听窗口(测试用)
    [Serializable]
    public class StateLisenCell
    {
        [DisplayLabel("监听标签")]
        public string stateName;

        [DisplayLabel("状态")]
        [ReadOnly]
        public bool state;
    }

#endif
}