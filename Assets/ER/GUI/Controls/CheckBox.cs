using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ER.GUI
{
    public class CheckBox : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private GameObject CheckedImg;

        private bool isSelected = false;

        public event Action<bool> OnValueChanged;

        public bool Value
        {
            get => isSelected;
            set
            {
                var old = isSelected;
                isSelected = value;
                CheckedImg.SetActive(isSelected);
                if (old != value)
                {
                    OnValueChanged?.Invoke(isSelected);
                }
            }
        }

        public void ClearOnValueChangedEvent()
        {
            OnValueChanged = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Value = !isSelected;
        }
    }
}