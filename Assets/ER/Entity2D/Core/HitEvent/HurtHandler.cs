using ER.Entity2D.Agents;
using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 受伤处理器
    /// </summary>
    public class HurtHandler : MonoBehaviour, IHurtHandler
    {
        private EntityAgent m_entity;

        private Dictionary<string, float> hitRecorders = new Dictionary<string, float>();

        private HashSet<HitReplySource> sources = new HashSet<HitReplySource>();
        public EntityAgent entity { get => m_entity; set => m_entity = value; }

        [SerializeField]
        [Tooltip("这些受击箱的事件将会由本对象处理")]
        [DisplayLabel("受击箱")]
        private HurtBox box;

        [DisplayLabel("响应器注册表")]
        [SerializeField]
        private HitReplySource[] required;

        private void Awake()
        {
            foreach (var source in required)
                AddSource(source);
        }

        public void AddSource(HitReplySource source)
        {
            sources.Add(source);
            source.handler = this;
        }

        public void RemoveSource(HitReplySource source)
        {
            sources.Remove(source);
            source.handler = null;
        }

        private void Start()
        {
            box.handler = this;
        }

        public HitedResponseInfo TakeDamage(HitInfo info)
        {
            UpdateRecord(info.tag);

            HitedResponseInfo resp = new HitedResponseInfo()
            {
                handleTag = HitHandleFlag.Geted
            };
            foreach (var source in sources)
            {
                if (source.enabled)
                    source.ResponseHit(info, ref resp);
            }
            return resp;
        }

        /// <summary>
        /// 更新伤害计时器
        /// </summary>
        /// <param name="tag"></param>
        private void UpdateRecord(string tag)
        {
            if (HitRecorder.CD.ContainsKey(tag))
            {
                hitRecorders[tag] = Time.time;
            }
            else
            {
                hitRecorders[HitRecorder.defaultTag] = Time.time;
            }
        }

        /// <summary>
        /// 判断当前是否接受指定 标签的 攻击
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool CanHit(string tag)
        {
            if (hitRecorders.TryGetValue(tag, out var lastTime))
            {
                float cd = HitRecorder.GetHitCD(tag);
                return (Time.time - lastTime) >= cd;
            }
            return true;
        }
    }
}