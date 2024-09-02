
using  ER.Entity2D.Enum;
using UnityEngine;

namespace  ER.Entity2D
{
    public abstract class EntityAction:MonoBehaviour,IEntityAction
    {
        [Tooltip("动作注册名")]
        [SerializeField]
        protected ActionName registryName;
        protected ActionGroup relyGroup;
        protected DynamicEntity owner;
        protected IEntityAction.ActionState state;


        public ActionName RegistryName => registryName;
        public GameObject RelyObject => gameObject;
        public ActionGroup RelyGroup { get => relyGroup; set => relyGroup = value; }
        public DynamicEntity Owner { get => owner; set => owner = value; }
        public IEntityAction.ActionState State { get => state; set => state = value; }


        public abstract void Handler(BaseActionParams handlerParams);
        public abstract void Init();
    }
}