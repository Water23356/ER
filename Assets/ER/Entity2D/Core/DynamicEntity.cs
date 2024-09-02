using  ER.Entity2D.Enum;
using System;
using UnityEngine;

namespace  ER.Entity2D
{
    /// <summary>
    /// 动态实体; 主要泛指玩家,敌人, 具有复杂行为,动画的实体
    /// 一定拥有 动画机,状态机,动作组, 受击体
    /// </summary>
    public class DynamicEntity:Entity
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private VictimAttribute vattribute;
        [SerializeField]
        private VictimRegion victimRegion;
        [SerializeField]
        private ActionGroup actionGroup;
        [SerializeField]
        private EntityStateMachine stateMachine;

        public Animator Animator { get => animator; set => animator = value; }
        public VictimRegion VictimRegion { get => victimRegion; set { Remove<VictimRegion>(); victimRegion = value; } }

        public VictimAttribute VAttribute{ get => vattribute;set{ Remove<VictimAttribute>(); vattribute= value; } }
        public ActionGroup ActionGroup { get => actionGroup; set { Remove<ActionGroup>(); actionGroup = value; } }
        public EntityStateMachine StateMachine { get => stateMachine; set { Remove<EntityStateMachine>(); stateMachine = value; } }

        protected override void Awake()
        {
            if (vattribute != null)
                Add(vattribute, false);
            if (victimRegion != null)
                Add(victimRegion, false);
            if (actionGroup != null)
                Add(actionGroup, false);
            if (stateMachine != null)
                Add(stateMachine, false);
            base.Awake();
        }

        protected override void Initialized()
        {
            
        }

        public void EnterAction(ActionName action)
        {
            actionGroup?.EnterAction(action);
        }
        public void ExitAction(ActionName action)
        {
            actionGroup?.ExitAction(action);
        }
        public void ActionHandler(ActionParams actionParams)
        {
            actionGroup?.ActionHandler(actionParams);
        }

    }
}