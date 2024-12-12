using System;
using UnityEngine;

namespace ER.Entity2D
{
    public abstract class Area:MonoBehaviour
    {
        public event Action<GameObject> onEnter;
        public event Action<GameObject> onExit;

        protected void InvokeEnterEvent(GameObject other)
        {
            onEnter?.Invoke(other);
        }
        protected void InvokeExitEvent(GameObject other)
        {
            onExit?.Invoke(other);
        }
    }
}