using ER.ForEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 打击区域
    /// <code>
    /// - 在 hitbox 和 hurtbox 碰撞时触发 onHit 事件;
    /// - 过滤:
    ///     - 处在受击cd的对象
    ///     - 超过受击次数的对象
    ///     - 超出击打对象个数
    /// - 配合 HitSender 使用: 将 HitSender 的 SendHit 订阅在 HitBox 的 onHit 事件
    /// - 由 HitSender 负责传递击打事件的具体信息, HitBox 仅起到触发和对象过滤的作用
    /// </code>
    /// </summary>
    public class HitBox : MonoBehaviour
    {
        [SerializeField]
        [DisplayLabel("区域")]
        private Area m_area;

        [SerializeField]
        [DisplayLabel("穿透限制")]
        private bool m_pierceLimit = true;

        [SerializeField]
        [Tooltip("最多允许对 x 个不同实体造成伤害")]
        [DisplayLabel("穿透")]
        private int m_pierce = 1;

        [SerializeField]
        [DisplayLabel("多次击打限制")]
        private bool m_multipleHitLimit = true;

        [SerializeField]
        [Tooltip("对同一个实体重复造成 x 次伤害")]
        [DisplayLabel("多次击打")]
        private int m_multipleHit = 1;

        [SerializeField]
        [DisplayLabel("持续检测")]
        private bool m_keepCheck = true;

        [SerializeField]
        [DisplayLabel("击打标签")]
        private string m_hittag;

        /// <summary>
        /// 发生击打事件时触发
        /// </summary>
        public Action<HurtHandler> onHit;

        private Dictionary<HurtHandler, int> m_counter = new();

        /// <summary>
        /// 获取或设置打击区域
        /// </summary>
        public Area area
        {
            get { return m_area; }
            private set { m_area = value; }
        }

        /// <summary>
        /// 获取或设置是否启用穿透限制
        /// </summary>
        public bool pierceLimit
        {
            get { return m_pierceLimit; }
            set { m_pierceLimit = value; }
        }

        /// <summary>
        /// 获取或设置穿透次数，即最多允许对多少个不同实体造成伤害
        /// </summary>
        public int pierce
        {
            get { return m_pierce; }
            set { m_pierce = value; }
        }

        /// <summary>
        /// 获取或设置是否启用多次击打限制
        /// </summary>
        public bool multipleHitLimit
        {
            get { return m_multipleHitLimit; }
            set { m_multipleHitLimit = value; }
        }

        /// <summary>
        /// 获取或设置多次击打次数，即对同一个实体重复造成伤害的次数
        /// </summary>
        public int multipleHit
        {
            get { return m_multipleHit; }
            set { m_multipleHit = value; }
        }

        /// <summary>
        /// 获取或设置是否启用持续检测
        /// </summary>
        public bool keepCheck
        {
            get { return m_keepCheck; }
            set { m_keepCheck = value; }
        }

        /// <summary>
        /// 获取或设置击打标签
        /// </summary>
        public string hitTag
        {
            get { return m_hittag; }
            set { m_hittag = value; }
        }

        /// <summary>
        /// 获取或设置击打计数器，用于记录每个 HurtHandler 的击打次数
        /// </summary>
        private Dictionary<HurtHandler, int> counter
        {
            get { return m_counter; }
            set { m_counter = value; }
        }

        private HashSet<HurtHandler> checkList = new HashSet<HurtHandler>();

        /// <summary>
        /// 重置击打参数
        /// </summary>
        /// <param name="pierce">穿透次数</param>
        /// <param name="multipleHit">多次击打次数</param>
        public void ResetHit(int pierce = 1, int multipleHit = 1)
        {
            this.pierce = pierce;
            this.multipleHit = multipleHit;
        }

        private void OnEnable()
        {
            m_area.onEnter += OnAreaEnter;
            m_area.onExit += OnAreaExit;
        }

        private void OnAreaEnter(GameObject other)
        {
            var box = other.GetComponent<HurtBox>();
            if (box == null) return;
            checkList.Add(box.handler);
            if (box.handler.CanHit(hitTag))
            {
                HitEntity(box.handler);
            }
        }

        private void OnAreaExit(GameObject other)
        {
            var box = other.GetComponent<HurtBox>();
            if (box == null) return;
            checkList.Remove(box.handler);
        }

        private void FixedUpdate()
        {
            if (!keepCheck) return;
            var list = checkList.ToArray();
            foreach (var handler in list)
            {
                if (handler.CanHit(hitTag))
                {
                    HitEntity(handler);
                }
            }
        }

        /// <summary>
        /// 处理击打事件
        /// </summary>
        /// <param name="handler">被击打对象的 HurtHandler</param>
        private void HitEntity(HurtHandler handler)
        {
            if (!counter.ContainsKey(handler))
            {
                if (!pierceLimit || pierce > 0)
                {
                    counter[handler] = multipleHit;
                    if(pierceLimit)
                        pierce--;
                    onHit?.Invoke(handler);
                }
            }
            else if (!multipleHitLimit || counter[handler] > 0)
            {
                if (multipleHitLimit)
                    counter[handler]--;
                onHit?.Invoke(handler);
            }
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// 注销所有已注册的事件
        /// </summary>
        private void UnregisterEvents()
        {
            m_area.onEnter -= OnAreaEnter;
            m_area.onExit -= OnAreaExit;
        }
    }
}