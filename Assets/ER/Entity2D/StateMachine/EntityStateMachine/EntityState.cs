using ER.StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    public class EntityState : StateCell
    {
        private DynamicEntity owner;


        private List<StateTransformInfo> transforms;//过渡委托

        public EntityState AddStateTransforms(params StateTransformInfo[] tfs)
        {
            if (tfs.Length > 0)
            {
                foreach (var t in tfs)
                {
                    transforms.Add(t);
                }
                transforms.Sort(new StateTransformInfoComparer());
            }
            return this;
        }
        public void ClearStateTransforms()
        {
            transforms.Clear();
        }

        public override void Update()
        {
            if(transforms!=null)
            {
                for (int i = 0; i < transforms.Count; i++)
                {
                    int index = transforms[i].stateIndex;
                    if (index == this.stateIndex)
                    {
                        //Debug.Log($"跳过检测:{index}");
                        continue;//跳过向自身的条件过渡
                    }

                    if (transforms[i].detect())
                    {
                        if(owner.StateMachine.GetState(index)!=null)//如果目标状态不存在的话, 则不进行状态机的更新
                        {
                            owner.Animator.SetInteger("state", index);
                            owner.StateMachine.ChangeState(index);
                        }

                        if (transforms[i].IsBreak)
                        {
                            return;
                        }
                    }
                }
            }
            base.Update();
        }

        public EntityState(int index, DynamicEntity _owner) : base(index)
        {
            owner = _owner;
            transforms = new List<StateTransformInfo>();
        }
    }

    /// <summary>
    /// 状态转换信息
    /// </summary>
    public struct StateTransformInfo
    {
        /// <summary>
        /// 优先级, 数字越小越优先
        /// </summary>
        public int[] priority;

        /// <summary>
        /// 执行完毕 结果函数 后是否跳出后续的检测?
        /// </summary>
        public bool IsBreak;

        /// <summary>
        /// 检测委托, 如果满足条件则执行结果
        /// </summary>
        public Func<bool> detect;
        /// <summary>
        /// 目标状态索引(关联 动画机 和 状态机)
        /// </summary>
        public int stateIndex;
    }

    public class StateTransformInfoComparer : IComparer<StateTransformInfo>
    {
        public int Compare(StateTransformInfo x, StateTransformInfo y)
        {
            int i = 0;
            while (true)
            {
                int xp = GetP(x, i);
                int yp = GetP(y, i);
                if (xp < yp)
                {
                    return -1;
                }
                else if (xp > yp)
                {
                    return 1;
                }
                else
                {
                    if (xp == -1)
                    {
                        return 0;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        private int GetP(StateTransformInfo s, int index)
        {
            if (s.priority.Length > index)
            {
                return s.priority[index];
            }
            return -1;
        }
    }
}