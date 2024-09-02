using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace ER.GUI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class EMaxSizeFitter : UIBehaviour, ILayoutSelfController, ILayoutController
    {
        [HideInInspector]
        [SerializeField]
        private float m_maxWidth;

        [HideInInspector]
        [SerializeField]
        private float m_maxHeight;

        public float maxWidth { get => m_maxWidth; set => m_maxWidth = value; }
        public float maxHeight { get => m_maxHeight; set => m_maxHeight = value; }

        [NonSerialized]
        private RectTransform m_Rect;

        private DrivenRectTransformTracker m_Tracker;

        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                return false;
            }

            currentValue = newValue;
            return true;
        }

        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                {
                    m_Rect = GetComponent<RectTransform>();
                }

                return m_Rect;
            }
        }

        protected EMaxSizeFitter()
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }

        private void HandleSelfFittingAlongAxis(int axis)
        {
            float maxSize = axis == 0 ? maxWidth : maxHeight;
            float size = LayoutUtility.GetPreferredSize(m_Rect, axis);
            if (maxSize > 0 && size > maxSize) size = maxSize;
            rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, size);
        }

        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();
            HandleSelfFittingAlongAxis(0);
        }

        public virtual void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis(1);
        }

        protected void SetDirty()
        {
            if (IsActive())
            {
                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            }
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            SetDirty();
        }

#endif
    }
}