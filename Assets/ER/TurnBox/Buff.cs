using ER.Dynamic;
using System.Collections.Generic;
using UnityEngine;

namespace ER.TurnBox
{
    public abstract class Buff : WithDynamicProperties
    {
        private int m_level;
        private int m_duration;
        private bool m_isDebuff;
        private bool m_isVisible;
        private BuffTag m_tag;
        private IBuffVisual m_visual;
        private RBuff m_configure;

        public Role Target { get; set; }

        public RBuff Configure { get => m_configure; }
        public abstract string RegistryName { get; }
        public int Level
        { get => m_level; set { m_level = value; OnModifyLevel(); } }
        public int Duration
        { get => m_duration; set { m_duration = value; OnModifyDuration(); } }
        public bool IsDebuff { get => m_isDebuff; set => m_isDebuff = value; }
        public bool IsVisible { get => m_isVisible; set => m_isVisible = value; }
        public BuffTag Tag { get => m_tag; protected set => m_tag = value; }
        public IBuffVisual Visual { get => m_visual; set => m_visual = value; }

        public Buff(RBuff origin)
        {
            m_configure = origin;
            IsDebuff = origin.isDebuff;
            IsVisible = origin.isVisible;
            Tag = origin.Tag;
        }

        #region 标签管理

        /// <summary>
        /// 清空所有标签
        /// </summary>
        public void ClearTag()
        {
            Tag = BuffTag.None;
        }

        /// <summary>
        /// 添加标签, 多个标签可以使用 | 分割合并
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(BuffTag tag)
        {
            Tag |= tag;
        }

        /// <summary>
        /// 移除标签, 多个标签可以使用 | 分割合并
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(BuffTag tag)
        {
            Tag &= ~tag;
        }

        /// <summary>
        /// 判断该buff是否拥有指定标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool HasTag(BuffTag tag)
        {
            return (Tag & tag) == tag;
        }

        #endregion 标签管理

        protected virtual void OnModifyLevel()
        {
            if (HasTag(BuffTag.NotZero) && Level == 0)
            {
                Target.buffGroup.RemoveBuff(this);
                UpdateVisual();
            }
        }

        protected virtual void OnModifyDuration()
        {
            if (!HasTag(BuffTag.Status) && Duration <= 0)
            {
                if (Target == null || Target.buffGroup == null) return;
                Target.buffGroup.RemoveBuff(this);
                UpdateVisual();
            }
        }

        /// <summary>
        /// 强制同步显示
        /// </summary>
        public void UpdateVisual()
        {
            if (Visual != null)
                Visual.UpdateVisual(this);
        }

        /// <summary>
        /// 叠加时的处理方法
        /// </summary>
        /// <param name="overBuff"></param>
        public virtual void Overlay(Buff overBuff)
        {
            Debug.Log($"Buff 被覆盖: {RegistryName}");
            Visual?.PlayApply();
        }

        /// <summary>
        /// 当buff被附加时触发(非叠加时)
        /// </summary>
        /// <param name="target"></param>
        public virtual void ApplyEffect()
        {
            Debug.Log($"Buff 被添加: {RegistryName}");
            Visual?.PlayApply();
        }

        /// <summary>
        /// 当buff被移除时触发
        /// </summary>
        /// <param name="target"></param>
        public virtual void RemoveEffect()
        {
            Debug.Log($"Buff 被移除: {RegistryName} Tag: {Tag}");
            Visual?.PlayRemove();
        }

        /// <summary>
        /// buff被特定触发点触发时的处理
        /// </summary>
        /// <param name="target"></param>
        /// <param name="triggerTime"></param>
        public virtual void Trigger(TriggerTime triggerTime, object infos = null)
        {
            Debug.Log($"Buff 被触发: {RegistryName}");
            Visual?.PlayTrigger(triggerTime);
        }

        /// <summary>
        /// 总回合结束时, 减少回合计数
        /// </summary>
        public virtual void TurnEndTick()
        {
            if(!HasTag( BuffTag.Status))
                Duration--;
        }

        public virtual Buff Copy()
        {
            var buff = Configure?.CreateInstance() ?? null;
            buff.Level = Level;
            buff.Duration = Duration;
            var keys = dynamicProperties.GetAllKeys();
            foreach (var key in keys)
            {
                buff.SetDynamicProperty(key, dynamicProperties[key]);
            }
            return buff;
        }
    }
}