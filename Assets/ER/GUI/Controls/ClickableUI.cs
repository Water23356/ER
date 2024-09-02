using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ER.GUI
{
    public class ClickableUI : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnClick;
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("点击");
            OnClick?.Invoke();
        }
    }
}