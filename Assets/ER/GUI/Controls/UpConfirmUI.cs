using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ER.GUI
{
    public class UpConfirmUI : MonoBehaviour, IPointerUpHandler
    {
        public event Action OnConfirm;

        public void OnPointerUp(PointerEventData eventData)
        {
            OnConfirm?.Invoke();
        }
    }
}