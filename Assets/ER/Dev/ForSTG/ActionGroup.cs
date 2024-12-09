using System.Collections.Generic;
using UnityEngine;

namespace ER.STG
{
    public class ActionGroup : MonoBehaviour
    {
        [SerializeField]
        private EntityAction[] actionLoads;

        private Dictionary<string, EntityAction> m_actions;

        public Dictionary<string, EntityAction> Actions
        {
            get
            {
                if (m_actions == null)
                    m_actions = new Dictionary<string, EntityAction>();
                return m_actions;
            }
        }

        private void Awake()
        {
            foreach (var act in actionLoads)
            {
                act.Owner = transform;
                Actions[act.ActionName] = act;
            }
            actionLoads = null;
        }

        public virtual void Shoot()
        {
            Debug.Log("射击");
            ExecuteAction("shoot");
        }

        public virtual void Bomb()
        {
            ExecuteAction("bomb");
        }

        public virtual void Action(int index)
        {
            ExecuteAction("action_" + index);
        }

        protected void ExecuteAction(string key)
        {
            if (Actions.TryGetValue(key, out var act))
            {
                act?.Execute();
            }
        }
    }
}