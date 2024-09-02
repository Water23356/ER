using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ER.GUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDraggableUI
    {
        private RectTransform rectTransform;
        private Canvas canvas;
        private CanvasGroup canvasGroup;

        public event Action<IDropArea> OnDragConfirm;

        public bool followMouse = true;
        public float followAlpha = 0.75f;
        private float alphaBake = 1.0f;
        public bool IsDragging { get; private set; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (followMouse)
            {
                alphaBake = canvasGroup.alpha;
                canvasGroup.alpha = followAlpha;
            }
            canvasGroup.blocksRaycasts = false;
            IsDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (followMouse)
            {
                if (canvas == null) canvas = GetComponentInParent<Canvas>();
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (followMouse)
            {
                canvasGroup.alpha = alphaBake;
            }
            canvasGroup.blocksRaycasts = true;
            IsDragging = false;
        }

        /// <summary>
        /// 当拖至指定区域时触发
        /// </summary>
        /// <param name="area">区域标识符</param>
        public void DragConfirm(IDropArea area)
        {
            OnDragConfirm?.Invoke(area);
        }
    }
}