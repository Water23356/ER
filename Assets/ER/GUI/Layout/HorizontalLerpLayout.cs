using ER.ForEditor;
using System.Collections.Generic;
using UnityEngine;

namespace ER.GUI
{
    /// <summary>
    /// 水平布局, 带有插值动画
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class HorizontalLerpLayout : MonoBehaviour, ILerpLayoutGroup
    {
        [SerializeField]
        private LerpLayoutItem[] pre_items;

        private List<ILerpLayoutItem> items;

        private RectTransform m_rectTransform;

        [DisplayLabel("布局区域宽度")]
        [SerializeField]
        private float areaWidth = 200;

        [Tooltip("负数表示无限")]
        [DisplayLabel("最大元素间隔")]
        [SerializeField]
        private float maxInterval = -1;

        [DisplayLabel("布局样式")]
        [SerializeField]
        private LayoutStyle layoutStyle = LayoutStyle.Center;

        [DisplayLabel("反向布局")]
        [SerializeField]
        private bool reverse = false;

        [Header("动画")]
        [SerializeField]
        private float lerpSpeed = 6;

        [SerializeField]
        private float maxSpeed = 500f;

        private RectTransform rectTransform
        {
            get
            {
                if (m_rectTransform == null)
                    m_rectTransform = GetComponent<RectTransform>();
                return m_rectTransform;
            }
        }

        public List<ILerpLayoutItem> Items
        {
            get
            {
                if (items == null)
                    items = new List<ILerpLayoutItem>();
                return items;
            }
        }

        public float LerpSpeed { get => lerpSpeed; set => lerpSpeed = value; }
        public float MaxLerpSpeed { get => maxSpeed; set => maxSpeed = value; }
        public LayoutStyle LayoutMode { get => layoutStyle; set => layoutStyle = value; }
        public bool Reverse { get => reverse; set => reverse = value; }
        public float AreaWidth { get => areaWidth; set => areaWidth = value; }
        public void AddItem(params ILerpLayoutItem[] items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
                item.OwnerGroup = this;
            }
            UpdateLayout();
        }

        public void RemoveItem(params ILerpLayoutItem[] items)
        {
            foreach (var item in items)
            {
                Items.Remove(item);
                item.OwnerGroup = null;
            }
            UpdateLayout();
        }

        public void ClearItem()
        {
            foreach (var item in Items)
            {
                item.OwnerGroup = null;
            }
            Items.Clear();
        }

        private Vector2 GetLayoutPos(int index)
        {
            if (Items.Count == 0)
                return Vector2.zero;
            Vector2 startPos = Vector2.left * (AreaWidth / 2);
            var interval = AreaWidth / Items.Count;
            if (maxInterval > 0 && interval > maxInterval)
            {
                interval = maxInterval;
            }
            switch (LayoutMode)
            {
                case LayoutStyle.Left:

                    return new Vector2(interval * index - AreaWidth / 2, 0);

                case LayoutStyle.Right:
                    return new Vector2(-interval * index + AreaWidth / 2, 0);
            }
            return new Vector2(interval * (index + 0.5f) - (interval * Items.Count) / 2, 0);
        }

        [ContextMenu("刷新布局")]
        public void UpdateLayout()
        {
            if (Reverse)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].LayoutPos = GetLayoutPos(Items.Count - i) + (Vector2)transform.position;
                }
            }
            else
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].LayoutPos = GetLayoutPos(i) + (Vector2)transform.position;
                }
            }
        }

        private void Start()
        {
            AddItem(pre_items);
            pre_items=null;
        }

        public enum LayoutStyle
        {
            Left,
            Center,
            Right,
        }
    }
}