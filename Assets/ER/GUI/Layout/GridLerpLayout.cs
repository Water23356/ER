using ER.ForEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.GUI
{
    /// <summary>
    /// 网格布局, 带有插值动画(准确来说是复合的 行布局 或者 列布局)
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class GridLerpLayout : MonoBehaviour, ILerpLayoutGroup
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

        [DisplayLabel("反向布局")]
        [SerializeField]
        private bool reverse = false;

        [DisplayLabel("单元行最大元素个数")]
        [SerializeField]
        [Range(1, 20)]
        private int limitCount = 5;

        [DisplayLabel("单元行间距")]
        [SerializeField]
        private float cellRawInterval = 50f;

        [DisplayLabel("布局优先")]
        [SerializeField]
        private FirtLayoutMode firstLayout;

        [DisplayLabel("布局样式")]
        [SerializeField]
        private LayoutStyle layoutStyle = LayoutStyle.Center;

        [DisplayLabel("拓展起点")]
        [SerializeField]
        private ExpandStyle expandStyle = ExpandStyle.LeftTop;

        [Header("动画")]
        [SerializeField]
        private float lerpSpeed = 6;

        [SerializeField]
        private float maxSpeed = 500f;

        [SerializeField]
        private int layoutCount;

        /// <summary>
        /// 层数
        /// </summary>
        public int LayerCount
        {
            get
            {
                layoutCount = Mathf.CeilToInt((float)Items.Count / limitCount);
                Debug.Log($"总: {Items.Count} 限: {limitCount} lc: {layoutCount}");
                return layoutCount;
            }
        }

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
            int index_x = index % limitCount;
            int index_y = index / limitCount;
            var layerCount = Mathf.Min(Items.Count - index_y * limitCount, limitCount);
            if (firstLayout == FirtLayoutMode.Row)
                return new Vector2(GetLayoutPosX(index_x, layerCount), GetLayoutPosY(index_y, layerCount));
            return new Vector2(GetLayoutPosX(index_y, layerCount), GetLayoutPosY(index_x, layerCount));
        }

        private float GetLayoutPosX(int x_index, int layerCount)
        {
            if (firstLayout == FirtLayoutMode.Row)
            {
                var interval = AreaWidth / layerCount;
                if (maxInterval > 0 && interval > maxInterval)
                {
                    interval = maxInterval;
                }
                switch (LayoutMode)
                {
                    case LayoutStyle.LeftTop:

                        return interval * x_index - AreaWidth / 2;

                    case LayoutStyle.RightButtom:
                        return -interval * x_index + AreaWidth / 2;
                }
                return interval * (x_index + 0.5f - layerCount / 2f);
            }
            else
            {
                if (expandStyle == ExpandStyle.LeftTop)
                    return cellRawInterval * x_index;
                return -cellRawInterval * x_index;
            }
        }

        private float GetLayoutPosY(int y_index, int layerCount)
        {
            if (firstLayout == FirtLayoutMode.Row)
            {
                if (expandStyle == ExpandStyle.LeftTop)
                    return -cellRawInterval * y_index;
                return cellRawInterval * y_index;
            }
            else
            {
                var interval = AreaWidth / layerCount;
                if (maxInterval > 0 && interval > maxInterval)
                {
                    interval = maxInterval;
                }
                switch (LayoutMode)
                {
                    case LayoutStyle.LeftTop:
                        return -interval * y_index + AreaWidth / 2;

                    case LayoutStyle.RightButtom:
                        return interval * y_index - AreaWidth / 2;
                }
                return interval * (y_index + 0.5f - layerCount / 2f);
            }
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
            pre_items = null;
        }

        public enum LayoutStyle
        {
            LeftTop,
            Center,
            RightButtom,
        }

        public enum ExpandStyle
        {
            LeftTop,
            RightButtom,
        }

        public enum FirtLayoutMode
        {
            Row,
            Column,
        }
    }
}