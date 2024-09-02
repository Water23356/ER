using ER.ForEditor;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ER.GUI
{
    public class DropArea : MonoBehaviour, IDropArea
    {
        [SerializeField]
        [DisplayLabel("标识符")]
        private string area_field;

        public event Action<IDraggableUI, PointerEventData> OnDropConfirm;

        /// <summary>
        /// 区域标识符
        /// </summary>
        public string AreaName
        {
            get => area_field; set => area_field = value;
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject droppedObject = eventData.pointerDrag;
            var dui = droppedObject.GetComponent<IDraggableUI>();
            if (dui != null)
            {
                dui.DragConfirm(this);
                OnDropConfirm?.Invoke(dui, eventData);
            }
        }
    }
}