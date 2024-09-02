
using ER.Entity2D.Enum;
using System;
using UnityEngine;

namespace  ER.Entity2D
{
    /// <summary>
    /// 实体动作, 依附在子动作物体上, 负责该动作相关的逻辑, 在初始化时需要指定 owner(自身), RegistryName , 并且确保它们的唯一性
    /// </summary>
    public interface IEntityAction
    {
        public enum ActionState
        {
            /// <summary>
            /// 睡眠中
            /// </summary>
            SLEEPING = -1,
            /// <summary>
            /// 起手
            /// </summary>
            INCHOATE = 0,
            /// <summary>
            /// 执行中
            /// </summary>
            EXECUTING = 1,
            /// <summary>
            /// 收尾
            /// </summary>
            ENDING = 2,
        }
        /// <summary>
        /// 动作状态
        /// </summary>
        public ActionState State { get; }
        public ActionName RegistryName { get; }
        /// <summary>
        /// 所依附的游戏物体
        /// </summary>
        public GameObject RelyObject { get; }
        /// <summary>
        /// 所依附的动作组对象
        /// </summary>
        public ActionGroup RelyGroup { get; set; }
        /// <summary>
        /// 所属Entity对象
        /// </summary>
        public DynamicEntity Owner { get; set; }
        /// <summary>
        /// 动作控制器
        /// </summary>
        /// <param name="params"></param>
        public void Handler(BaseActionParams @params);
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init();
    }
}