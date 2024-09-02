// Ignore Spelling: Actings

using ER.Entity2D.Enum;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 动作组对象, 依附在动作组预制体的总父物体上, 下属各个子动作物体;
    /// 预初始化: 需要将子动作物体拖入加载列表, Awake时, 会自动为动作注入 index, relyGroup, owner,
    /// 仅负责动作的具体逻辑, 由 状态机 进行调度
    /// </summary>
    public sealed class ActionGroup : MonoBehaviour, IAttribute
    {
        private DynamicEntity owner;
        public DynamicEntity Owner { get => owner; set => owner = value; }

        #region 属性

        private Dictionary<ActionName, EntityAction> actions = new Dictionary<ActionName, EntityAction>();

        [SerializeField]
        [Tooltip("子动作栏: 拖入注册")]
        private List<EntityAction> actionsLoadList = new List<EntityAction>();//子动作

        [SerializeField]
        private string groupRegistryName;

        private HashSet<ActionName> actings = new HashSet<ActionName>();//处于活跃状态的 动作
        /// <summary>
        /// 处于活跃(激活)状态的 动作个数
        /// </summary>
        public int ActingCount => actings.Count;
        /// <summary>
        /// 处于活跃状态的动作
        /// </summary>
        public HashSet<ActionName> Actings=>actings;
        public string GroupRegistryName { get => groupRegistryName; }
        Entity IAttribute.Owner { get => owner; set => owner = value as DynamicEntity; }

        #endregion 属性

        /// <summary>
        /// (初始化)重命名物体名称, 用于协调动画机, 因为预制体的名称是不一样的, 如果替换动画模组, 需要改成统一的物体名称, 动画机才能识别到动作组物体
        /// </summary>
        public void Rename()
        {
            gameObject.name = "ActionGourp";
        }

        [ContextMenu("正在活跃的动作: ")]
        private void _PrintCount()
        {
            foreach(var i in actings)
            {
                Debug.Log(i);
            }
        }

        /// <summary>
        /// 进入指定动作的状态(由状态机调用)
        /// </summary>
        /// <param name="actionName"></param>
        public void EnterAction(ActionName actionName)
        {
            EntityAction act = actions[actionName];
            if (!act.RelyObject.activeSelf)
            {
                Debug.Log("激活: " + actionName);
                actings.Add(actionName);
                act.State = IEntityAction.ActionState.INCHOATE;
                act.RelyObject.SetActive(true);
            }
        }

        /// <summary>
        /// 离开指定动作的状态(由动画机调用)
        /// </summary>
        /// <param name="actionName"></param>
        public void ExitAction(ActionName actionName)
        {

            EntityAction act = null;
            if ((int)actionName == -1)//任意则移除正在执行中的某一个动作
            {
                foreach(var c in actings)
                {
                    act = actions[c];
                    if (act.RelyObject.activeSelf)
                    {
                        Debug.Log("休眠: " + c);
                        act.State = IEntityAction.ActionState.SLEEPING;
                        actings.Remove(c);
                        act.RelyObject.SetActive(false);
                    }
                    break;
                }
                return;
            }
            if((int)actionName == -2)
            {
                foreach (var c in actings)
                {
                    act = actions[c];
                    if (act.RelyObject.activeSelf)
                    {
                        Debug.Log("休眠: " + c);
                        act.State = IEntityAction.ActionState.SLEEPING;
                        act.RelyObject.SetActive(false);
                    }
                }
                actings.Clear();
                return;
            }

            act = actions[actionName];
            if (act.RelyObject.activeSelf)
            {
                Debug.Log("休眠: " + actionName);
                act.State = IEntityAction.ActionState.SLEEPING;
                actings.Remove(actionName);
                act.RelyObject.SetActive(false);
            }
        }
        /// <summary>
        /// 获取指定动作的执行状态
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public IEntityAction.ActionState GetActionState(ActionName actionName)
        {
            EntityAction act = actions[actionName];
            return act.State;
        }


        /// <summary>
        /// 动作执行控制器, 由动画机控制的端口
        /// </summary>
        public void ActionHandler(ActionParams paramPack)
        {
            ActionHandler(paramPack.BaseParams);
        }

        /// <summary>
        /// 动作执行控制器, 一些复杂的动作并不是一进入状态就触发; 通过这个函数给 状态机 和 动画机 提供对动作复杂的控制
        /// </summary>
        public void ActionHandler(BaseActionParams paramPack)
        {
            if((int)paramPack.actionName == -1)//-1选项为任意, 此时会从正在执行的动作内选取一个作为作用对象
            {
                foreach(var act in actings)
                {
                    actions[act].Handler(paramPack);
                    break;
                }
                return;
            }
            if ((int)paramPack.actionName == -2)//-1选项为全
            {
                foreach (var act in actings)
                {
                    actions[act].Handler(paramPack);
                }
                return;
            }


            if (actions.TryGetValue(paramPack.actionName, out EntityAction action))
            {
                action.Handler(paramPack);
            }
            throw new System.Exception($"尝试控制 动作组 中不存在的动作:{paramPack.actionName}");
        }

        public void Initialize()
        {
            for (int i = 0; i < actionsLoadList.Count; i++)//将动作单元注册进列表
            {
                EntityAction act = actionsLoadList[i];
                act.RelyGroup = this;
                act.Owner = Owner;
                act.Init();
                actions[act.RegistryName] = act;
            }
        }

        public void Destroy()
        {
            Destroy(this);
        }

    }
}