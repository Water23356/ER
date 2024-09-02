
using ER.Entity2D.Enum;
using ER.StateMachine;
using System;
using UnityEngine;

namespace ER.Entity2D
{
    public abstract class EntityStateMachine :MonoBehaviour, IAttribute
    {
        protected DynamicEntity owner;
        protected StateCellMachine stateMachine;
        protected ActionGroup actionGroup;
        protected ActionStateTable monitorStates = new ActionStateTable();

        Entity IAttribute.Owner { get=>owner; set=>owner = value as DynamicEntity; }
        public DynamicEntity Owner { get => owner; set => owner = value; }

        public StateCell Current { get { return stateMachine.currentState; } }

        /// <summary>
        /// 跳转至指定状态
        /// </summary>
        /// <param name="index"></param>
        public void ChangeState(int index)
        {
            stateMachine.ChangeState(index);
        }
        /// <summary>
        /// 获取指定状态
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StateCell GetState(int index)
        {
            return stateMachine.GetState(index);
        }

        /// <summary>
        /// (持续性指令)
        /// 动作激活接口, 外部可接入 输入监听器, 或者AI控制器, 
        /// AI和玩家模块可以通过此函数控制 角色进入到指定 动作状态;
        /// </summary>
        /// <param name="actionName"></param>
        public void SetActionState(ActionName actionName, bool state)
        {
            monitorStates[actionName] = state;
        }

        public virtual void Destroy()
        {
            Destroy(this);
        }

        public void Initialize()
        {
            actionGroup = owner.ActionGroup;
            
            if (actionGroup == null)
            {
                throw new Exception($"[{nameof(owner.name)}]: 缺失对应的动作组 或 动作组与状态机不匹配");
            }
            InitStateMachine();
            enabled = true;
        }
        /// <summary>
        /// 初始化状态机的逻辑, 在这里封装状态机所需要的状态, 并嵌入 Update 跳转逻辑
        /// </summary>
        protected abstract void InitStateMachine();
        /// <summary>
        /// 当前状态是否在默认的状态组内?
        /// </summary>
        /// <returns></returns>
        public abstract bool IsInDefault();


        protected virtual void FixedUpdate()
        {
            stateMachine.Update();
        }
    }
}