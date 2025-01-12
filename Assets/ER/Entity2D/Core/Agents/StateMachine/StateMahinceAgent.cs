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
    public class StateMahinceAgent : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        [DisplayLabel("当前状态")]
        private StateCell m_current;

        [SerializeField]
        [DisplayLabel("初始化器")]
        private StateMahcineInitializer initializer;

        private EntityAgent m_entity;
        protected Dictionary<string, bool> stateParams = new Dictionary<string, bool>();
        protected Dictionary<string, StateLayer> actionLayers = new Dictionary<string, StateLayer>();
        protected Dictionary<string, StateCell> states = new Dictionary<string, StateCell>();

        /// <summary>
        /// 改变状态时触发的事件
        /// </summary>
        public event Action<string> onChangedState;

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

        public Animator animator
        { get { return m_animator; } }

        /// <summary>
        /// 获取当前状态
        /// </summary>
        public StateCell current
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

        //检查当前状态的过渡, 参数填的是触发器参数, 如果无触发器则为 string.empty
        private void CheckTransition(string triggerTag = "")
        {
            //更新主状态机状态
            if (current != null)
            {
                var aim = current.CheckTransition(triggerTag);
                var aimState = GetStateCell(aim);
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
        private void Awake()
        {
            initializer = GetComponent<StateMahcineInitializer>();
        }
        private void Start()
        {
            if (initializer != null)
            {
                initializer.InitStateMahcine(this);
            }
            else
            {
                current = null;
            }
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

        #region 公开方法

        /// <summary>
        /// 设置状态机控制参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="status"></param>
        public void SetParam(string paramName, bool status)
        {
            //Debug.Log($"设置状态: {stateParamName} : {status}");
            if (stateParams.TryGetValue(paramName, out var old))
            {
                if (old != status)
                {
                    stateParams[paramName] = status;
                    CheckTransition();
                }
            }
            else
            {
                stateParams[paramName] = status;
                CheckTransition();
            }
        }

        /// <summary>
        /// 获取指定状态
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public StateCell GetStateCell(string stateName)
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
        public StateLayer GetStateLayer(string layerName)
        {
            if (actionLayers.TryGetValue(layerName, out var state))
                return state;
            return null;
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
            var state = GetStateCell(stateName);
            if (state != null)
            {
                state.NextStep();
                CheckTransition();
                CheckActionLayerTransition();
            }
        }

        /// <summary>
        /// 获取指定参数的状态
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
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

        #endregion 公开方法

        #region 派生可用方法

        /// <summary>
        /// 清空状态机设置
        /// </summary>
        protected void Clear()
        {
            m_current = null;
            stateParams.Clear();
            actionLayers.Clear();
            states.Clear();
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
        /// 创建一个空的状态
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        protected StateCell CreateState(string stateName)
        {
            StateCell state = new StateCell() { stateName = stateName };
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
        protected StateLayer CreateActionLayer(string layerName)
        {
            StateLayer state = new StateLayer() { stateName = layerName };
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
        protected string TransitionIF(string defaultState, params string[] stateNames)
        {
            for (int i = 0; i < stateNames.Length; i++)
            {
                if (GetParamState(stateNames[i]))
                    return stateNames[i];
            }
            return defaultState;
        }

        #endregion 派生可用方法

        public abstract class StateMahcineInitializer : MonoBehaviour
        {
            private ActionAgent m_actAgent;
            public StateMahinceAgent stateMachine { get; set; }

            public ActionAgent actAgent
            {
                get => m_actAgent; private set => m_actAgent = value;
            }

            /// <summary>
            /// 子类实现, 状态机初始化, 并返回初始状态
            /// </summary>
            /// <returns></returns>
            protected abstract StateCell BuildStateMachine();

            public void InitStateMahcine()
            {
                if (stateMachine != null)
                {
                    stateMachine.Clear();
                    actAgent = stateMachine.entity.actAgent;
                    stateMachine.current = BuildStateMachine();
                }
            }

            public void InitStateMahcine(StateMahinceAgent stateMachine)
            {
                this.stateMachine = stateMachine;
                InitStateMahcine();
            }

            protected void SetParam(string stateParamName, bool status)
            {
                stateMachine.SetParam(stateParamName, status);
            }

            protected StateCell GetState(string stateName)
            {
                return stateMachine.GetStateCell(stateName);
            }

            protected StateLayer GetLayer(string layerName)
            {
                return stateMachine.GetStateLayer(layerName);
            }

            public void TriggerTransition(string triggerTag)
            {
                stateMachine.TriggerTransition(triggerTag);
            }

            /// <summary>
            /// 设置状态机状态(无过渡检测), 用作动作过渡时强制状态复位
            /// </summary>
            /// <param name="stateParamName"></param>
            /// <param name="status"></param>
            protected void SetParamNoCheck(string stateParamName, bool status)
            {
                stateMachine.SetParamNoCheck(stateParamName, status);
            }

            /// <summary>
            /// 创建一个空的状态
            /// </summary>
            /// <param name="stateName"></param>
            /// <returns></returns>
            protected StateCell CreateState(string stateName)
            {
                return stateMachine.CreateState(stateName);
            }

            /// <summary>
            /// 设置动画参数
            /// </summary>
            /// <param name="triggerName"></param>
            protected void SetAnimatorTrigger(string triggerName)
            {
                stateMachine.SetAnimatorTrigger(triggerName);
            }

            /// <summary>
            /// 创建一个空的状态层
            /// </summary>
            /// <param name="layerName"></param>
            /// <returns></returns>
            protected StateLayer CreateActionLayer(string layerName)
            {
                return stateMachine.CreateActionLayer(layerName);
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
            protected string TransitionIF(string defaultState, params string[] stateNames)
            {
                return stateMachine.TransitionIF(defaultState, stateNames);
            }

            /// <summary>
            /// 清空状态机设置
            /// </summary>
            protected void Clear()
            {
                stateMachine.Clear();
            }
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