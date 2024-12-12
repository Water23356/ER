using ER.ForEditor;
using UnityEngine;

namespace ER.Entity2D.Agents
{
    /// <summary>
    /// 实体代理器;
    /// 是基本实体事件的作用对象:
    /// 1. 伤害传递事件
    /// 2. Buff传递事件
    /// </summary>
    public class EntityAgent : MonoBehaviour
    {
        [SerializeField]
        [DisplayLabel("动作代理器")]
        private ActionAgent m_actAgent;
        [SerializeField]
        [DisplayLabel("状态机代理器")]
        private StateMahinceAgent m_smAgent;
        [SerializeField]
        [DisplayLabel("控制器代理器")]
        private ControllerAgent m_controllerAgent;
        [SerializeField]
        [DisplayLabel("状态代理器")]
        private StateAgent m_staAgent;
        [SerializeField]
        [DisplayLabel("受伤监听器")]
        private HurtHandler m_hurtHandler;
        [SerializeField]
        [DisplayLabel("刚体")]
        private Rigidbody2D m_rigidbody;

        public ActionAgent actAgent { get => m_actAgent; private set => m_actAgent = value; }
        public StateMahinceAgent smAgent { get => m_smAgent; private set => m_smAgent = value; }
        public ControllerAgent controllerAgent { get => m_controllerAgent; private set => m_controllerAgent = value; }
        public StateAgent staAgent { get => m_staAgent; private set => m_staAgent = value; }
        public HurtHandler hurtHandler { get => m_hurtHandler; set => m_hurtHandler = value; }
        public Rigidbody2D rigidbody => m_rigidbody;

        private void Awake()
        {
            if (actAgent == null) actAgent = GetComponent<ActionAgent>();
            if (actAgent != null) actAgent.entity = this;

            if (smAgent == null) smAgent = GetComponent<StateMahinceAgent>();
            if (smAgent != null) smAgent.entity = this;

            if (controllerAgent == null) controllerAgent = GetComponent<ControllerAgent>();
            if (controllerAgent != null) controllerAgent.entity = this;

            if (staAgent == null) staAgent = GetComponent<StateAgent>();
            if (staAgent != null) staAgent.entity = this;

            if(hurtHandler!=null) hurtHandler.entity = this;
        }

        private void Start()
        {
            actAgent?.InitAll();
            controllerAgent?.InitAll();
        }

        public HitedResponseInfo TakeDamage(HitInfo info)
        {
            if (m_hurtHandler == null) return HitedResponseInfo.Empty;
            return m_hurtHandler.TakeDamage(info);
        }

        public bool CanHit(string tag)
        {
            if (m_hurtHandler == null) return false;
            return m_hurtHandler.CanHit(tag);
        }

        /// <summary>
        /// 触发指定动作状态下一个阶段
        /// </summary>
        /// <param name="actionStateName"></param>
        public void ActionNextStep(string actionStateName)
        {
            if(smAgent!=null)
            {
                smAgent.NextStep(actionStateName);
            }
        }
    }

}
