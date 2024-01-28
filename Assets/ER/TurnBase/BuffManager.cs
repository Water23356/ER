using ER.Entity2D;
using System.Collections.Generic;
using UnityEngine;

namespace ER.TurnBase
{
    /// <summary>
    /// 回合制 效果管理器
    /// </summary>
    public class BuffManager
    {
        protected TurnPlayer owner;
        /// <summary>
        /// 效果管理器所属的玩家对象, 在被赋值时进行事件初始化, 如果有拓展事件接口, 子类请重写该属性的set器
        /// </summary>
        public virtual TurnPlayer Owner
        {
            get => owner;
            set
            {
                if(owner != null)
                {
                    owner.OnRoundStartEvent -= SelfRoundStartEffect;
                    owner.OnRoundEndEvent -= SelfRoundEndEffect;
                    owner.SandBox.OnRoundStartEvent -= WorldRoundStartEffect;
                    owner.SandBox.OnRoundEndEvent -= WorldRoundEndEffect;
                }
                owner = value;
                if(owner!=null)
                {
                    owner.OnRoundStartEvent += SelfRoundStartEffect;
                    owner.OnRoundEndEvent += SelfRoundEndEffect;
                    owner.SandBox.OnRoundStartEvent += WorldRoundStartEffect;
                    owner.SandBox.OnRoundEndEvent += WorldRoundEndEffect;
                }
                
            }
        }
        protected List<Buff> buffs = new List<Buff>();
        protected Dictionary<string, Buff> buffsDic = new Dictionary<string, Buff>();

        /// <summary>
        /// 添加效果
        /// </summary>
        /// <param name="buff"></param>
        public void Add(Buff buff)
        {
            if (buffsDic.TryGetValue(buff.buffName, out Buff bf))//出现同一个buff重复添加
            {
                bf.Repeat(buff);
            }
            else
            {
                Debug.Log($"添加新的效果:{buff.buffName}");
                buff.owner = this;
                buffsDic[buff.buffName] = buff;
                buffs.Add(buff);
                buff.Enter();
                ArrangeBuffs();
            }
        }
        /// <summary>
        /// 移除效果
        /// </summary>
        /// <param name="name"></param>
        /// <returns>如果移除失败则返回false</returns>
        public bool Remove(string name)
        {
            bool exist = false;
            Buff buff = null;
            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].buffName == name)
                {
                    buff = buffs[i];
                    buffs.RemoveAt(i);
                    exist = true;
                    break;
                }
            }
            if (exist)
            {
                buffsDic.Remove(name);
                buff.Exit();
            }
            return exist;
        }

        /// <summary>
        /// 判断指定效果是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            return buffsDic.ContainsKey(name);
        }

        /// <summary>
        /// 按优先级排列效果
        /// </summary>
        protected void ArrangeBuffs()
        {
            for (int i = 0; i < buffs.Count - 1; i++)
            {
                for (int k = i + 1; k < buffs.Count; k++)
                {
                    if (buffs[k].priority > buffs[i].priority)
                    {
                        var temp = buffs[k];
                        buffs[k] = buffs[i];
                        buffs[i] = temp;
                    }
                }
            }
        }
        /// <summary>
        /// 自身回合开始时触发的效果
        /// </summary>
        public void SelfRoundStartEffect()
        {
            for(int i=0;i<buffs.Count;i++)
            {
                buffs[i].EffectOnSelfRoundStart();
            }
        }
        public void SelfRoundEndEffect()
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                buffs[i].EffectOnSelfRoundEnd();
            }
        }
        public void WorldRoundStartEffect()
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                buffs[i].EffectOnWorldRoundStart();
            }
        }
        public void WorldRoundEndEffect()
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                buffs[i].EffectOnWorldRoundEnd();
            }
        }

        /// <summary>
        /// 析构函数, 如果有拓展事件接口, 子类需要在析构函数中注销事件
        /// </summary>
        ~BuffManager()
        {
            if(owner!=null)
            {
                owner.OnRoundStartEvent -= SelfRoundStartEffect;
                owner.OnRoundEndEvent -= SelfRoundEndEffect;
                owner.SandBox.OnRoundStartEvent -= WorldRoundStartEffect;
                owner.SandBox.OnRoundEndEvent -= WorldRoundEndEffect;
            }
        }
    }
}