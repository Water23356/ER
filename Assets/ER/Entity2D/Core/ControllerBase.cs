using ER.Entity2D.Agents;
using ER.ForEditor;
using UnityEngine;

namespace ER.Entity2D
{
    public abstract class ControllerBase : MonoBehaviour
    {
        public StateMahinceAgent smAgent
        {
            get
            {
                return agent?.entity?.smAgent;
            }
        }

        public ControllerAgent agent { get; set; }
        public EntityAgent entity { get=>agent?.entity; }

        public abstract void Init();

        public virtual void Start() { }
        public virtual void OnDestroy() { }

    }
}