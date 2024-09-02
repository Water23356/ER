using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ER.TurnBox
{
    public class BuffGroup : IEnumerable<Buff>
    {
        private Role m_owner;
        private IBuffGroupVisual m_visual;
        public Dictionary<string, Buff> m_buffs = new Dictionary<string, Buff>();
        public Role Owner { get => m_owner; protected set => m_owner = value; }
        public IBuffGroupVisual Visual { get; set; }

        /// <summary>
        /// buff数量
        /// </summary>
        public int Count
        { get { return m_buffs.Count; } }

        public BuffGroup(Role role)
        {
            Owner = role;
        }

        /// <summary>
        /// 强制同步显示
        /// </summary>
        public void UpdateVisual()
        {
            if (Visual != null)
                Visual.UpdateVisual(this);
        }

        public void Clear()
        {
            m_buffs.Clear();
            UpdateVisual();
        }

        /// <summary>
        /// 添加新的Buff
        /// </summary>
        /// <param name="buff"></param>
        public void AddBuff(Buff buff)
        {
            if (buff == null)
            {
                Debug.Log("不可添加空Buff");
                return;
            }
            Debug.Log($"添加Buff: {buff.RegistryName}");

            //foreach(var bf in m_buffs.Values)
            //{
            //    Debug.Log($"已有buff: {bf.RegistryName}");
            //}

            if (m_buffs.TryGetValue(buff.RegistryName, out var old))
            {
                //Debug.Log("发生覆盖");
                old.Overlay(buff);
                old.UpdateVisual();
                return;
            }

            m_buffs[buff.RegistryName] = buff;
            buff.Target = Owner;

            //Debug.Log($"buff显示组: {Visual != null} buff是否可视: {buff.IsVisible}");
            buff.Visual = Visual?.PlayAddBuff(buff);
            buff.ApplyEffect();
        }

        /// <summary>
        /// 是否包含指定Buff对象(判断对象而非注册名)
        /// </summary>
        /// <param name="buff"></param>
        public bool HasBuff(Buff buff)
        {
            return m_buffs.ContainsKey(buff.RegistryName);
        }

        /// <summary>
        /// 移除Buff
        /// </summary>
        /// <param name="buff"></param>
        public void RemoveBuff(Buff buff, bool trigger = true)
        {
            if (m_buffs.TryGetValue(buff.RegistryName, out var old))
            {
                Debug.Log($"移除buff: {old.RegistryName}");
                m_buffs.Remove(old.RegistryName);
                if (trigger) old.RemoveEffect();
                if (Visual != null)
                {
                    Visual.PlayRemoveBuff(old);
                }
            }
        }

        /// <summary>
        /// 移除Buff
        /// </summary>
        /// <param name="buff"></param>
        public void RemoveBuff(string registryName, bool trigger = true)
        {
            if (m_buffs.TryGetValue(registryName, out var buff))
            {
                Debug.Log($"移除buff: {buff.RegistryName}");
                m_buffs.Remove(registryName);
                if (trigger) buff.RemoveEffect();
                if (Visual != null)
                {
                    Visual.PlayRemoveBuff(buff);
                }
            }
        }

        /// <summary>
        /// 移除第一个buff
        /// </summary>
        public void RemoveBuffFirst(bool trigger = true)
        {
            string needRemove = string.Empty;
            foreach (var buff in m_buffs.Keys)
            {
                needRemove = buff;
                break;
            }
            RemoveBuff(needRemove, trigger);
        }

        /// <summary>
        /// 是否包含指定Buff
        /// </summary>
        /// <param name="buff"></param>
        public bool HasBuff(string registryName)
        {
            return m_buffs.ContainsKey(registryName);
        }

        /// <summary>
        /// 获取指定Buff
        /// </summary>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public Buff GetBuff(string registryName)
        {
            if (m_buffs.TryGetValue(registryName, out var buff))
                return buff;
            return null;
        }

        /// <summary>
        /// 获取指定buff的level, 如果不存在则返回默认值
        /// </summary>
        /// <param name="registryName"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public int GetBuffLevel(string registryName, int defualtValue = 0)
        {
            if (m_buffs.TryGetValue(registryName, out var buff))
            {
                return buff.Level;
            }
            return defualtValue;
        }

        /// <summary>
        /// 获取指定buff的duration, 如果不存在则返回默认值
        /// </summary>
        /// <param name="registryName"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public int GetBuffDuration(string registryName, int defualtValue = 0)
        {
            if (m_buffs.TryGetValue(registryName, out var buff))
            {
                return buff.Duration;
            }
            return defualtValue;
        }

        /// <summary>
        /// 在指定时机调用该函数, 以触发所有buff在该时机下的效果
        /// </summary>
        /// <param name="triggerTime"></param>
        public void Apply(TriggerTime triggerTime, object infos = null)
        {
            var buffs = m_buffs.Values.ToArray();
            foreach (var buff in buffs)
                buff.Trigger(triggerTime, infos);
        }

        /// <summary>
        /// 修改指定buff对象的属性
        /// </summary>
        /// <param name="registryName"></param>
        /// <param name="propName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public bool ModifyProperty(string registryName, string propName, object newValue)
        {
            if (m_buffs.TryGetValue(registryName, out var buff))
            {
                switch (propName)
                {
                    case "level":
                        buff.Level = (int)newValue;
                        break;

                    case "duration":
                        buff.Duration = (int)newValue;
                        break;

                    default:
                        buff[propName] = newValue;
                        break;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定buff对象的属性, 如果目标不存在则返回默认值
        /// </summary>
        /// <param name="registryName"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public object GetProperty(string registryName, string propName, object defaultValue = null)
        {
            if (m_buffs.TryGetValue(registryName, out var buff))
            {
                switch (propName)
                {
                    case "level":
                        return buff.Level;

                    case "duration":
                        return buff.Duration;

                    default:
                        return buff[propName];
                }
            }
            return defaultValue;
        }

        public IEnumerator<Buff> GetEnumerator()
        {
            foreach (var buff in m_buffs.Values)
            {
                yield return buff;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var buff in m_buffs.Values)
            {
                yield return buff;
            }
        }
    }
}